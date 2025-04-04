using GameUtilities;
using UnityEngine;

/// <summary>
/// Makes a bullet move forward and checks if any collisions occured then does the appropriate actions
/// </summary>
public class Bullet : MonoBehaviour
{
    [Header("Bullet Velocity")]
    [SerializeField]
    [Tooltip("The velocity the bullet will move at")]
    float velocity;
    [Header("Shootable Selection")]
    [SerializeField]
    [Tooltip("The layers that are able to be hit by the bullet")]
    LayerMask shootables;

    GameManager gameManager;
    Vector3 previous;

    void Awake() => previous = transform.position;

    void FixedUpdate()
    {
        transform.position += Vector3.forward * (velocity * Time.fixedDeltaTime);
        // Check if any shootable objects are between the current and previous position, can hit the trigger of the explosive barrel
        if (Physics.Linecast(transform.position, previous, out RaycastHit hit, shootables, QueryTriggerInteraction.Collide))
        {
            if (hit.collider.CompareTag("Player"))
            {
                gameManager = UtilityMethods.Parent(UtilityMethods.Parent(gameObject)).GetComponentInChildren<Obstacle>().GameManager;
                gameManager.State = GameManager.GameState.Dead; // Kill the player if it hits the player
            }
            hit.collider.gameObject.GetComponent<ExplosiveBarrel>()?.Explode(); // Explode the object if it is a barrel
            if (hit.collider != null) Destroy(gameObject); // If the bullet collided with something then destroy it
        }
        previous = transform.position;
    }
}
