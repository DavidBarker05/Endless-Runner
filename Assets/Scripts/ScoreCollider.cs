using UnityEngine;

public class ScoreCollider : MonoBehaviour
{
    [SerializeField]
    LevelManager levelManager;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && GetComponentInParent<ObstacleRow>().HasObstacles) levelManager.Score++;
    }
}
