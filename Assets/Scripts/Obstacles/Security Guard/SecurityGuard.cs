using UnityEngine;

/// <summary>
/// Code to make security guard shoot
/// </summary>
public class SecurityGuard : MonoBehaviour
{
    [SerializeField]
    [Tooltip("How many times the guard shoots per second")]
    float fireRate;
    [SerializeField]
    [Tooltip("The place to spawn bullets")]
    Transform gunBarrel;
    [SerializeField]
    [Tooltip("Bullet prefab")]
    GameObject bullet;
    [Tooltip("Max range of the guard")]
    [SerializeField]
    GameObject maxRange;

    float shootTime;

    /// <summary>
    /// Indicates if the security guard can shoot
    /// </summary>
    public bool ShootingEnabled { get; set; }

    /// <summary>
    /// Max range of the guard
    /// </summary>
    public GameObject MaxRange => maxRange;

    void FixedUpdate()
    {
        if (!ShootingEnabled) return; // If can't shoot then skip
        shootTime += Time.fixedDeltaTime; // Increase time since last shot
        if (shootTime <= 1f / fireRate) return; // If time is before the shooting then skip
        var _bullet = Instantiate(bullet, gunBarrel);
        _bullet.transform.position = gunBarrel.position;
        _bullet.transform.rotation = gunBarrel.rotation;
        shootTime = 0f; // Reset time
    }
}
