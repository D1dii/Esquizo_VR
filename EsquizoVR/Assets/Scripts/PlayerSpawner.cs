using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class PlayerSpawner : MonoBehaviour
{
    public XROrigin player;
    private bool hasTeleported = false;

    private void Update()
    {
        if (player != null && !hasTeleported)
        {
            TeleportPlayer();
            hasTeleported = true;
        }
    }

    private void TeleportPlayer()
    {
       player.MoveCameraToWorldLocation(transform.position);
       player.MatchOriginUpCameraForward(Vector3.up, transform.forward);   
    }

}
