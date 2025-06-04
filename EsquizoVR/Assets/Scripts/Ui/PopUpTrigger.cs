using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopUpTrigger : MonoBehaviour
{
    public enum PopupType 
    { 
        Welcome, 
        Anomalies,
        UseObjects, 
        GoToFinalRoom, 
        SelectionShots, 
        None
    }
    public PopupType popupToShow = PopupType.None;

    private bool hasSpawned = false;

    private void OnTriggerEnter(Collider other)
    {
        if (hasSpawned) return;

        switch (popupToShow)
        {
            case PopupType.Welcome:
                UiController.Instance.ShowPopup("Welcome", "Welcome! You are an inspector from the police department. Some neighbors have reported paranormal activity inside this house at night. Your mission is to search for anomalies and document them with your camera.", false);
                break;
            case PopupType.Anomalies:
                UiController.Instance.ShowPopup("Anomalies", "Take a look at this place during the day to recognize all the anomalies that may appear at night. Some of them are difficult to spot, and others are very, very... scary.", false);
                break;
            case PopupType.UseObjects:
                UiController.Instance.ShowPopup("UseObjects", "Use your camera to capture anomalies by taking a shot with the trigger. Try to properly aim at what you think is the anomaly. You can review the shots in your notebook.", false );
                break;
            case PopupType.SelectionShots:
                UiController.Instance.ShowPopup("SelectionShots", "To progress through the game, select the shots where you believe an anomaly is present. If you're correct, you’ll advance to the next level. Otherwise, you’ll repeat this level with different anomalies.", true);
                break;
        }
        hasSpawned = true;
    }
}
