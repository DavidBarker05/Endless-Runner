using GameUtilities;
using System.Linq;
using UnityEngine;

public class JumpBoostPickup : MonoBehaviour, IPickup
{
    public PickupManager PickupManager { get; set; }
    public PlayerManager PlayerManager { get; set; }
    public string Name => "JumpBoostPickup";
    public float Duration => 5f;

    public void Effect(float useTime) => PlayerManager.ExtraJumpHeight = useTime >= 0f ? 10f : 0f;

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        if (PickupManager.PickupExists(Name)) PickupManager.ResetUseTime(Name, Duration);
        else PickupManager.AddPickup(Name, Duration, Effect);
        Destroy(UtilityMethods.Parent(gameObject));
    }
}
