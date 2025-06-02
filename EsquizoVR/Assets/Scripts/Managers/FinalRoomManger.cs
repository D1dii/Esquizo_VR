using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinalRoomManger : MonoBehaviour
{
    public NotebookController notebook;
    public LevelManager level;
    public Animator doorAnimator;


    private Collider finalRoomCollider;

    private bool doorOpened = false;
    private bool playerInFinalRoom = false;

    private void Awake()
    {
        finalRoomCollider = GetComponent<Collider>();
        finalRoomCollider.enabled = false; // Ensure the collider is enabled
    }

    public void CheckOpenFinalRoom()
    {
        if (!doorOpened && notebook.cameraShots.Count >= level.CurrentAnomaliesOnLevel)
        {
            OpenFinalDoor();
        }
    }

    private void OpenFinalDoor()
    {
        finalRoomCollider.enabled = true;
        doorOpened = true;
        doorAnimator.SetTrigger("Open");
        Debug.Log("Final door is opening!");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!playerInFinalRoom) {
            finalRoomCollider.enabled = false;


            //Placeholder, pero cuando se checkee 
            //level.PassLevel();
            //SceneManager.LoadScene("SampleScene");
            Debug.Log("Player has entered the final room!");
            doorAnimator.SetTrigger("Close");

            playerInFinalRoom = true;
        }
    }
}
