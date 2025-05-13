using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class LanternManager : MonoBehaviour
{
    public class Linterna : MonoBehaviour
    {
        public Light spotLight;  
        public XRController controller; 

        private bool isOn = true;

        void Start()
        {
            if (spotLight == null)
            {
                Debug.LogError("No se ha asignado la luz de la linterna.");
            }
        }

        void Update()
        {
      
            if (controller)
            {
                if (controller.selectInteractionState.activatedThisFrame)
                {
                    ToggleLight();
                }
            }
        }

        void ToggleLight()
        {
            isOn = !isOn;
            spotLight.enabled = isOn;
        }
    } 
}
