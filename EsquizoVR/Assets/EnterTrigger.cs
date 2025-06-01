using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterTrigger : MonoBehaviour
{
    public MirrorAnomally stateManager;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            stateManager.playerHasEntered = true;
            Debug.Log("Player entró en ENTER. Estado activado.");
        }
    }
}
