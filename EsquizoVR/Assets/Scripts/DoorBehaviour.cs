using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

[RequireComponent(typeof(XRGrabInteractable))]
public class DoorBehaviour : MonoBehaviour
{
    public Rigidbody doorRigidbody;
    public float pushForce = 100f;
    public float maxDistance = 1.5f;

    private XRBaseInteractor interactor;
    private bool isGrabbing = false;

    void Start()
    {
        var grab = GetComponent<XRGrabInteractable>();
        grab.selectEntered.AddListener(OnGrab);
        grab.selectExited.AddListener(OnRelease);
    }

    void FixedUpdate()
    {
        if (!isGrabbing || interactor == null) return;

        Vector3 handPos = interactor.transform.position;
        Vector3 doorPos = transform.position;

        Vector3 direction = (handPos - doorPos);
        direction.y = 0;

        if (direction.magnitude > maxDistance)
            direction = direction.normalized * maxDistance;

        doorRigidbody.AddForceAtPosition(direction.normalized * pushForce, doorPos);
    }

    void OnGrab(SelectEnterEventArgs args)
    {
        interactor = args.interactorObject.transform.GetComponent<XRBaseInteractor>();
        isGrabbing = true;
    }

    void OnRelease(SelectExitEventArgs args)
    {
        isGrabbing = false;
        interactor = null;
    }
}


