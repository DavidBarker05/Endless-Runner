using System.Collections.Generic;
using UnityEngine;

public class PickupManager : MonoBehaviour
{
    public static PickupManager instance;

    Dictionary<string, IPickup> foo2 = new Dictionary<string, IPickup>();

    void Awake()
    {
        if (instance != null && instance != this) Destroy(this);
        else instance = this;
    }

    void FixedUpdate()
    {
        List<string> toRemove = new List<string>();
        foreach (var idk in foo2)
        {
            idk.Value.UseTime -= Time.fixedDeltaTime;
            idk.Value.Effect();
            if (idk.Value.UseTime < 0f) toRemove.Add(idk.Key);
        }
        foreach (string key in toRemove)
        {
            foo2.Remove(key);
        }
    }

    public void AddPickup(IPickup pickup)
    {
        if (foo2.ContainsKey(pickup.Name))
        {
            foo2[pickup.Name].UseTime = pickup.Duration;
        }
        else
        {
            foo2.Add(pickup.Name, pickup);
        }
    }
}
