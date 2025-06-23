using UnityEngine;

public class AfterGapSlow : MonoBehaviour
{
    [SerializeField]
    float normalFOV;

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        PlayerManager.Instance.InvulnerableHelp = false;
        PlayerManager.Instance.MainCam.fieldOfView = normalFOV;
        PlayerManager.Instance.SpeedJumpHeight = 0f;
    }
}
