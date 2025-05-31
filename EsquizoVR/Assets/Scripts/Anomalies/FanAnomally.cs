using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FanAnomally : Anomaly
{
    private Animator animator;
    private Collider fanCollider;
    public override void InitAnomaly()
    {
        Destroy(LevelManager.instance.AnomaliesAndModel["Fan"]);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (animator == null)
            {
                animator = GetComponent<Animator>();
            }
            if (fanCollider == null)
            {
                fanCollider = GetComponent<Collider>();
            }

            if (animator != null && fanCollider != null)
            {
                animator.SetTrigger("Fall");
                fanCollider.enabled = false; // Desactiva el collider para evitar múltiples activaciones
            }
        }
    }
}
