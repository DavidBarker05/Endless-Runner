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
        for (int i = 0; i < names.Count; i++) // Go through all pickups
        {
            useTimes[i] -= Time.deltaTime; // Count down their use times
            effects[i](useTimes[i]); // Use their effect
            if (useTimes[i] < 0f) RemovePickup(i); // Remove them if the use time is below 0
        }
    }

    /// <summary>
    /// Checks if the pickup already exists in the list
    /// </summary>
    /// <param name="name">The name of the pickup to check for</param>
    /// <returns>Bool stating if the pickup exists</returns>
    public bool PickupExists(string name) => names.Contains(name);

    /// <summary>
    /// Add a pickup
    /// </summary>
    /// <param name="name">Name of the pickup</param>
    /// <param name="duration">Duration of the pickup</param>
    /// <param name="effect">Effect of the pickup</param>
    public void AddPickup(string name, float duration, Effect effect)
    {
        names.Add(name);
        useTimes.Add(duration);
        effects.Add(effect);
    }

    // Remove a pickup at that index
    void RemovePickup(int index)
    {
        names.RemoveAt(index);
        useTimes.RemoveAt(index);
        effects.RemoveAt(index);
    }

    /// <summary>
    /// Reset the use time of a pick
    /// </summary>
    /// <param name="name">Name of pickup to reset</param>
    /// <param name="duration">Duration to set back to</param>
    public void ResetUseTime(string name, float duration) => useTimes[names.IndexOf(name)] = duration;
}
