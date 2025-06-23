using UnityEngine;

public class SlopeBoost : MonoBehaviour
{
    [SerializeField]
    float boostFOV;
    [SerializeField]
    float boostHeight;

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        PlayerManager.Instance.IsForcedSlide = true;
        PlayerManager.Instance.MainCam.fieldOfView = boostFOV;
    }

    void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        PlayerManager.Instance.IsForcedSlide = false;
        PlayerManager.Instance.SpeedJumpHeight = boostHeight;
    }
}
