using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitTrigger : MonoBehaviour
{
    public MirrorAnomally stateManager;
    public AudioSource audioSource;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (stateManager.playerHasEntered)
            {
                Debug.Log("Player entró en EXIT después de ENTER. Reproduciendo audio.");
                audioSource?.Play();
                //stateManager.playerHasEntered = false; // Reiniciar
            }
            else
            {
                Debug.Log("Player entró en EXIT sin haber pasado por ENTER.");
            }
        }
    }
}
