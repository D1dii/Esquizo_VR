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

    [SerializeField]
    private float followDistance = 1.5f;

    [SerializeField]
    private float followSmoothness = 5f;

    private XRGrabInteractable grabInteractable;
    private bool isOn = false;
    private bool isHeldByLeftHand = false;
    private InputDevice leftHandDevice;
    private bool buttonPressedLastFrame = false;
    private bool buttonpress = false;

    private Transform cameraTransform;
    private Rigidbody rb;
    private Transform lanternAnchor;

    private void Awake()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();
        rb = GetComponent<Rigidbody>();
        cameraTransform = Camera.main.transform;

        lanternAnchor = new GameObject("LanternAnchor").transform;
        lanternAnchor.SetParent(cameraTransform);
        lanternAnchor.localPosition = new Vector3(-0.2f, -0.3f, 0.2f);
        lanternAnchor.localRotation = Quaternion.identity;
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

    private void FollowCharacterNotGrabbed()
    {
        if (lanternAnchor == null || rb == null) return;

        if (!rb.isKinematic) rb.isKinematic = true;

        transform.position = lanternAnchor.position;
        transform.rotation = lanternAnchor.rotation;
    }

    private void OnSelectExited(SelectExitEventArgs args)
    {
        isHeldByLeftHand = false;
    }

    private void Update()
    {

        if (SelectFinalShotsManager.instance.selectingFinalShots == true) { return; }

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