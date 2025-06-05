using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Pickup")) return;
        IPickup pickup = other.GetComponent<IPickup>();
        Destroy(GameUtilities.UtilityMethods.UtilityMethods.Parent(other.gameObject)); // Destroy the pickup
        PickupManager.Instance.AddPickup(pickup);
    }
}
