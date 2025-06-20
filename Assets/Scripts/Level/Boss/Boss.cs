using UnityEngine;

/// <summary>
/// The base abstract class that child <see cref="Boss"/> scripts will derive from.
/// </summary>
public abstract class Boss : MonoBehaviour
{
    /// <summary>
    /// Begin the coroutine to deactivate the boss.
    /// </summary>
    public virtual void Disengage() => StartCoroutine(Deactivate());

    // Deactivate (destroy) the boss after a certain number of seconds
    System.Collections.IEnumerator Deactivate()
    {
        float timer = 0f;
        while (timer < 2.5f)
        {
            while (GameManager.Instance.State == GameManager.GameState.Paused) yield return null;
            timer += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }
        Destroy(gameObject);
    }
}
