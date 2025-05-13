using System.Collections.Generic;
using UnityEngine;

public class PickupManager : MonoBehaviour
{
    public static PickupManager instance;

    Dictionary<string, IPickup> activePickups = new Dictionary<string, IPickup>();

    void Awake()
    {
        if (instance != null && instance != this) Destroy(this);
        else instance = this;
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
        foreach (string key in toRemove)
        {
            activePickups.Remove(key);
        }
    }

    public void AddPickup(IPickup pickup)
    {
        if (activePickups.ContainsKey(pickup.Name))
        {
            activePickups[pickup.Name].UseTime = pickup.Duration;
        }
        else
        {
            activePickups.Add(pickup.Name, pickup);
        }
    }
}
