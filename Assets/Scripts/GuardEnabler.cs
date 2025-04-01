using UnityEngine;

public class GuardEnabler : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("SecurityGuard")) other.gameObject.GetComponent<SecurityGuard>().ShootingEnabled = true;
    }
}
