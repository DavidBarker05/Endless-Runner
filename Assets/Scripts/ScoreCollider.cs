using UnityEngine;

public class ScoreCollider : MonoBehaviour
{
    [SerializeField]
    LevelManager levelManager;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player")) levelManager.IncreaseScore(1);
    }
}
