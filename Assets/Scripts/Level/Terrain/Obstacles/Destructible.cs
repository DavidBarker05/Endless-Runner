using GameUtilities.UtilityMethods;
using System.Collections;
using UnityEngine;

public class Destructible : MonoBehaviour
{
    [SerializeField]
    GameObject objectMesh;

    ParticleSystem destructionParticles;

    void Awake() => destructionParticles = UtilMethods.Parent(gameObject).GetComponentInChildren<ParticleSystem>();

    [System.Obsolete] // Because particle.playbackSpeed is deprecated
    void FixedUpdate()
    {
        if (GameManager.Instance.State == GameManager.GameState.Paused && destructionParticles.playbackSpeed != 0f)
        {
            if (destructionParticles.playbackSpeed != 0f) destructionParticles.playbackSpeed = 0f;
        }
        else
        {
            if (destructionParticles.playbackSpeed == 0f) destructionParticles.playbackSpeed = 1f;
        }
    }

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
        StartCoroutine(DestroyObject());
    }

    IEnumerator DestroyObject()
    {
        float timer = 0f;
        while (timer < destructionParticles.main.duration)
        {
            while (GameManager.Instance.State == GameManager.GameState.Paused) yield return null;
            timer += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }
    }
}
