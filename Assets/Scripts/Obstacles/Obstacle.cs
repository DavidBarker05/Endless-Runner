using UnityEngine;

/// <summary>
/// Code that kills the player when they collide with an obstacle
/// </summary>
public class Obstacle : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) GameManager.instance.State = GameManager.GameState.Dead;
    }
}
