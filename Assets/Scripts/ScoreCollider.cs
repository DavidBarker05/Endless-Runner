using UnityEngine;

public class ScoreCollider : MonoBehaviour
{
    [SerializeField]
    LevelManager levelManager;

    ObstacleRow obstacleRow;

    void Awake() => obstacleRow = GetComponentInParent<ObstacleRow>();

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && obstacleRow.ObstacleCount > 0) levelManager.IncreaseScore(1);
    }
}
