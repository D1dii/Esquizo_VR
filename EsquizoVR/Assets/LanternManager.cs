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

    private XRGrabInteractable grabInteractable;
    [SerializeField, Tooltip("Estado actual de la linterna (solo lectura)")]
    private bool isOn = false;

    private bool isHeldByLeftHand = false;
    private InputDevice leftHandDevice;
    private bool buttonPressedLastFrame = false;

    private void Awake()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();
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
        }
    }

    private void OnSelectExited(SelectExitEventArgs args)
    {
        isHeldByLeftHand = false;
    }

    private void Update()
    {
        if (!isHeldByLeftHand) return;

        
        if (leftHandDevice.isValid && leftHandDevice.TryGetFeatureValue(CommonUsages.primaryButton, out bool buttonPressed) && buttonPressed)
        {
            ToggleFlashlight();
        }
    }

    private void ToggleFlashlight()
    {
        isOn = !isOn;
        flashlight.enabled = isOn;
    }
}
