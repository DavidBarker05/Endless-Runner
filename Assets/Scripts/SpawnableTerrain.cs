using System.Collections.Generic;
using UnityEngine;

public class SpawnableTerrain : MonoBehaviour
{
    [SerializeField]
    List<GameObject> obstacleRows = new List<GameObject>();

    public bool CanMove { get; set; }
    public float Size { get; private set; }
    public List<GameObject> ObstacleRows { get { return obstacleRows; } }

    private void Awake()
    {
        Size = gameObject.transform.localScale.z * 10f;
    }
}
