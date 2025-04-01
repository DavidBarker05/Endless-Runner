using GameUtilities;
using System.Collections.Generic;
using Array = System.Array;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField]
    GameManager gameManager;
    [SerializeField]
    Transform spawnLocation;
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
    [SerializeField]
    [Tooltip("List of spawnable pickups for level one")]
    List<GameObject> levelOnePickups = new List<GameObject>();

    const float MAX_SPEED = 0.5f;

    List<GameObject> generatedTerrain = new List<GameObject>();
    bool isLevelStart = true;
    GameObject lastGeneratedTerrain;
    int lastGeneratedObstacleCount;

    public float Speed { get; private set; }
    public bool GenerateTerrainOnTrigger { get; private set; }
    public int Score { get; set; }
    List<GameObject> PossibleTerrain => isLevelStart ? levelOneStartingTerrain : levelOneTerrain;

    void FixedUpdate()
    {
        if (!(gameManager.State == GameManager.GameState.Alive)) return;
        GameObject[] moveableTerrain = Array.FindAll<GameObject>(generatedTerrain.ToArray(), (GameObject t) => t.GetComponent<SpawnableTerrain>().CanMove);
        Array.ForEach<GameObject>(moveableTerrain, t => t.transform.position -= UtilityMethods.ZVector(Speed));
        Speed += startingSpeed / 30f * Time.fixedDeltaTime;
        Speed = Mathf.Clamp(Speed, startingSpeed, MAX_SPEED);
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("FrontTerrain")) return;
        DestroyTerrain(UtilityMethods.GetParent(other.gameObject));
        if (GenerateTerrainOnTrigger) return;
        GenerateTerrainOnTrigger = true;
        GenerateTerrain();
    }

    public void GenerateTerrain()
    {
        if (PossibleTerrain.Count == 0) return;
        int terrainIndex = Random.Range(0, PossibleTerrain.Count);
        while (!IsValidTerrain(PossibleTerrain[terrainIndex]))
        {
            terrainIndex = Random.Range(0, PossibleTerrain.Count);
        }
        var terrain = Instantiate(PossibleTerrain[terrainIndex], transform);
        terrain.transform.position = spawnLocation.position;
        if (levelOneObstacles.Count > 0 && terrain.GetComponent<SpawnableTerrain>().ObstacleRows.Count > 0)
        {
            foreach (var obstacleRow in terrain.GetComponent<SpawnableTerrain>().ObstacleRows)
            {
                int numberOfObstacles = Random.Range(obstacleRow.GetComponent<ObstacleRow>().MinimumObstacles, obstacleRow.GetComponent<ObstacleRow>().MaximumObstacles + 1);
                if (numberOfObstacles == 0 || lastGeneratedObstacleCount == 2)
                {
                    lastGeneratedObstacleCount = 0;
                    continue;
                }
                obstacleRow.GetComponent<ObstacleRow>().HasObstacles = true;
                int firstSpawnLane = Random.Range(0, lanes.Length);
                int obstacleIndex = Random.Range(0, levelOneObstacles.Count);
                Vector3 spawnPos = UtilityMethods.YZVector(obstacleRow.transform.position) + UtilityMethods.XVector(lanes[firstSpawnLane].position);
                var firstObstacle = Instantiate(levelOneObstacles[obstacleIndex], obstacleRow.transform);
                firstObstacle.transform.position = spawnPos;
                if (numberOfObstacles == 1 || lastGeneratedObstacleCount == 1)
                {
                    lastGeneratedObstacleCount = 1;
                    continue;
                }
                lastGeneratedObstacleCount = 2;
                int secondSpawnLane = Random.Range(0, lanes.Length);
                while (firstSpawnLane == secondSpawnLane)
                {
                    secondSpawnLane = Random.Range(0, lanes.Length);
                }
                obstacleIndex = Random.Range(0, levelOneObstacles.Count);
                spawnPos = UtilityMethods.YZVector(obstacleRow.transform.position) + UtilityMethods.XVector(lanes[secondSpawnLane].position);
                var secondObstacle = Instantiate(levelOneObstacles[obstacleIndex], obstacleRow.transform);
                secondObstacle.transform.position = spawnPos;
            }
        }
        else lastGeneratedObstacleCount = 0;
        if (levelOnePickups.Count > 0 && terrain.GetComponent<SpawnableTerrain>().PickupRows.Count > 0)
        {
            foreach (var pickupRow in terrain.GetComponent<SpawnableTerrain>().PickupRows)
            {
                float spawnRoll = Random.Range(0f, 1f);
                if (spawnRoll <= pickupRow.GetComponent<PickupRow>().SpawnChance)
                {
                    int lane = Random.Range(0, lanes.Length);
                    int pickupIndex = Random.Range(0, levelOnePickups.Count);
                    Vector3 spawnPos = UtilityMethods.YZVector(pickupRow.transform.position) + UtilityMethods.XVector(lanes[lane].position);
                    var pickup = Instantiate(levelOnePickups[pickupIndex], pickupRow.transform);
                    pickup.transform.position = spawnPos;
                }
            }
        }
        generatedTerrain.Add(terrain);
        lastGeneratedTerrain = terrain;
    }

    void GenerateStartingTerrain()
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

    bool IsValidTerrain(GameObject terrain) => lastGeneratedTerrain?.tag switch
    {
        "SecurityDoor" => !terrain.CompareTag("SecurityDoor"),
        _ => true,
    };

    void DestroyTerrain(GameObject terrain)
    {
        generatedTerrain.Remove(terrain);
        Destroy(terrain);
    }

    public void ResetGame()
    {
        gameManager.State = GameManager.GameState.Alive;
        Array.ForEach<GameObject>(generatedTerrain.ToArray(), t => DestroyTerrain(t));
        isLevelStart = true;
        lastGeneratedTerrain = null;
        lastGeneratedObstacleCount = 0;
        Speed = startingSpeed;
        GenerateTerrainOnTrigger = false;
        Score = 0;
        GenerateStartingTerrain();
        playerManager.ResetPlayer();
    }
}
