using UnityEngine;

public class AfterGapSlow : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) LevelManager.Instance.BonusSpeed = 0f;
    }
}
