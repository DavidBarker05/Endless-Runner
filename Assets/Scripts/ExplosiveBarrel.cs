using Array = System.Array;
using UnityEngine;

public class ExplosiveBarrel : MonoBehaviour
{
    [SerializeField]
    [Min(0.001f)]
    float explosionSize;
    [SerializeField]
    LayerMask explodables;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Bullet")) Explode();
    }

    void Explode()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionSize, explodables, QueryTriggerInteraction.Collide);
        Collider player = Array.Find<Collider>(colliders, (Collider c) => c.CompareTag("Player"));
        // To-Do: Kill Player
        Collider[] barrels = Array.FindAll<Collider>(colliders, (Collider c) => c.CompareTag("ExplosiveBarrel"));
        Array.ForEach<Collider>(barrels, b => b.gameObject.GetComponent<ExplosiveBarrel>().Explode());
    }
}
