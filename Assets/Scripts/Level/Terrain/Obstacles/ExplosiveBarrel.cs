using Array = System.Array;
using UnityEngine;

/// <summary>
/// Handles the explosion of an explosive barrel
/// </summary>
public class ExplosiveBarrel : Destructable
{
    [Header("Explosion")]
    [SerializeField]
    [Tooltip("The radius of the explosion")]
    [Min(0.001f)]
    float explosionSize;
    [SerializeField]
    [Tooltip("The layers that are affected by the explosion")]
    LayerMask explodables;
    
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !PlayerManager.Instance.Invulnerable) DestroyObstacle();
        if (other.CompareTag("LevelOneBoss")) base.DestroyObstacle();
    }

    public override void DestroyObstacle()
    {
        gameObject.layer = 2; // Make sure current barrel isn't in the list of barrels that need to explode
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionSize, explodables, QueryTriggerInteraction.Collide); // Find all explodable objects
        Collider player = Array.Find<Collider>(colliders, (Collider c) => c.CompareTag("Player"));
        if (player != null && !PlayerManager.Instance.Invulnerable)
        {
            GameManager.Instance.State = GameManager.GameState.Dead; // If the player is in the explosion kill them
            PlayerManager.Instance.State = PlayerManager.AnimationState.Exploded;
        }
        Array.ForEach<Collider>(colliders, c => c.gameObject.GetComponent<ExplosiveBarrel>()?.DestroyObstacle()); // If another barrel is in the explosion explode it
        base.DestroyObstacle();
    }
}
