using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{

    public static LevelManager instance;

    public AnomalyManager anomalyManager;
    public int levelIndex = 0; 

    // Start is called before the first frame update
    public void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        anomalyManager.SpawnAnomalies();
    }

    // Update is called once per frame
    public void Update()
    {
        
    }
}
