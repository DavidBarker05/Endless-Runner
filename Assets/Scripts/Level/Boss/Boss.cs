using UnityEngine;

/// <summary>
/// The base abstract class that the first and second boss scripts derive from
/// </summary>
public abstract class Boss : MonoBehaviour
{
    /// <summary>
    /// Begin the coroutine to deactivate the boss
    /// </summary>
    public virtual void Disengage() => StartCoroutine(Deactivate());

    // Deactivate (destroy) the boss after a certain number of seconds
    System.Collections.IEnumerator Deactivate()
    {
        yield return new WaitForSeconds(2.5f);
        Destroy(gameObject);
    }
}
