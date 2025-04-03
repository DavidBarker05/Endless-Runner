using UnityEngine;

/// <summary>
/// Checks to see if player passes obstacles and increases score
/// </summary>
public class ScoreCollider : MonoBehaviour
{
    public LevelManager LevelManager { get; set; }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && GetComponentInParent<ObstacleRow>().HasObstacles) LevelManager.Score++;
    }
}
