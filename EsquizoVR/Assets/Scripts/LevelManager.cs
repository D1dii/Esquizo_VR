using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;

    public AnomalyManager anomalyManager;

    [SerializeField] private List<IntIntPair> anomaliesPerLevelList = new();
    [SerializeField] private List<StringGameObjectPair> anomaliesAndModelList = new();

    [SerializeField] private Dictionary<int, int> anomaliesPerLevelDict;
    private Dictionary<string, GameObject> anomaliesAndModelDict;

    public IReadOnlyDictionary<int, int> AnomaliesPerLevel => anomaliesPerLevelDict;
    public IReadOnlyDictionary<string, GameObject> AnomaliesAndModel => anomaliesAndModelDict;

    [SerializeField] private TextNumAnomalies textNumAnomalies;

    
    [SerializeField] private int currentlevelIndex = 0;
    public int CurrentLevelIndex => currentlevelIndex;
    [SerializeField] public int CurrentAnomaliesOnLevel => anomaliesPerLevelDict[currentlevelIndex];

    public GameObject player;
    public Vector3 startPosition;

    public NotebookController notebookController;

    private void Awake()
    {
        instance = this;
        anomaliesPerLevelDict = anomaliesPerLevelList.ToDictionary(p => p.level, p => p.anomalyCount);
        anomaliesAndModelDict = anomaliesAndModelList.ToDictionary(p => p.anomalyId, p => p.modelPrefab);
    }

    private void Start()
    {
        currentlevelIndex = GameManager.instance.currentLevel;
        startPosition = player.transform.position; // Store the initial player position
        StartLevel();
    }

    public void StartLevel()
    {
        Debug.Log("Starting level " + currentlevelIndex);
        anomalyManager.SpawnAnomalies(AnomaliesPerLevel[currentlevelIndex]);

        player.transform.position = startPosition; // Reset player position for the new level

        if (textNumAnomalies != null)
        {
            textNumAnomalies.UpdateAnomalyText();
        }
        else
        {
            Debug.LogWarning("TextNumAnomalies is not assigned in LevelManager.");
        }
    }

    

    public void PassLevel()
    {
        GameManager.instance.currentLevel++;
        SceneManager.LoadScene("SampleScene");
    } 

    public void RestartLevel()
    {
        SceneManager.LoadScene("SampleScene");
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
