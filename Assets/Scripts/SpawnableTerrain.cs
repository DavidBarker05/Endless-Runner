using System.Collections.Generic;
using UnityEngine;

public class SpawnableTerrain : MonoBehaviour
{
    [SerializeField]
    List<GameObject> obstacleRows = new List<GameObject>();
    [SerializeField]
    List<GameObject> pickupRows = new List<GameObject>();

    public bool CanMove { get; set; }
    public float Size { get; private set; }
    public List<GameObject> ObstacleRows { get { return obstacleRows; } }
    public List<GameObject> PickupRows { get { return pickupRows; } }

    private void Awake()
    {
        Size = gameObject.transform.localScale.z * 10f;
    }
}
