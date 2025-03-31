using UnityEngine;

public class Obstacle : MonoBehaviour
{
    // OnTriggerEnter works with CharacterController, but OnCollisionEnter does not, so we use OnTriggerEnter
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Do obstacle killing stuff
        }
    }
}
