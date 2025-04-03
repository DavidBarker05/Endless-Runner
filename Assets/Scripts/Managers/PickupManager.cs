using System.Collections.Generic;
using UnityEngine;
using static IPickup;

public class PickupManager : MonoBehaviour
{
    readonly List<string> names = new List<string>();
    readonly List<float> useTimes = new List<float>();
    readonly List<Effect> effects = new List<Effect>();

    void FixedUpdate()
    {
        for (int i = 0; i < names.Count; i++)
        {
            useTimes[i] -= Time.deltaTime;
            effects[i](useTimes[i]);
            if (useTimes[i] < 0f) RemovePickup(i);
        }
    }

    public bool PickupExists(string name) => names.Contains(name);

    public void AddPickup(string name, float duration, Effect effect)
    {
        names.Add(name);
        useTimes.Add(duration);
        effects.Add(effect);
    }

    void RemovePickup(int index)
    {
        names.RemoveAt(index);
        useTimes.RemoveAt(index);
        effects.RemoveAt(index);
    }

    public void ResetUseTime(string name, float duration) => useTimes[names.IndexOf(name)] = duration;
}
