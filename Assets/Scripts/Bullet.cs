using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("Game Manager")]
    [SerializeField]
    GameManager gameManager;
    [Header("Bullet Velocity")]
    [SerializeField]
    [Tooltip("The velocity the bullet will move at")]
    float velocity;
    [Header("Shootable Selection")]
    [SerializeField]
    [Tooltip("What layers are able to be hit by the bullet")]
    LayerMask shootables;

    Vector3 previous;

    void Awake() => previous = transform.position;

    void FixedUpdate()
    {
        transform.position += Vector3.forward * (velocity * Time.fixedDeltaTime);
        // Check if any shootable objects are between the current and previous position, can hit the trigger of the explosive barrel
        if (Physics.Linecast(transform.position, previous, out RaycastHit hit, shootables, QueryTriggerInteraction.Collide))
        {
            if (hit.collider.CompareTag("Player")) gameManager.State = GameManager.GameState.Dead; // Kill the player if it hits the player
            hit.collider.gameObject.GetComponent<ExplosiveBarrel>()?.Explode(); // Explode the object if it is a barrel
        }
        previous = transform.position;
    }
}
