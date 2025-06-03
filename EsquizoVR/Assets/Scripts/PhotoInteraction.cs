using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PhotoInteraction : MonoBehaviour
{

    public SelectFinalShotsManager manager;
    private bool isSelected = false;
    private Vector3 originalScale;
    private bool isHovering = false;

    [SerializeField] private InputActionReference triggerLeft;
    [SerializeField] private InputActionReference triggerRight;

    void Start()
    {
        originalScale = transform.localScale;
        triggerLeft.action.Enable();
        triggerRight.action.Enable();
    }

    private void OnDestroy()
    {
        triggerLeft.action.Disable();
        triggerRight.action.Disable();
    }

    void OnTriggerEnter(Collider other)
    {
        if (!isSelected && !isHovering)
        {
            transform.DOScale(originalScale * 1.1f, 0.2f);
            isHovering = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (!isSelected && isHovering)
        {
            transform.DOScale(originalScale, 0.2f);
            isHovering = false;
        }
    }

    void Update()
    {
        if (!isSelected && isHovering && (triggerLeft.action.WasPressedThisFrame() || triggerRight.action.WasPressedThisFrame()))
        {
            isSelected = true;
            manager.currentSelections++;
            transform.DOScale(originalScale * 1.1f, 0.2f);
        }
    }

    public void ResetState()
    {
        isSelected = false;
        isHovering = false;
        transform.localScale = originalScale;
    }


}
