using GameUtilities.UtilityMethods;
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

    Vector3 previous;

    void Awake() => previous = transform.position;

    void FixedUpdate()
    {
        if (GameManager.Instance.State != GameManager.GameState.Alive) return;
        transform.position -= Vector3.forward * (velocity * Time.fixedDeltaTime);
        // Check if any shootable objects are between the current and previous position, can hit the trigger of the explosive barrel
        if (Physics.Linecast(transform.position, previous, out RaycastHit hit, shootables, QueryTriggerInteraction.Collide))
        {
            if (hit.collider.CompareTag("Player") && !PlayerManager.Instance.Invulnerable)
            {
                GameManager.Instance.State = GameManager.GameState.Dead; // Kill the player if it hits the player
                PlayerManager.Instance.State = PlayerManager.AnimationState.Shot;
            }
            hit.collider.gameObject.GetComponent<ExplosiveBarrel>()?.DestroyObstacle(); // Explode the object if it is a barrel
            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("MaxGuardRange"))
            {
                GameObject maxRange = GameObjectMethods.Parent(gameObject).GetComponentInParent<SecurityGuard>().MaxRange;
                if (hit.collider.gameObject.GetInstanceID() == maxRange.GetInstanceID()) Destroy(gameObject); // Check if the max range it hits belongs to the bullet's guard and not another guard before destroying the bullet
            }
            else Destroy(gameObject); // Destroy the bullet if it hits something
        }
        previous = transform.position;
    }
}
