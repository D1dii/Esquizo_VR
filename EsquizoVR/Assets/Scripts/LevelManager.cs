using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public AnomalyManager anomalyManager;
    private int levelIndex = 0; 

    // Start is called before the first frame update
    void Start()
    {
        anomalyManager.SpawnAnomalies();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
