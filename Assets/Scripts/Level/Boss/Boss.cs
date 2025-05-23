using System.Collections;
using UnityEngine;

public abstract class Boss : MonoBehaviour
{
    public virtual void Disengage() => StartCoroutine(Deactivate());

    private IEnumerator Deactivate()
    {
        yield return new WaitForSeconds(2.5f);
        Destroy(gameObject);
    }
}
