using GameUtilities;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Pickup"))
        {
            IPickup pickup = other.GetComponent<IPickup>();
            Destroy(UtilityMethods.Parent(other.gameObject)); // Destroy the pickup
            PickupManager.instance.AddPickup(pickup);
        }
    }
}
