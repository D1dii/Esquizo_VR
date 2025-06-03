using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

[RequireComponent(typeof(XRSimpleInteractable))]
public class PhotoController : MonoBehaviour
{
    public bool hasAnomaly = false;
    private InputAction selectPhotoAction;

    public bool isSelected = false;

    private Vector3 originalScale;
    private Quaternion fixedRotation;
    private XRSimpleInteractable interactable;

    private bool isHovering = false;
    private float lastHoverTime;
    private float hoverTimeout = 0.5f;

    public void Initialize(InputActionReference inputRef)
    {
        selectPhotoAction = inputRef.action;
        selectPhotoAction.Enable();
        selectPhotoAction.performed += OnSelectPerformed;
    }

    private void Awake()
    {
        originalScale = transform.localScale;
        fixedRotation = Quaternion.Euler(90, 0, 90);

        interactable = GetComponent<XRSimpleInteractable>();
        interactable.selectMode = InteractableSelectMode.Multiple;

        interactable.hoverEntered.AddListener(ctx => OnHoverEnter());
    }

    private void OnDestroy()
    {
        if (selectPhotoAction != null)
        {
            selectPhotoAction.performed -= OnSelectPerformed;
            selectPhotoAction.Disable();
        }

        interactable.hoverEntered.RemoveAllListeners();
    }

    private void OnHoverEnter()
    {
        isHovering = true;
        lastHoverTime = Time.time;
        if (!isSelected)
        {
            transform.DOScale(originalScale * 6.1f, 0.2f);
        }
        CancelInvoke(nameof(DisableHover));
        Invoke(nameof(DisableHover), hoverTimeout);
    }

    private void DisableHover()
    {
        if (!isSelected)
        {
            isHovering = false;
            transform.DOScale(originalScale, 0.2f);
        }
    }

    private void OnSelectPerformed(InputAction.CallbackContext context)
    {
        if (!SelectFinalShotsManager.instance.selectingFinalShots)
            return;

        if (!isHovering)
            return;

        if (isSelected || SelectFinalShotsManager.instance.currentSelections >= SelectFinalShotsManager.instance.maxSelections)
            return;

        isSelected = true;
        SelectFinalShotsManager.instance.currentSelections++;
        transform.DOScale(originalScale * 1.15f, 0.2f);
        Debug.Log("Photo selected: " + gameObject.name);
    }

    private void LateUpdate()
    {
        if (SelectFinalShotsManager.instance.selectingFinalShots)
        {
            transform.rotation = fixedRotation;
        }
    }

    public void ResetPhoto()
    {
        isSelected = false;
        transform.localScale = originalScale;
        transform.rotation = fixedRotation;
    }
}