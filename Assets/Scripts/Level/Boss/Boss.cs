using UnityEngine;

/// <summary>
/// <para>
/// The base abstract class that child <see cref="Boss"/> scripts will derive from.
/// </para>
/// </summary>
public abstract class Boss : MonoBehaviour
{
    /// <summary>
    /// <para>
    /// Begin the coroutine to deactivate the boss.
    /// </para>
    /// </summary>
    public virtual void Disengage() => StartCoroutine(Deactivate());

    // Deactivate (destroy) the boss after a certain number of seconds
    System.Collections.IEnumerator Deactivate()
    {
        yield return new WaitForSeconds(2.5f);
        Destroy(gameObject);
    }
}
