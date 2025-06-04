using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    public CharacterController characterController;
    private bool haschanged = false;
    private int frameCount = 0;

    // Start is called before the first frame update  
    void Start()
    {
    }

    // Update is called once per frame  
    void Update()
    {
        frameCount++;
        //if (!haschanged && frameCount == 10)
        //{
            characterController.center = new Vector3(0, 0.25f, 0);
            characterController.height = 0.5f;
            haschanged = true;
        ///}
    }
}
