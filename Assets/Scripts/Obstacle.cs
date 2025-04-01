using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [SerializeField]
    GameManager gameManager;

    // OnTriggerEnter works with CharacterController, but OnCollisionEnter does not, so we use OnTriggerEnter
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Do obstacle killing stuff
            // Work with game manager game states
        }
    }
}
