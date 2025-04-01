using UnityEngine;

public class ScoreCollider : MonoBehaviour
{
    [SerializeField]
    LevelManager levelManager;

    ObstacleRow obstacleRow;

    void Start()
    {
        obstacleRow = GetComponentInParent<ObstacleRow>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && obstacleRow.ObstacleCount > 0) levelManager.IncreaseScore(1);
    }
}
