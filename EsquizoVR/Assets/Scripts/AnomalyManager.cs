using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnomalyManager : MonoBehaviour
{
    public List<IAnomaly> anomalies = new List<IAnomaly>();
    public int anomalyCount = 0;


    private List<IAnomaly> activeAnomalies = new List<IAnomaly>();


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnAnomalies()
    {
        for (int i = 0; i < anomalies.Count; i++)
        {
            var anomaly = Instantiate(anomalies[i] as MonoBehaviour).gameObject;
            anomaly.transform.SetParent(this.transform);
            anomaly.transform.position = anomalies[i].spawnPos.position;
        }
    }

    public void InitAnomaly()
    {
        foreach (IAnomaly anomaly in anomalies)
        {
            anomaly.OnAnomalyStart();
        }
    }


}
