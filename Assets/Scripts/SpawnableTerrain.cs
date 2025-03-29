using UnityEngine;

public class SpawnableTerrain : MonoBehaviour
{
    public bool CanMove { get; set; }
    public float Size { get; private set; }

    private void Awake()
    {
        Size = gameObject.transform.localScale.z * 10f;
    }

    void Start()
    {
    }

    void Update()
    {
    }
}
