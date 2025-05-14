using System.Collections;
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

    IEnumerator Shoot()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f / fireRate);
            if (GameManager.instance.State == GameManager.GameState.Alive)
            {
                var _bullet = Instantiate(bullet, gunBarrel);
                _bullet.transform.position = gunBarrel.position;
                _bullet.transform.rotation = gunBarrel.rotation;
            }
        }
    }
}
