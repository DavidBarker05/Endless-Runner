using GameUtilities;
using UnityEngine;

public class Destructible : MonoBehaviour
{
    [SerializeField]
    GameObject objectMesh;

    ParticleSystem destructionParticles;

    void Awake() => destructionParticles = UtilityMethods.Parent(gameObject).GetComponentInChildren<ParticleSystem>();

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player") && !other.CompareTag("LevelOneBoss")) return;
        if (PlayerManager.Instance.Invulnerable || other.CompareTag("LevelOneBoss")) DestroyObstacle();
    }

    public virtual void DestroyObstacle()
    {
        objectMesh.SetActive(false);
        gameObject.SetActive(false);
        destructionParticles.Play();
        Destroy(UtilityMethods.Parent(gameObject), destructionParticles.main.duration);
    }
}
