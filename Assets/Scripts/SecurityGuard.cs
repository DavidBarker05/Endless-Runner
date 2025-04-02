using UnityEngine;

/// <summary>
/// Code to make security guard shoot
/// </summary>
public class SecurityGuard : MonoBehaviour
{
    [SerializeField]
    float fireRate;
    [SerializeField]
    [Tooltip("The place to spawn bullets")]
    Transform gunBarrel;
    [SerializeField]
    [Tooltip("Bullet prefab")]
    GameObject bullet;

    float shootTime;

    /// <summary>
    /// Indicates if the security guard can shoot
    /// </summary>
    public bool ShootingEnabled { get; set; }

    void FixedUpdate()
    {
        if (!ShootingEnabled) return; // If can't shoot then skip
        shootTime += Time.fixedDeltaTime; // Increase time since last shot
        if (shootTime <= 1f / fireRate) return; // If time is before the shooting then skip
        var go = Instantiate(bullet, transform);
        go.transform.position = gunBarrel.position;
        go.transform.rotation = gunBarrel.rotation;
        shootTime = 0f; // Reset time
    }
}
