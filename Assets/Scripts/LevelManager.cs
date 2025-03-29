using GameUtilities;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField]
    PlayerManager playerManager;
    [SerializeField]
    [Min(0.001f)]
    float startingSpeed;
    [SerializeField]
    [Min(2)]
    int startingTerrainCount;
    [SerializeField]
    List<GameObject> levelOneTerrain = new List<GameObject>();

    public float Speed { get; private set; }
    public bool GenerateTerrainOnTrigger { get; private set; }

    List<GameObject> generatedTerrain = new List<GameObject>();

    private void Awake()
    {
        Speed = startingSpeed;
        GenerateTerrainOnTrigger = false;
    }

    void Start()
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
        playerManager.ResetPlayer();
    }

    public void GenerateTerrain()
    {
        if (levelOneTerrain.Count > 0)
        {
            int index = Random.Range(0, levelOneTerrain.Count);
            var terrain = Instantiate(levelOneTerrain[index], transform);
            terrain.transform.position = transform.position;
            terrain.transform.rotation = transform.rotation;
            generatedTerrain.Add(terrain);
        }
    }
}
