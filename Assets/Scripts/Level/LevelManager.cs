using GameUtilities.UtilityMethods;
using GameEvents = GameUtilities.GameEvents;
using System.Collections.Generic;
using Array = System.Array;
using UnityEngine;

/// <summary>
/// Manages level generation and score
/// </summary>
public class LevelManager : MonoBehaviour, GameEvents::IEventListener
{
    public static LevelManager Instance { get; private set; }

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
    [SerializeField]
    [Tooltip("The location where the level one boss spawns")]
    Transform levelOneBossSpawnLocation;
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
    [SerializeField]
    [Tooltip("")]
    LevelOneBoss levelOneBoss;

    const float MAX_SPEED = 0.5f; // The max speed that everything can reach

    List<GameObject> generatedTerrain = new List<GameObject>();
    bool isLevelStart = true;
    GameObject lastGeneratedTerrain;
    int lastGeneratedObstacleCount;
    float bossTimer;
    Boss boss;

    float _speed;
    /// <summary>
    /// The speed that all terrain moves
    /// </summary>
    public float Speed { get => _speed; private set => _speed = Mathf.Clamp(value, startingSpeed, MAX_SPEED); }
    /// <summary>
    /// Bool that indicates if terrain can be generated when they go into eachother's triggers
    /// </summary>
    public bool GenerateTerrainOnTrigger { get; private set; }
    /// <summary>
    /// The current score that the player has
    /// </summary>
    public int Score { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public bool IsBossActive { get; private set; }

    // What terrain is currently able to be spawned
    List<GameObject> PossibleTerrain => isLevelStart ? levelOneStartingTerrain : levelOneTerrain;

    void Awake()
    {
        if (Instance != null && Instance != this) Destroy(gameObject);
        else Instance = this;
    }

    void Start()
    {
        GameManager.Instance.AddListener(GameEvents::EventType.ObstaclePassed, this);
        GameManager.Instance.AddListener(GameEvents::EventType.BonusPickupEffect, this);
        GameManager.Instance.AddListener(GameEvents::EventType.BossOneSpawn, this);
        GameManager.Instance.AddListener(GameEvents::EventType.BossOneBeaten, this);
    }

    void FixedUpdate()
    {
        if (GameManager.Instance.State != GameManager.GameState.Alive) return;
        GameObject[] moveableTerrain = Array.FindAll<GameObject>(generatedTerrain.ToArray(), (GameObject t) => t.GetComponent<SpawnableTerrain>().CanMove); // Find all terrain that can move
        Array.ForEach<GameObject>(moveableTerrain, t => t.transform.position -= UtilityMethods.ZVector(Speed)); // Move all the terrain that can move
        Speed += startingSpeed / 30f * Time.fixedDeltaTime; // Increase speed
        bossTimer += Time.fixedDeltaTime;
        if (bossTimer >= 30f) GameManager.Instance.InvokeEvent(!IsBossActive ? GameEvents::EventType.BossOneSpawn : GameEvents::EventType.BossOneBeaten, this);
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

    void OnDestroy()
    {
        GameManager.Instance.RemoveListener(GameEvents::EventType.ObstaclePassed, this);
        GameManager.Instance.RemoveListener(GameEvents::EventType.BonusPickupEffect, this);
        GameManager.Instance.RemoveListener(GameEvents::EventType.BossOneSpawn, this);
        GameManager.Instance.RemoveListener(GameEvents::EventType.BossOneBeaten, this);
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
        SpawnableTerrain _terrain = terrain.GetComponent<SpawnableTerrain>();
        if (terrain.CompareTag("SecurityDoor")) lastGeneratedObstacleCount = 2;
        if (levelOneObstacles.Count > 0 && _terrain.ObstacleRows.Count > 0)
        {
            foreach (GameObject row in _terrain.ObstacleRows)
            {
                ObstacleRow _row = row.GetComponent<ObstacleRow>();
                lastGeneratedObstacleCount = lastGeneratedObstacleCount == 2 || (_row.NumberOfObstacles == 2 && lastGeneratedObstacleCount > 0) ? 0 : _row.NumberOfObstacles;
                if (lastGeneratedObstacleCount == 0) continue;
                _row.HasObstacles = true;
                SpawnTerrainObjects(row, levelOneObstacles, _row.NumberOfObstacles);
            }
        }
        if (levelOnePickups.Count > 0 && _terrain.PickupRows.Count > 0)
        {
            foreach (GameObject row in _terrain.PickupRows)
            {
                if (!row.GetComponent<PickupRow>().IsSuccessfulSpawn) continue;
                List<GameObject> pickups = new List<GameObject>();
                foreach (GameObject pickup in levelOnePickups)
                {
                    if (pickup.name != "Boss One Pickup") pickups.Add(pickup);
                    if (pickup.name != "Boss One Pickup" || !IsBossActive) continue;
                    for (int i = 0; i < 5; i++) pickups.Add(pickup);
                }
                if (pickups.Count == 0) continue;
                SpawnTerrainObjects(row, pickups);
            }
        }
        generatedTerrain.Add(terrain);
        lastGeneratedTerrain = terrain;
    }

    void SpawnTerrainObjects(GameObject row, List<GameObject> spawnables, int depth = 1, int lane = -1)
    {
        if (depth == 0) return;
        int _lane;
        do _lane = Random.Range(0, lanes.Length); while (_lane == lane);
        int index = Random.Range(0, spawnables.Count);
        Vector3 spawnPos = UtilityMethods.YZVector(row.transform.position) + UtilityMethods.XVector(lanes[_lane].position);
        GameObject spawnable = Instantiate(spawnables[index], row.transform);
        spawnable.transform.position = spawnPos;
        SpawnTerrainObjects(row, spawnables, depth - 1, _lane);
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
        "LitTerrain" => !terrain.CompareTag("LitTerrain"),
        "UnlitTerrain" => !terrain.CompareTag("UnlitTerrain"),
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
        GameManager.Instance.State = GameManager.GameState.Alive;
        Array.ForEach<GameObject>(generatedTerrain.ToArray(), t => DestroyTerrain(t));
        isLevelStart = true;
        lastGeneratedTerrain = null;
        lastGeneratedObstacleCount = 0;
        Speed = startingSpeed;
        GenerateTerrainOnTrigger = false;
        GameManager.Instance.InvokeEvent(GameEvents::EventType.ObstaclePassed, this, 0);
        Score = 0;
        bossTimer = 0f;
        if (boss != null) Destroy(boss.gameObject);
        GenerateStartingTerrain();
        PlayerManager.Instance.ResetPlayer();
        PickupManager.Instance.ResetPickups();
    }

    public void OnEvent(GameEvents::EventType eventType, Component sender, object param = null)
    {
        switch (eventType)
        {
            case GameEvents::EventType.ObstaclePassed:
                Score = (int)param;
                break;
            case GameEvents::EventType.BonusPickupEffect:
                Score = (int)param;
                break;
            case GameEvents::EventType.BossOneSpawn:
                bossTimer = 0f;
                if (levelOneBoss == null) return;
                boss = Instantiate(levelOneBoss);
                boss.transform.position = levelOneBossSpawnLocation.position;
                bossTimer = 5f; // Make boss disappear 5 seconds before end of level
                IsBossActive = true;
                break;
            case GameEvents::EventType.BossOneBeaten:
                bossTimer = 0f;
                if (levelOneBoss == null) return;
                bossTimer = -5f; // Fix the timing issue cause from making disappear early
                IsBossActive = false;
                BossOnePickup[] bossOnePickups = FindObjectsByType<BossOnePickup>(FindObjectsSortMode.None);
                Array.ForEach<BossOnePickup>(bossOnePickups, b => GameObject.Destroy(UtilityMethods.Parent(b.gameObject)));
                break;
        }
    }
}
