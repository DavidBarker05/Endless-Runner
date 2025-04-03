using UnityEngine;

/// <summary>
/// Code that kills the player when they collide with an obstacle
/// </summary>
public class Obstacle : MonoBehaviour
{
    [SerializeField]
    GameManager gameManager;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) gameManager.State = GameManager.GameState.Dead;
    }
}
