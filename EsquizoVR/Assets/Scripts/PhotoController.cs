using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class PhotoController : MonoBehaviour
{
    public bool hasAnomaly = false;
    public XRSimpleInteractable interactable;

    public InputActionReference selectPhotoAction;

    public bool isSelected = false;

    public bool isBeingSelected = false;

    private void Awake()
    {
        selectPhotoAction.action.Enable();
        selectPhotoAction.action.performed += ctx => SelectPhoto();
    }

    // Start is called before the first frame update
    void Start()
    {
        interactable = GetComponent<XRSimpleInteractable>();
    }

    // Update is called once per frame
    void Update()
    {
        if (SelectFinalShotsManager.instance.selectingFinalShots)
        {
            if (interactable.isSelected && SelectFinalShotsManager.instance.currentSelections < SelectFinalShotsManager.instance.maxSelections)
            {
                selectPhotoAction.action.Enable();
            }
            
        }
        else
        {
            selectPhotoAction.action.Disable();
        }
    }

    private void SelectPhoto()
    {
        
        isSelected = !isSelected;
        if (isSelected)
        {
            SelectFinalShotsManager.instance.currentSelections++;
            Debug.Log("Photo selected: " + gameObject.name);
        }
        else
        {
            SelectFinalShotsManager.instance.currentSelections--;
            Debug.Log("Photo deselected: " + gameObject.name);
        }
    }
}
