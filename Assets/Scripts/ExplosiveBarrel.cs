using GameUtilities;
using UnityEngine;

public class ExplosiveBarrel : MonoBehaviour
{
    [SerializeField]
    [Min(0.001f)]
    float explosionSize;
    [SerializeField]
    LayerMask explodableObjects;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Bullet")) Explode();
    }

    void Explode()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionSize, explodableObjects, QueryTriggerInteraction.Collide);
        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("Player"))
            {
                // To-Do: Kill
                continue;
            }
            collider.gameObject.GetComponent<ExplosiveBarrel>().Explode();
        }
    }
}
