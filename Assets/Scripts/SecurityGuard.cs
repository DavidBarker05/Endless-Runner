using UnityEngine;

public class SecurityGuard : MonoBehaviour
{
    [SerializeField]
    float fireRate;
    [SerializeField]
    Transform gunBarrel;
    [SerializeField]
    GameObject bullet;

    float shootTime;

    public bool ShootingEnabled { get; set; }

    void FixedUpdate()
    {
        if (!ShootingEnabled) return;
        shootTime += Time.fixedDeltaTime;
        if (shootTime <= 1f / fireRate) return;
        var go = Instantiate(bullet, transform);
        go.transform.position = gunBarrel.position;
        go.transform.rotation = gunBarrel.rotation;
        shootTime = 0f;
    }
}
