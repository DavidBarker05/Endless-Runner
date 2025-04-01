using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField]
    float velocity;
    [SerializeField]
    LayerMask shootables;

    Vector3 previous;

    void Awake() => previous = transform.position;

    void FixedUpdate()
    {
        transform.position += Vector3.forward * velocity;
        if (Physics.Linecast(transform.position, previous, out RaycastHit hit, shootables, QueryTriggerInteraction.Collide))
        {
            if (hit.collider.CompareTag("ExplosiveBarrel")) hit.collider.gameObject.GetComponent<ExplosiveBarrel>().Explode();
            if (hit.collider.CompareTag("Player"))
            {
                // To-Do: Kill Player
            }
        }
        previous = transform.position;
    }
}
