using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class CameraController : MonoBehaviour
{
    public Transform playerTransform;

    public XRGrabInteractable interactable;

    public InputActionReference cameraShotAction;

    public int resWidth = 1920;
    public int resHeight = 1080;

    public Camera cameraComponent;

    public void Awake()
    {
        cameraShotAction.action.Enable();
        cameraShotAction.action.performed += ctx => TakeCameraShot();
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
        transform.position = new Vector3(playerTransform.position.x + 0.5f, transform.position.y, playerTransform.position.z - 0.5f);
    }

    public static string ScreenShotName(int width, int height)
    {
        return string.Format("{0}/Screenshots/screenshot_{1}x{2}_{3}.png",
            Application.dataPath, width, height, System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss"));
    }

    public void TakeCameraShot()
    {
        if (interactable.isSelected)
        {
            RenderTexture renderTexture = new RenderTexture(resWidth, resHeight, 24);
            cameraComponent.targetTexture = renderTexture;
            Texture2D screenShot = new Texture2D(resWidth, resHeight, TextureFormat.RGB24, false);
            cameraComponent.Render();
            RenderTexture.active = renderTexture;
            screenShot.ReadPixels(new Rect(0, 0, resWidth, resHeight), 0, 0);
            cameraComponent.targetTexture = null;
            RenderTexture.active = null; // JC: added to avoid errors
            Destroy(renderTexture);
            byte[] bytes = screenShot.EncodeToPNG();
            string filePath = ScreenShotName(resWidth, resHeight);
            System.IO.File.WriteAllBytes(filePath, bytes);
            Debug.Log(string.Format("Took screenshot to: {0}", filePath));
        }
    }
}
