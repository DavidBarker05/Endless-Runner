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

    /// <summary>
    /// Indicates if the terrain can move
    /// </summary>
    public bool CanMove { get; set; }
    /// <summary>
    /// The size of the terrain on the z-axis
    /// </summary>
    public float Size { get; private set; }
    /// <summary>
    /// The obstacle rows attatched to the terrain
    /// </summary>
    public List<GameObject> ObstacleRows { get { return obstacleRows; } }
    /// <summary>
    /// The pickup rows attatched to the terrain
    /// </summary>
    public List<GameObject> PickupRows { get { return pickupRows; } }

    private void Awake() => Size = gameObject.transform.localScale.z * 10f; // Set the size
}
