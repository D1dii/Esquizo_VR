using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AnomalyManager : MonoBehaviour
{
    public List<Anomaly> anomalies = new();

    private List<Anomaly> activeAnomalies = new();

    public void SpawnAnomalies(int numAnomalies)
    {
        if (numAnomalies > anomalies.Count)
        {
            Debug.LogError("Not enough unique anomalies to spawn");
            return;
        }

        List<Anomaly> shuffled = new(anomalies);
        Shuffle(shuffled);

        var selected = shuffled.Take(numAnomalies);

        foreach (var anomalyPrefab in selected)
        {
            var instance = Instantiate(anomalyPrefab as MonoBehaviour).gameObject;
            instance.transform.position = anomalyPrefab.SpawnPos;

            Anomaly anomalyInstance = instance.GetComponent<Anomaly>();
            activeAnomalies.Add(anomalyInstance);
        }

        InitAnomalies();
    }

    public void InitAnomalies()
    {
        foreach (var anomaly in activeAnomalies)
        {
            anomaly.InitAnomaly();
        }
    }
    private void Shuffle<T>(List<T> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int rand = Random.Range(0, i + 1);
            (list[i], list[rand]) = (list[rand], list[i]);
        }
    }
}
