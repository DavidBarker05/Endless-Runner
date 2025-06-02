using System;
using System.Collections.Generic;
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
            keyValuePair.Value.Effect();
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
            keyValuePair.Value.UseTime = 0f;
            keyValuePair.Value.Effect();
            toRemove.Add(keyValuePair.Key);
        }
        Array.ForEach<string>(toRemove.ToArray(), k => activePickups.Remove(k));
    }
}
