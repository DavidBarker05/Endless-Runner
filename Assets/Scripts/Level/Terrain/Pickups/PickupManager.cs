using GameEvents = GameUtilities.GameEvents;
using System.Collections.Generic;
using UnityEngine;

public class PickupManager : MonoBehaviour
{
    public static PickupManager Instance { get; private set; }

    Dictionary<string, IPickup> activePickups = new Dictionary<string, IPickup>();

    readonly List<string> _toRemove = new List<string>();

    void Awake()
    {
        if (Instance != null && Instance != this) Destroy(gameObject);
        else Instance = this;
    }

    void FixedUpdate()
    {
        if (GameManager.Instance.State == GameManager.GameState.Paused) return;
        foreach (var keyValuePair in activePickups)
        {
            keyValuePair.Value.UseTime -= Time.fixedDeltaTime;
            if (keyValuePair.Key == "BonusPickup") GameManager.Instance.InvokeEvent(NameToEventType(keyValuePair.Key), this, LevelManager.Instance.Score + 5);
            else GameManager.Instance.InvokeEvent(NameToEventType(keyValuePair.Key), this, keyValuePair.Value);
            if (keyValuePair.Value.UseTime < 0f) _toRemove.Add(keyValuePair.Key);
        }
        foreach (string key in _toRemove)
        {
            activePickups.Remove(key);
        }
        _toRemove.Clear();
    }

    public void AddPickup(IPickup pickup)
    {
        if (activePickups.ContainsKey(pickup.Name)) activePickups[pickup.Name].UseTime = pickup.Duration;
        else activePickups.Add(pickup.Name, pickup);
    }

    public void ResetPickups()
    {
        foreach (var keyValuePair in activePickups)
        {
            keyValuePair.Value.UseTime = -1f;
            if (keyValuePair.Key == "BonusPickup") GameManager.Instance.InvokeEvent(NameToEventType(keyValuePair.Key), this, LevelManager.Instance.Score + 5);
            else GameManager.Instance.InvokeEvent(NameToEventType(keyValuePair.Key), this, keyValuePair.Value);
            _toRemove.Add(keyValuePair.Key);
        }
        foreach(string key in _toRemove)
        {
            activePickups.Remove(key);
        }
        _toRemove.Clear();
    }

    GameEvents::EventType NameToEventType(string name) => name switch
    {
        "JumpBoostPickup" => GameEvents::EventType.JumpBoostPickupEffect,
        "BonusPickup" => GameEvents::EventType.BonusPickupEffect,
        "InvulnerabilityPickup" => GameEvents::EventType.InvulnerabilityPickupEffect,
        "BossOnePickup" => GameEvents::EventType.BossOnePickupEffect,
        _ => GameEvents::EventType.Empty,
    };
}
