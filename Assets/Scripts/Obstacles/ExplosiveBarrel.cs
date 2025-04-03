using Array = System.Array;
using UnityEngine;
using GameUtilities;

/// <summary>
/// Handles the explosion of an explosive barrel
/// </summary>
public class ExplosiveBarrel : MonoBehaviour
{
    [Header("Explosion")]
    [SerializeField]
    [Tooltip("The radius of the explosion")]
    [Min(0.001f)]
    float explosionSize;
    [SerializeField]
    [Tooltip("The layers that are affected by the explosion")]
    LayerMask explodables;

    ParticleSystem explosion; // Explosion particle system

    GameManager gameManager;

    void Awake() => explosion = UtilityMethods.Parent(gameObject).GetComponentInChildren<ParticleSystem>();
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) Explode();
    }

    public void Explode()
    {
        gameManager = GetComponent<Obstacle>().GameManager;
        gameObject.layer = 2; // Make sure current barrel isn't in the list of barrels that need to explode
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionSize, explodables, QueryTriggerInteraction.Collide); // Find all explodable objects
        Collider player = Array.Find<Collider>(colliders, (Collider c) => c.CompareTag("Player"));
        if (player != null) gameManager.State = GameManager.GameState.Dead; // If the player is in the explosion kill them
        Array.ForEach<Collider>(colliders, c => c.gameObject.GetComponent<ExplosiveBarrel>()?.Explode()); // If another barrel is in the explosion explode it
        gameObject.SetActive(false); // Hide the barrel mesh and collider
        explosion.Play();
        Destroy(UtilityMethods.Parent(gameObject), explosion.main.duration); // Destroy the explosive barrel once the animation ends
    }
}
