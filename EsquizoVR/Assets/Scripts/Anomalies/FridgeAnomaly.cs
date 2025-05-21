using System.Collections;
using UnityEngine;

public class FridgeAnomaly : Anomaly
{
    public float detectionRange = 3f;
    public float shakeIntensity = 0.05f;
    private Transform playerHead; 
    private Vector3 originalPosition;
    private bool isShaking = false;

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
            if (!isShaking)
            {
                StartCoroutine(Shake());
                isShaking = true;
            }
        }
        else
        {
            StopAllCoroutines();
            transform.localPosition = originalPosition;
            isShaking = false;
        }
    }

    IEnumerator Shake()
    {
        while (true)
        {
            Vector3 randomOffset = new Vector3(
                Random.Range(-shakeIntensity, shakeIntensity),
                Random.Range(-shakeIntensity, shakeIntensity),
                Random.Range(-shakeIntensity, shakeIntensity)
            );

            transform.localPosition = originalPosition + randomOffset;
            yield return null;
        }
    }

    public override void InitAnomaly()
    {
        Destroy(LevelManager.instance.AnomaliesAndModel["Fridge"]);
    }
}
