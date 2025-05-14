using GameUtilities;
using System.Collections.Generic;
using Array = System.Array;
using UnityEngine;

/// <summary>
/// Manages level generation and score
/// </summary>
public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;

    [Header("Spawn Management")]
    [SerializeField]
    [Tooltip("The location where tiles spawn on")]
    Transform spawnLocation;
    [SerializeField]
    [Tooltip("The three lanes everything spawns on")]
    Transform[] lanes = new Transform[3];
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

    const float MAX_SPEED = 0.5f; // The max speed that everything can reach

    List<GameObject> generatedTerrain = new List<GameObject>();
    bool isLevelStart = true;
    GameObject lastGeneratedTerrain;
    int lastGeneratedObstacleCount;

    /// <summary>
    /// The speed that all terrain moves
    /// </summary>
    public float Speed { get; private set; }
    /// <summary>
    /// Bool that indicates if terrain can be generated when they go into eachother's triggers
    /// </summary>
    public bool GenerateTerrainOnTrigger { get; private set; }
    /// <summary>
    /// The current score that the player has
    /// </summary>
    public int Score { get; set; }
    // What terrain is currently able to be spawned
    List<GameObject> PossibleTerrain => isLevelStart ? levelOneStartingTerrain : levelOneTerrain;

    private void Awake()
    {
        if (instance != null && instance != this) Destroy(this);
        else instance = this;
    }

    void FixedUpdate()
    {
        if (GameManager.instance.State != GameManager.GameState.Alive) return;
        GameObject[] moveableTerrain = Array.FindAll<GameObject>(generatedTerrain.ToArray(), (GameObject t) => t.GetComponent<SpawnableTerrain>().CanMove); // Find all terrain that can move
        Array.ForEach<GameObject>(moveableTerrain, t => t.transform.position -= UtilityMethods.ZVector(Speed)); // Move all the terrain that can move
        Speed += startingSpeed / 30f * Time.fixedDeltaTime; // Increase speed
        Speed = Mathf.Clamp(Speed, startingSpeed, MAX_SPEED); // Make sure doesn't exceed max speed
    }

    // The trigger is behind the player
    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("FrontTerrain")) return; // If it isn't the front of a terrain that collides then skip it
        DestroyTerrain(UtilityMethods.Parent(other.gameObject)); // Destroy the terrain
        if (GenerateTerrainOnTrigger) return; // If terrain is generated on trigger then don't manually generate new terrain
        GenerateTerrainOnTrigger = true; // Update the bool
        GenerateTerrain(); // Manually generate a new piece of terrain
    }

    public void GenerateTerrain()
    {
        if (PossibleTerrain.Count == 0) return; // Make sure that there is terrain to generate
        int terrainIndex = Random.Range(0, PossibleTerrain.Count); // Get a random terrain to generate
        while (!IsValidTerrain(PossibleTerrain[terrainIndex])) // Make sure valid
        {
            terrainIndex = Random.Range(0, PossibleTerrain.Count);
        }
        var terrain = Instantiate(PossibleTerrain[terrainIndex], transform);
        terrain.transform.position = spawnLocation.position;
        // Make sure can generate obstacles
        if (levelOneObstacles.Count > 0 && terrain.GetComponent<SpawnableTerrain>().ObstacleRows.Count > 0)
        {
            foreach (var obstacleRow in terrain.GetComponent<SpawnableTerrain>().ObstacleRows) // Go through all obstacle rows
            {
                int numberOfObstacles = Random.Range(obstacleRow.GetComponent<ObstacleRow>().MinimumObstacles, obstacleRow.GetComponent<ObstacleRow>().MaximumObstacles + 1); // Get random number of obstacles to generate
                if (numberOfObstacles == 0 || lastGeneratedObstacleCount == 2) // If number of obstacles to generate is 0, or the previous number was 2 (which would make the game too hard if generate again)
                {
                    lastGeneratedObstacleCount = 0;
                    continue; // Skip this obstacle row
                }
                obstacleRow.GetComponent<ObstacleRow>().HasObstacles = true;
                int firstSpawnLane = Random.Range(0, lanes.Length); // Lane to spawn in
                int obstacleIndex = Random.Range(0, levelOneObstacles.Count); // Random obstaclw
                Vector3 spawnPos = UtilityMethods.YZVector(obstacleRow.transform.position) + UtilityMethods.XVector(lanes[firstSpawnLane].position);
                var firstObstacle = Instantiate(levelOneObstacles[obstacleIndex], obstacleRow.transform);
                firstObstacle.transform.position = spawnPos;
                if (numberOfObstacles == 1 || lastGeneratedObstacleCount == 1) // If number of obstacles is 1, or the previous number was 1 then can't generate 2 obstacles
                {
                    lastGeneratedObstacleCount = 1;
                    continue; // Go to next obstacle row
                }
                lastGeneratedObstacleCount = 2;
                int secondSpawnLane = Random.Range(0, lanes.Length); // Next lane to spawn in
                while (firstSpawnLane == secondSpawnLane) // Make sure empty
                {
                    secondSpawnLane = Random.Range(0, lanes.Length);
                }
                obstacleIndex = Random.Range(0, levelOneObstacles.Count); // Random obstacle
                spawnPos = UtilityMethods.YZVector(obstacleRow.transform.position) + UtilityMethods.XVector(lanes[secondSpawnLane].position);
                var secondObstacle = Instantiate(levelOneObstacles[obstacleIndex], obstacleRow.transform);
                secondObstacle.transform.position = spawnPos;
            }
        }
        else lastGeneratedObstacleCount = 0;
        // Make sure can generate pickups
        if (levelOnePickups.Count > 0 && terrain.GetComponent<SpawnableTerrain>().PickupRows.Count > 0)
        {
            foreach (var pickupRow in terrain.GetComponent<SpawnableTerrain>().PickupRows) // Go through all pickups rows
            {
                float spawnRoll = Random.Range(0f, 1f); // Random nummber to see if can spawn
                if (spawnRoll <= pickupRow.GetComponent<PickupRow>().SpawnChance) // Check if number is in the spawn chance
                {
                    int lane = Random.Range(0, lanes.Length); // Lane to spawn in
                    int pickupIndex = Random.Range(0, levelOnePickups.Count); // Random pickup
                    Vector3 spawnPos = UtilityMethods.YZVector(pickupRow.transform.position) + UtilityMethods.XVector(lanes[lane].position);
                    var pickup = Instantiate(levelOnePickups[pickupIndex], pickupRow.transform);
                    pickup.transform.position = spawnPos;
                }
            }
        }
        if (terrain.CompareTag("SecurityDoor")) lastGeneratedObstacleCount = 2;
        generatedTerrain.Add(terrain);
        lastGeneratedTerrain = terrain;
    }

    // Generate starting terrain based off of starting variables
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

    // Check what terrain is valid based off of last generated terrain tag
    bool IsValidTerrain(GameObject terrain) => lastGeneratedTerrain?.tag switch
    {
        "SecurityDoor" => !terrain.CompareTag("SecurityDoor"),
        _ => true,
    };

    // Destroys terrain and removes it from the list
    void DestroyTerrain(GameObject terrain)
    {
        generatedTerrain.Remove(terrain);
        Destroy(terrain);
    }

    // Resets game to start
    public void ResetGame()
    {
        GameManager.instance.State = GameManager.GameState.Alive;
        Array.ForEach<GameObject>(generatedTerrain.ToArray(), t => DestroyTerrain(t));
        isLevelStart = true;
        lastGeneratedTerrain = null;
        lastGeneratedObstacleCount = 0;
        Speed = startingSpeed;
        GenerateTerrainOnTrigger = false;
        Score = 0;
        GenerateStartingTerrain();
        PlayerManager.instance.ResetPlayer();
    }
}
