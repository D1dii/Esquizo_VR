using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextNumAnomalies : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI anomalyText;
    [SerializeField] private AnomalyManager anomalyManager;

    private void Start()
    {
        UpdateAnomalyText();
    }

    public void UpdateAnomalyText()
    {
        if (anomalyManager == null || anomalyText == null)
        {
            Debug.LogWarning("AnomalyManager or Text is not assigned.");
            return;
        }

        int activeCount = anomalyManager.GetActiveAnomalyCount();
        anomalyText.text = $"Anomalies: {activeCount}";
    }
}

