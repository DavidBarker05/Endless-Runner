using UnityEngine;

/// <summary>
/// Code that kills the player when they collide with an obstacle
/// </summary>
public class Obstacle : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player") || PlayerManager.Instance.Invulnerable) return;
        GameManager.Instance.State = GameManager.GameState.Dead;
        if (gameObject.CompareTag("ExplosiveBarrel")) PlayerManager.Instance.State = PlayerManager.AnimationState.Exploded;
        else if (gameObject.CompareTag("LevelOneBoss")) PlayerManager.Instance.Caught = true;
        else PlayerManager.Instance.State = PlayerManager.AnimationState.Crash;
    }
}
