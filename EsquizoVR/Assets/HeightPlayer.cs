using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeightPlayer : MonoBehaviour
{
    public CharacterController characterController;

    private void Update()
    {
        characterController.height = 0.5f;
        characterController.center = new Vector3(0, 0.25f, 0);
    }
}
