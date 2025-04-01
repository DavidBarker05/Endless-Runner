using GameUtilities;
using UnityEngine;

public class GuardMaxRange : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet")) Destroy(UtilityMethods.GetParent(other.gameObject));
    }
}
