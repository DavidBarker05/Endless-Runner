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

    /// <summary>
    /// Max range of the guard
    /// </summary>
    public GameObject MaxRange => maxRange;

    public void EnableShooting() => StartCoroutine(Shoot());

    System.Collections.IEnumerator Shoot()
    {
        while (true)
        {
            float timer = 0f;
            while (timer < 1f / fireRate)
            {
                // while (paused) yield return null;
                timer += Time.fixedDeltaTime;
                yield return new WaitForFixedUpdate();
            }
            if (GameManager.Instance.State == GameManager.GameState.Alive)
            {
                var _bullet = Instantiate(bullet, gunBarrel);
                _bullet.transform.position = gunBarrel.position;
                _bullet.transform.rotation = gunBarrel.rotation;
            }
        }
    }
}
