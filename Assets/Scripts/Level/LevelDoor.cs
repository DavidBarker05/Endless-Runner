using UnityEngine;

public class LevelDoor : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) LevelManager.Instance.LevelUp();
    }
}
