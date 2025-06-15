using UnityEngine;

public class SlopeBoost : MonoBehaviour
{
    [SerializeField]
    float boostSpeed;

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        PlayerManager.Instance.IsForcedSlide = true;
        LevelManager.Instance.BonusSpeed = boostSpeed;
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) PlayerManager.Instance.IsForcedSlide = false;
    }
}
