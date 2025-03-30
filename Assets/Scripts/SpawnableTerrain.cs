using System.Collections.Generic;
using UnityEngine;

public class SpawnableTerrain : MonoBehaviour
{
    public List<GameObject> obstacleRows = new List<GameObject>();

    public bool CanMove { get; set; }
    public float Size { get; private set; }

    private void Awake()
    {
        Size = gameObject.transform.localScale.z * 10f;
    }
}
