using UnityEngine;

/// <summary>
/// Checks to see if player passes obstacles and increases score
/// </summary>
public class ScoreCollider : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag("Player")) return; // If doesn't collide with player then skip
        if (GetComponentInParent<ObstacleRow>() != null && !GetComponentInParent<ObstacleRow>().HasObstacles) return; // If has obstacle rows but no obstacles then skip
        LevelManager.instance.Score++; // If there are obstacles or it's a security door then increase score
    }
}
