using UnityEngine;

public class ExplosiveBarrel : MonoBehaviour
{
    [SerializeField]
    [Min(0.001f)]
    float explosionSize;
    [SerializeField]
    LayerMask explodableObjects;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("Bullet")) Explode();
    }

    void Explode()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionSize, explodableObjects, QueryTriggerInteraction.Collide);
        foreach (Collider collider in colliders)
        {
            if (collider.gameObject.CompareTag("Player"))
            {
                // Kill
            }
            else if (collider.gameObject.CompareTag("ExplosiveBarrel")) collider.gameObject.GetComponent<ExplosiveBarrel>().Explode();
        }
    }
}
