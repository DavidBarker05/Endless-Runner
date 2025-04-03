using GameUtilities;
using UnityEngine;

/// <summary>
/// Attatched to a trigger so that when any security guard passes through it they can start shooting
/// </summary>
public class GuardEnabler : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("SecurityGuard")) other.GetComponentInParent<SecurityGuard>().ShootingEnabled = true; // Allow the guard that passed through to shoot
    }
}
