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
    /// 
    /// </summary>
    public bool IsSuccessfulSpawn { get; private set; }

    void Awake() => IsSuccessfulSpawn = Random.Range(0f, 1f) <= Mathf.Min(baseSpawnChance + (LevelManager.Instance.IsBossActive ? bonusSpawnChance : 0f), 1f);
}
