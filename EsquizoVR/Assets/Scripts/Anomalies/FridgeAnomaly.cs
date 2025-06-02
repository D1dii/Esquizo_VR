using System.Collections;
using UnityEngine;

public class FridgeAnomaly : Anomaly
{
    public float detectionRange = 2f;
    public float shakeIntensity = 0.05f;

    private Transform playerHead;
    private Vector3 originalPosition;

    public Animator animator;

    private bool hasTriggered = false;

    void Start()
    {
        originalPosition = transform.localPosition;

        if (Camera.main != null)
        {
            playerHead = Camera.main.transform;
        }
        else
        {
            Debug.LogWarning("No se encontró la cámara principal. Asegúrate de que la Main Camera está correctamente etiquetada.");
        }
    }

    void Update()
    {
        if (playerHead == null) return;

        float distance = Vector3.Distance(playerHead.position, transform.position);

        if (distance < detectionRange)
        {
            if (!hasTriggered)
            {
                animator.SetBool("Open", true);
                hasTriggered = true;
            }
        }
        else
        {
            if (hasTriggered)
            {
                animator.SetBool("Open", false);
                hasTriggered = false;
            }
        }
    }

    public override void InitAnomaly()
    {
        Destroy(LevelManager.instance.AnomaliesAndModel["Fridge"]);
    }
}