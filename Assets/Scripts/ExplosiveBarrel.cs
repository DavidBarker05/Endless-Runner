using Array = System.Array;
using UnityEngine;
using GameUtilities;

public class ExplosiveBarrel : MonoBehaviour
{
    [SerializeField]
    GameManager gameManager;
    [SerializeField]
    [Min(0.001f)]
    float explosionSize;
    [SerializeField]
    LayerMask explodables;

    ParticleSystem explosion;

    void Awake() => explosion = UtilityMethods.GetParent(gameObject).GetComponentInChildren<ParticleSystem>();

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) Explode();
    }

    public void Explode()
    {
        gameObject.layer = 2;
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionSize, explodables, QueryTriggerInteraction.Collide);
        Collider player = Array.Find<Collider>(colliders, (Collider c) => c.CompareTag("Player"));
        gameManager.State = GameManager.GameState.Dead;
        Collider[] barrels = Array.FindAll<Collider>(colliders, (Collider c) => c.CompareTag("ExplosiveBarrel"));
        Array.ForEach<Collider>(barrels, b => b.gameObject.GetComponent<ExplosiveBarrel>().Explode());
        gameObject.SetActive(false);
        explosion.Play();
        Destroy(UtilityMethods.GetParent(gameObject), explosion.main.duration);
    }
}
