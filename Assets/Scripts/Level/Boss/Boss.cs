using UnityEngine;

/// <summary>
/// The base <see langword="abstract"/> class that child <see cref="Boss"/> scripts will derive from.
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
        yield return new WaitForSeconds(2.5f);
        Destroy(gameObject);
    }
}
