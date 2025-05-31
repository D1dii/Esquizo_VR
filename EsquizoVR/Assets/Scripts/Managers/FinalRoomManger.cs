using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalRoomManger : MonoBehaviour
{
    public NotebookController notebook;
    public LevelManager level;
    public Animator doorAnimator;

    private bool doorOpened = false;
    private bool playerInFinalRoom = false;

    public void CheckOpenFinalRoom()
    {
        if (!doorOpened && notebook.cameraShots.Count >= level.CurrentAnomaliesOnLevel)
        {
            OpenFinalDoor();
        }
    }

    private void OpenFinalDoor()
    {
        doorOpened = true;
        doorAnimator.SetTrigger("Open");
        Debug.Log("Final door is opening!");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!playerInFinalRoom) {
            
            Debug.Log("Player has entered the final room!");
            doorAnimator.SetTrigger("Close");                
            
            playerInFinalRoom = true; 
        }
    }
}
