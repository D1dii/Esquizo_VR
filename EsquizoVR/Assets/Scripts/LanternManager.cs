// File: LanternManager.cs
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class LanternManager : MonoBehaviour
{
    [Header("Flashlight Settings")]
    public GameObject flashlight;
    public XRBaseInteractor leftHandInteractor;
    public Transform playerTransform;

    [SerializeField] private float followDistance = 1.5f;
    [SerializeField] private float followSmoothness = 5f;
    [SerializeField] private InputActionReference toggleActionLeft;

    private XRGrabInteractable grabInteractable;
    private bool isOn = true;
    private bool isHeldByLeftHand = false;

    private Transform cameraTransform;
    private Rigidbody rb;
    private Transform lanternAnchor;

    private void Awake()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();
        rb = GetComponent<Rigidbody>();
        cameraTransform = Camera.main.transform;

        lanternAnchor = new GameObject("LanternAnchor").transform;
        lanternAnchor.SetParent(null);
        lanternAnchor.localRotation = Quaternion.identity;

        toggleActionLeft.action.Enable();
        toggleActionLeft.action.performed += ctx => TryToggleFlashlight();
    }

    private void OnEnable()
    {
        grabInteractable.selectEntered.AddListener(OnSelectEntered);
        grabInteractable.selectExited.AddListener(OnSelectExited);
    }

    private void OnDisable()
    {
        grabInteractable.selectEntered.RemoveListener(OnSelectEntered);
        grabInteractable.selectExited.RemoveListener(OnSelectExited);

        toggleActionLeft.action.performed -= ctx => TryToggleFlashlight();
        toggleActionLeft.action.Disable();
    }

    private void OnSelectEntered(SelectEnterEventArgs args)
    {
        isHeldByLeftHand = args.interactorObject.transform == leftHandInteractor.transform;
        if (isHeldByLeftHand && rb != null)
        {
            rb.isKinematic = false;
        }
    }

    private void OnSelectExited(SelectExitEventArgs args)
    {
        isHeldByLeftHand = false;
    }

    private void TryToggleFlashlight()
    {
        if (!SelectFinalShotsManager.instance.selectingFinalShots)
        {
            ToggleFlashlight();
        }
    }

    private void ToggleFlashlight()
    {
        isOn = !isOn;
        flashlight.SetActive(isOn);
    }

    private void Update()
    {
        if (SelectFinalShotsManager.instance.selectingFinalShots) return;

        if (!isHeldByLeftHand)
            FollowCharacterNotGrabbed();
    }

    private void FollowCharacterNotGrabbed()
    {
        if (rb == null || cameraTransform == null) return;

        if (!rb.isKinematic) rb.isKinematic = true;

        Vector3 offset = new Vector3(-0.2f, -0.3f, 0.2f);
        Vector3 forwardYaw = Quaternion.Euler(0, cameraTransform.eulerAngles.y, 0) * offset;

        lanternAnchor.position = cameraTransform.position + forwardYaw;
        transform.position = lanternAnchor.position;

        Quaternion yawRotation = Quaternion.Euler(0f, cameraTransform.eulerAngles.y, 0f);
        transform.rotation = yawRotation;
    }
}
