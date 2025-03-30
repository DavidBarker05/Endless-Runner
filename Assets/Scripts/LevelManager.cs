using GameUtilities;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [Header("Lanes")]
    [SerializeField]
    [Tooltip("The three lanes everything spawns on")]
    Transform[] lanes = new Transform[3];
    [Header("Player")]
    [SerializeField]
    [Tooltip("Player manager")]
    PlayerManager playerManager;
    [Header("Starting Values")]
    [SerializeField]
    [Tooltip("The speed that terrain moves at the start of the game")]
    [Min(0.001f)]
    float startingSpeed;
    [SerializeField]
    [Tooltip("The amount of starting terrain that spawns at the start of the level")]
    [Min(2)]
    int startingTerrainCount;
    [Header("Level One Spawnables")]
    [SerializeField]
    [Tooltip("List of spawnable terrain at the start of level one")]
    List<GameObject> levelOneStartingTerrain = new List<GameObject>();
    [SerializeField]
    [Tooltip("List of spawnable terrain for level one of the start")]
    List<GameObject> levelOneTerrain = new List<GameObject>();
    [SerializeField]
    [Tooltip("List of spawnable obstacles for level one")]
    List<GameObject> levelOneObstacles = new List<GameObject>();

    public float Speed { get; private set; }
    public bool GenerateTerrainOnTrigger { get; private set; }

    List<GameObject> generatedTerrain = new List<GameObject>();
    bool isLevelStart = true;
    GameObject lastGeneratedTerrain;

    private void Awake()
    {
        Speed = startingSpeed;
        GenerateTerrainOnTrigger = false;
    }

    void Start()
    {
        GenerateStartingTerrain();
    }

    void Update()
    {
    }

    private void FixedUpdate()
    {
        foreach (var terrain in generatedTerrain)
        {
            if (terrain.GetComponent<SpawnableTerrain>().CanMove) terrain.transform.position -= UtilityMethods.ZVector(Speed);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("FrontTerrain"))
        {
            generatedTerrain.Remove(other.gameObject.transform.parent.gameObject);
            Destroy(other.gameObject.transform.parent.gameObject);
            if (!GenerateTerrainOnTrigger)
            {
                GenerateTerrainOnTrigger = true;
                GenerateTerrain();
            }
        }
    }

    public void ResetGame()
    {
        Speed = startingSpeed;
        GenerateTerrainOnTrigger = false;
        isLevelStart = true;
        playerManager.ResetPlayer();
        generatedTerrain.Clear();
        GenerateStartingTerrain();
    }

    public void GenerateTerrain()
    {
        if (PossibleTerrain.Count == 0) return;
        int index = Random.Range(0, PossibleTerrain.Count);
        while (!IsValidTerrain(PossibleTerrain[index]))
        {
            index = Random.Range(0, PossibleTerrain.Count);
        }
        var terrain = Instantiate(PossibleTerrain[index], transform);
        terrain.transform.position = transform.position;
        terrain.transform.rotation = transform.rotation;
        if (terrain.GetComponent<SpawnableTerrain>().ObstacleRows.Count > 0)
        {
            foreach (var obstacleRow in terrain.GetComponent<SpawnableTerrain>().ObstacleRows)
            {

            }
        }
        generatedTerrain.Add(terrain);
        lastGeneratedTerrain = terrain;
    }

    public void GenerateStartingTerrain()
    {
        for (int i = 0; i < startingTerrainCount; i++)
        {
            GenerateTerrain();
        }
        for (int i = 0; i < startingTerrainCount - 1; i++)
        {
            for (int j = startingTerrainCount - 1; j > i; j--)
            {
                if (j == startingTerrainCount - 1) generatedTerrain[i].transform.position -= UtilityMethods.ZVector(generatedTerrain[j].GetComponent<SpawnableTerrain>().Size / 2f);
                else generatedTerrain[i].transform.position -= UtilityMethods.ZVector(generatedTerrain[j].GetComponent<SpawnableTerrain>().Size);
            }
            generatedTerrain[i].transform.position -= UtilityMethods.ZVector(generatedTerrain[i].GetComponent<SpawnableTerrain>().Size / 2f);
        }
        generatedTerrain[0].GetComponent<SpawnableTerrain>().CanMove = true;
        isLevelStart = false;
    }

    List<GameObject> PossibleTerrain
    {
        get
        {
            return isLevelStart ? levelOneStartingTerrain : levelOneTerrain;
        }
    }

    bool IsValidTerrain(GameObject terrain)
    {
        if (lastGeneratedTerrain == null) return true;
        if (lastGeneratedTerrain.CompareTag("SecurityDoor")) return !terrain.CompareTag("SecurityDoor");
        return true;
    }
}
