using System;
using System.Collections.Generic;
using GameEvents = GameUtilities.GameEvents;
using UnityEngine;

public class PickupManager : MonoBehaviour
{
    public static PickupManager Instance { get; private set; }

    Dictionary<string, IPickup> activePickups = new Dictionary<string, IPickup>();

    void Awake()
    {
        if (Instance != null && Instance != this) Destroy(gameObject);
        else Instance = this;
    }

    void FixedUpdate()
    {
        List<string> toRemove = new List<string>();
        foreach (var keyValuePair in activePickups)
        {
            keyValuePair.Value.UseTime -= Time.fixedDeltaTime;
            GameManager.Instance.InvokeEvent(NameToEvent(keyValuePair.Key), this, keyValuePair.Value.UseTime);
            if (keyValuePair.Value.UseTime < 0f) toRemove.Add(keyValuePair.Key);
        }
        Array.ForEach<string>(toRemove.ToArray(), k => activePickups.Remove(k));
    }

    public void AddPickup(IPickup pickup)
    {
        if (activePickups.ContainsKey(pickup.Name)) activePickups[pickup.Name].UseTime = pickup.Duration;
        else activePickups.Add(pickup.Name, pickup);
    }

    public void ResetPickups()
    {
        List<string> toRemove = new List<string>();
        foreach (var keyValuePair in activePickups)
        {
            keyValuePair.Value.UseTime = -1f;
            GameManager.Instance.InvokeEvent(NameToEvent(keyValuePair.Key), this, keyValuePair.Value.UseTime);
            toRemove.Add(keyValuePair.Key);
        }
        Array.ForEach<string>(toRemove.ToArray(), k => activePickups.Remove(k));
    }

    GameEvents::EventType NameToEvent(string name) => name switch
    {
        "JumpBoostPickup" => GameEvents::EventType.JumpBoostPickupEffect,
        _ => GameEvents::EventType.Empty,
    };
}
