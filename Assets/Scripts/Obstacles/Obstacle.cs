using UnityEngine;

/// <summary>
/// Code that kills the player when they collide with an obstacle
/// </summary>
public class Obstacle : MonoBehaviour
{
    public GameManager GameManager { get; set; }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) GameManager.State = GameManager.GameState.Dead;
    }
}
