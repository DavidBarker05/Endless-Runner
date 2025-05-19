using UnityEngine;

/// <summary>
/// Manages the chance of a pickup in a row
/// </summary>
public class PickupRow : MonoBehaviour
{
    [SerializeField]
    [Range(0f, 1f)]
    float baseSpawnChance;
    [SerializeField]
    [Tooltip("")]
    [Range(0f, 1f)]
    float bonusSpawnChance;

    /// <summary>
    /// The spawn chance of a pickup in this row
    /// </summary>
    public float SpawnChance => Mathf.Clamp(baseSpawnChance + (LevelManager.Instance.IsBossActive ? bonusSpawnChance: 0f), 0f, 1f);
}
