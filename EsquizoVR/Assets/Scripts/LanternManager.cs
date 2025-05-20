using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class LanternManager : MonoBehaviour
{
    [Header("Flashlight Settings")]
    public Light flashlight;
    public XRBaseInteractor leftHandInteractor;
    public Transform playerTransform;

    [SerializeField, Tooltip("Distancia delante de la cámara donde se colocará la linterna")]
    private float followDistance = 1.5f;

    [SerializeField, Tooltip("Velocidad con la que la linterna sigue al jugador")]
    private float followSmoothness = 5f;

    private XRGrabInteractable grabInteractable;
    [SerializeField, Tooltip("Estado actual de la linterna (solo lectura)")]
    private bool isOn = false;

    private bool isHeldByLeftHand = false;
    private InputDevice leftHandDevice;
    private bool buttonPressedLastFrame = false;

    private Transform cameraTransform;
    private bool buttonpress = false;

    private Rigidbody rb;

    private void Awake()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();
        rb = GetComponent<Rigidbody>();
        cameraTransform = Camera.main.transform;
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
    }

    private void OnSelectEntered(SelectEnterEventArgs args)
    {
        isHeldByLeftHand = args.interactorObject.transform == leftHandInteractor.transform;

        if (isHeldByLeftHand)
        {
            InputDevices.GetDeviceAtXRNode(XRNode.LeftHand).TryGetFeatureValue(CommonUsages.primaryButton, out _);
            leftHandDevice = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);

            if (rb != null) rb.isKinematic = false;
        }
    }

    public void FollowCharacterNotGrabbed()
    {
        if (cameraTransform == null || rb == null) return;

        Vector3 offset = cameraTransform.right * -0.2f + cameraTransform.up * -0.3f + cameraTransform.forward* 0.2f;
        Vector3 targetPosition = cameraTransform.position + offset;

        float distanceToTarget = Vector3.Distance(transform.position, targetPosition);

        if (distanceToTarget > 0.1f)
        {
            
            if (!rb.isKinematic) rb.isKinematic = true;

            Vector3 newPos = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * followSmoothness);
            Quaternion newRot = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(cameraTransform.forward), Time.deltaTime * followSmoothness);

            transform.position = newPos;
            transform.rotation = newRot;
        }
    }


    private void OnSelectExited(SelectExitEventArgs args)
    {
        isHeldByLeftHand = false;
    }

    private void Update()
    {
        if (!isHeldByLeftHand)
            FollowCharacterNotGrabbed();

        if (leftHandDevice.isValid &&
            leftHandDevice.TryGetFeatureValue(CommonUsages.primaryButton, out buttonpress) &&
            buttonpress && !buttonPressedLastFrame)
        {
            ToggleFlashlight();
        }

        buttonPressedLastFrame = buttonpress;
    }

    private void ToggleFlashlight()
    {
        isOn = !isOn;
        flashlight.enabled = isOn;
    }
}
