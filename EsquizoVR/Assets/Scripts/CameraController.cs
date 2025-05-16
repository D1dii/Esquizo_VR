using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class CameraController : MonoBehaviour
{
    public Transform playerTransform;

    public Camera camera;

    public XRGrabInteractable interactable;

    public InputActionReference cameraShotAction;

    private void Awake()
    {
        cameraShotAction.action.Enable();
        cameraShotAction.action.performed += ctx => TakeCameraShot();
    }

    private void OnDestroy()
    {
        cameraShotAction.action.performed -= ctx => TakeCameraShot();
        cameraShotAction.action.Disable();
    }

    // Start is called before the first frame update
    public void Start()
    {
        interactable = GetComponent<XRGrabInteractable>();
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

    public void TakeCameraShot()
    {
        if (interactable.isSelected)
        {
            Debug.Log("Camera Shot Taken!");

            Ray ray = camera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.collider.CompareTag("Anomaly"))
                {
                    Debug.Log("Anomaly detected!");
                }
                else
                {
                    Debug.Log($"Raycast hit: {hit.collider.name} at position {hit.point}");
                }
            }
            else
            {
                Debug.Log("Raycast did not hit anything.");
            }
        }
    }
}
