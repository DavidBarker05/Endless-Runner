using UnityEngine;

public class PickupRow : MonoBehaviour
{
    [SerializeField]
    [Range(0f, 1f)]
    float baseSpawnChance;

    public float SpawnChance {  get { return baseSpawnChance; } }
}
