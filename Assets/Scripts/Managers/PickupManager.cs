using System.Collections.Generic;
using UnityEngine;
using static IPickup;

public class PickupManager : MonoBehaviour
{
    List<string> names = new List<string>();
    List<float> useTimes = new List<float>();
    List<Effect> effects = new List<Effect>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (names.Count == 0) return;
        effects[0](useTimes[0]);
    }

    public bool PickupExists(string name) => names.Contains(name);

    public void AddPickup(string name, float duration, Effect effect)
    {
        names.Add(name);
        useTimes.Add(duration);
        effects.Add(effect);
    }

    public void ResetUseTime(string name, float duration) => useTimes[names.IndexOf(name)] = duration;
}
