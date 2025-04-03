using GameUtilities;
using System.Linq;
using UnityEngine;

public class JumpBoostPickup : MonoBehaviour, IPickup
{
    /// <summary>
    /// Pickup manager
    /// </summary>
    public PickupManager PickupManager { get; set; }
    /// <summary>
    /// Player manager
    /// </summary>
    public PlayerManager PlayerManager { get; set; }
    /// <summary>
    /// Jump boost name
    /// </summary>
    public string Name => "JumpBoostPickup";
    /// <summary>
    /// Jump boost duration
    /// </summary>
    public float Duration => 5f;

    /// <summary>
    /// Jump boost effect
    /// </summary>
    /// <param name="useTime">Current use time of the jump boost</param>
    public void Effect(float useTime) => PlayerManager.ExtraJumpHeight = useTime >= 0f ? 5f : 0f; // If greater than 0 give jump boost else don't

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return; // If not collision with player skip
        if (PickupManager.PickupExists(Name)) PickupManager.ResetUseTime(Name, Duration); // If effect already in use then reset its time
        else PickupManager.AddPickup(Name, Duration, Effect); // If not then add it
        Destroy(UtilityMethods.Parent(gameObject)); // Destroy the pickup
    }
}
