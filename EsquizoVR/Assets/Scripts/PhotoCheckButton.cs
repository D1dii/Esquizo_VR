using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Collider))]
public class PhotoCheckButton : MonoBehaviour
{
    //[SerializeField] private InputActionProperty validateActionLeft;
    //[SerializeField] private InputActionProperty validateActionRight;

    //[SerializeField] private float handAboveThreshold = 0.1f;
    //[SerializeField] private Transform detectionAreaTop;

    //private bool handInside = false;
    //private bool isReady = false;

    //private void OnEnable()
    //{
    //    validateActionLeft.action.Enable();
    //    validateActionRight.action.Enable();
    //}

    //private void OnDisable()
    //{
    //    validateActionLeft.action.Disable();
    //    validateActionRight.action.Disable();
    //}

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (!SelectFinalShotsManager.instance.selectingFinalShots) return;

    //    Vector3 otherPos = other.transform.position;
    //    if (otherPos.y > detectionAreaTop.position.y + handAboveThreshold)
    //    {
    //        handInside = true;
    //        isReady = true;
    //        transform.DOScale(transform.localScale * 1.05f, 0.2f);
    //    }
    //}

    //private void OnTriggerExit(Collider other)
    //{
    //    handInside = false;
    //    isReady = false;
    //    transform.DOScale(Vector3.one, 0.2f);
    //}

    //private void Update()
    //{
    //    if (!isReady || !SelectFinalShotsManager.instance.selectingFinalShots) return;

    //    if (validateActionLeft.action.WasPressedThisFrame() || validateActionRight.action.WasPressedThisFrame())
    //    {
    //        ValidateSelection();
    //        isReady = false;
    //    }
    //}

    //private void ValidateSelection()
    //{
    //    var manager = SelectFinalShotsManager.instance;

    //    if (manager.currentSelections != manager.maxSelections)
    //    {
    //        Debug.Log("Not enough photos selected.");
    //        return;
    //    }

    //    bool allCorrect = true;
    //    manager.finalShots.Clear();

    //    foreach (var shot in manager.notebookController.cameraShots)
    //    {
    //        var controller = shot.GetComponent<PhotoController>();
    //        if (controller != null && controller.isSelected)
    //        {
    //            manager.finalShots.Add(controller.hasAnomaly);
    //        }
    //    }

    //    foreach (bool isAnomaly in manager.finalShots)
    //    {
    //        if (!isAnomaly)
    //        {
    //            allCorrect = false;
    //            break;
    //        }
    //    }

    //    if (allCorrect)
    //    {
    //        manager.TriggerFadeAndComplete();
    //    }
    //    else
    //    {
    //        manager.ResetSelection();
    //        Debug.Log("Incorrect selection. Try again.");
    //    }
    //}
}
