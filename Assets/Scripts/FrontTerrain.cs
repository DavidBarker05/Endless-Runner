using UnityEngine;
using GameUtilities;

public class FrontTerrain : MonoBehaviour
{
    [SerializeField]
    LevelManager levelManager;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("BackTerrain"))
        {
            other.enabled = false;
            GetComponentInParent<SpawnableTerrain>().CanMove = true;
            transform.parent.position = other.transform.parent.position + UtilityMethods.ZVector((other.GetComponentInParent<SpawnableTerrain>().Size + GetComponentInParent<SpawnableTerrain>().Size) / 2f);
            if (levelManager.GenerateTerrainOnTrigger) levelManager.GenerateTerrain();
        }
    }
}
