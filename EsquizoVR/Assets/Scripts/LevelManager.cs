using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;

    public AnomalyManager anomalyManager;

    [SerializeField] private List<IntIntPair> anomaliesPerLevelList = new();
    [SerializeField] private List<StringGameObjectPair> anomaliesAndModelList = new();

    private Dictionary<int, int> anomaliesPerLevelDict;
    private Dictionary<string, GameObject> anomaliesAndModelDict;

    public IReadOnlyDictionary<int, int> AnomaliesPerLevel => anomaliesPerLevelDict;
    public IReadOnlyDictionary<string, GameObject> AnomaliesAndModel => anomaliesAndModelDict;

    private int currentlevelIndex = 1;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        anomaliesPerLevelDict = anomaliesPerLevelList.ToDictionary(p => p.level, p => p.anomalyCount);
        anomaliesAndModelDict = anomaliesAndModelList.ToDictionary(p => p.anomalyId, p => p.modelPrefab);
    }

    private void Start()
    {
        StartLevel();
    }

    public void StartLevel()
    {
        Debug.Log("Starting level " + currentlevelIndex);
        anomalyManager.SpawnAnomalies(AnomaliesPerLevel[currentlevelIndex]);
    }

    public int CurrentLevelIndex => currentlevelIndex;

    public void PassLevel()
    {
        currentlevelIndex++;
    }
}


[System.Serializable]
public class IntIntPair
{
    public int level;
    public int anomalyCount;
}

[System.Serializable]
public class StringGameObjectPair
{
    public string anomalyId;
    public GameObject modelPrefab;
}
