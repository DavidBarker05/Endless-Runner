using UnityEngine;

/// <summary>
/// Code that kills the player when they collide with an obstacle
/// </summary>
public class Obstacle : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        GameManager.Instance.State = GameManager.GameState.Dead;
        PlayerManager.Instance.State = gameObject.tag switch
        {
            "ExplosiveBarrel" => PlayerManager.AnimationState.Exploded,
            "LevelOneBoss" => PlayerManager.AnimationState.Shot,
            _ => PlayerManager.AnimationState.Crash,
        };
    }
}
