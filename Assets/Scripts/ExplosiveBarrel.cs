using UnityEngine;

public class ExplosiveBarrel : MonoBehaviour
{
    [SerializeField]
    [Min(0.001f)]
    float explosionSize;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Bullet")) Explode();
    }

    void Explode()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionSize, LayerMask.NameToLayer("Explodable"));
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
