using UnityEngine;

public class AlwaysDestructible : Destructible
{
    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player") && !other.CompareTag("LevelOneBoss")) return;
        DestroyObstacle();
    }
}
