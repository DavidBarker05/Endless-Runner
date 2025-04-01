using Array = System.Array;
using UnityEngine;
using GameUtilities;
using Unity.VisualScripting;

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
        if (player != null) gameManager.State = GameManager.GameState.Dead;
        Array.ForEach<Collider>(colliders, c => c.gameObject.GetComponent<ExplosiveBarrel>()?.Explode());
        gameObject.SetActive(false);
        explosion.Play();
        Destroy(UtilityMethods.GetParent(gameObject), explosion.main.duration);
    }
}
