using UnityEngine;

public abstract class Boss : MonoBehaviour
{
    public virtual void Disengage() => StartCoroutine(Deactivate());

    System.Collections.IEnumerator Deactivate()
    {
        yield return new WaitForSeconds(2.5f);
        Destroy(gameObject);
    }
}
