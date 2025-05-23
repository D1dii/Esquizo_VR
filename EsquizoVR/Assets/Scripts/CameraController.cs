using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class CameraController : MonoBehaviour
{
    public Transform playerTransform;
    private Rigidbody rb;

    public XRGrabInteractable interactable;

    public InputActionReference cameraShotAction;

    [SerializeField, Tooltip("Velocidad con la que la camara sigue al jugador")]
    private float followSmoothness = 5f;

    public int resWidth = 1920;
    public int resHeight = 1080;

    public Camera cameraComponent;

    public NotebookController notebookController;

    public GameObject photoPrefab;

    public void Awake()
    {
        cameraShotAction.action.Enable();
        cameraShotAction.action.performed += ctx => TakeCameraShot();
        rb = GetComponent<Rigidbody>();
    }

    public void OnDestroy()
    {
        cameraShotAction.action.performed -= ctx => TakeCameraShot();
        cameraShotAction.action.Disable();
    }

    // Start is called before the first frame update
    public void Start()
    {
        interactable = GetComponent<XRGrabInteractable>();
        cameraComponent = GetComponent<Camera>();
    }

    // Update is called once per frame
    public void Update()
    {

        if (!interactable.isSelected)
        {
            FollowCharacterNotGrabbed();
        }

        

    }

    public void FollowCharacterNotGrabbed()
    {
        if (playerTransform == null || rb == null) return;

        Vector3 offset = playerTransform.right * + 0.2f + playerTransform.up * -0.3f + playerTransform.forward * 0.4f;
        Vector3 targetPosition = playerTransform.position + offset;

        float distanceToTarget = Vector3.Distance(transform.position, targetPosition);

        if (distanceToTarget > 0.1f)
        {

            if (!rb.isKinematic) rb.isKinematic = true;

            Vector3 newPos = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * followSmoothness);
            Quaternion newRot = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(playerTransform.forward), Time.deltaTime * followSmoothness);

            transform.position = newPos;
            transform.rotation = newRot;
        }
    }

    public static string ScreenShotName(int width, int height)
    {
        switch(LevelManager.instance.CurrentLevelIndex)
        {
            case 1:
                return string.Format("{0}/Screenshots/Level_1/screenshot_{1}x{2}_{3}.png",
                Application.dataPath, width, height, System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss"));
            case 2:
                return string.Format("{0}/Screenshots/Level_2/screenshot_{1}x{2}_{3}.png",
                Application.dataPath, width, height, System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss"));
            case 3:
                return string.Format("{0}/Screenshots/Level_3/screenshot_{1}x{2}_{3}.png",
                Application.dataPath, width, height, System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss"));
            case 4:
                return string.Format("{0}/Screenshots/Level_4/screenshot_{1}x{2}_{3}.png",
                Application.dataPath, width, height, System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss"));
            case 5:
                return string.Format("{0}/Screenshots/Level_5/screenshot_{1}x{2}_{3}.png",
                Application.dataPath, width, height, System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss"));
            default:
                break;
        }

        return "No photo";
        
    }

    public void TakeCameraShot()
    {
        if (interactable.isSelected)
        {
            RenderTexture renderTexture = new RenderTexture(resWidth, resHeight, 24);
            cameraComponent.targetTexture = renderTexture;
            Texture2D screenShot = new Texture2D(resWidth, resHeight, TextureFormat.RGBA32, false);
            cameraComponent.Render();
            RenderTexture.active = renderTexture;
            screenShot.ReadPixels(new Rect(0, 0, resWidth, resHeight), 0, 0);
            screenShot.Apply(); // Aplica los cambios al Texture2D
            cameraComponent.targetTexture = null;
            RenderTexture.active = null;
            Destroy(renderTexture);

            // Crear el objeto con el Sprite
            var cameraShot = Instantiate(photoPrefab, new Vector3(0, 0, 0), Quaternion.identity);
            Sprite sprite = Sprite.Create(screenShot, new Rect(0, 0, screenShot.width, screenShot.height), new Vector2(0.5f, 0.5f));
            cameraShot.GetComponent<SpriteRenderer>().sprite = sprite;

            // Configurar el SpriteRenderer
            cameraShot.GetComponent<SpriteRenderer>().sortingOrder = 1;

            // Agregar a la lista
            notebookController.cameraShots.Add(cameraShot);

            // Guardar el archivo
            byte[] bytes = screenShot.EncodeToPNG();
            string filePath = ScreenShotName(resWidth, resHeight);
            System.IO.File.WriteAllBytes(filePath, bytes);

            Debug.Log(string.Format("Took screenshot to: {0}", filePath));
            if(HasShotAnAnomaly())
            {
                Debug.Log("Anomaly detected in the photo!");
            }
            else
            {
                Debug.Log("No anomalies detected in the photo.");
            }
        }
    }

    //Funcion para comprobar si el raycast de la camara colisiona con una anomalia
    //Esta funcion deberia almacenar ese dato en la informacion de la foto que se vera en el notebook 
    public bool HasShotAnAnomaly()
    {

        Vector3[] directions = {
           cameraComponent.transform.forward,
           cameraComponent.transform.forward + cameraComponent.transform.right * 0.1f,
           cameraComponent.transform.forward - cameraComponent.transform.right * 0.1f,
           cameraComponent.transform.forward + cameraComponent.transform.up * 0.1f,
           cameraComponent.transform.forward - cameraComponent.transform.up * 0.1f
        };

        foreach (var direction in directions)
        {
            Ray ray = new Ray(cameraComponent.transform.position, direction.normalized);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                var anomaly = hit.collider.GetComponent<Anomaly>();
                if (anomaly != null)
                {
                    float distance = Vector3.Distance(cameraComponent.transform.position, hit.point);
                    if (distance <= 3.0f)
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }
}
