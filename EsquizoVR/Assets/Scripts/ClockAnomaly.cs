using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClockAnomaly : MonoBehaviour, IAnomaly
{
    public Transform spawnPos { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

    public void OnAnomalyStart()
    {
        audioSource.PlayOneShot(audioClip);
    }

    public AudioSource audioSource;
    public AudioClip audioClip;

}
