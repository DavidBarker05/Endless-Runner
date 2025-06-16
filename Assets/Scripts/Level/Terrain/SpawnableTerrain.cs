using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Code that manages everything to do with a spawnable terrain and its rows
/// </summary>
public class SpawnableTerrain : MonoBehaviour
{
    [SerializeField]
    List<GameObject> obstacleRows = new List<GameObject>();
    [SerializeField]
    List<GameObject> pickupRows = new List<GameObject>();
    [SerializeField]
    Transform lowestPoint;

    /// <summary>
    /// Indicates if the terrain can move
    /// </summary>
    public bool CanMove { get; set; }
    /// <summary>
    /// The size of the terrain on the z-axis
    /// </summary>
    public float Size => GetComponent<Collider>().bounds.size.z;
    /// <summary>
    /// The obstacle rows attatched to the terrain
    /// </summary>
    public List<GameObject> ObstacleRows => obstacleRows;
    /// <summary>
    /// The pickup rows attatched to the terrain
    /// </summary>
    public List<GameObject> PickupRows => pickupRows;
    /// <summary>
    /// 
    /// </summary>
    public Transform LowestPoint => lowestPoint;
}
