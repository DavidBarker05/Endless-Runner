using UnityEngine;

/// <summary>
/// Attatched to the front of a terrain so that it can make the terrain behind it move forward
/// </summary>
public class FrontTerrain : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("BackTerrain")) return; // Only do the following code if it collides with a back terrain
        other.enabled = false;
        GetComponentInParent<SpawnableTerrain>().CanMove = true;
        transform.parent.position = other.transform.parent.position + GameUtilities.UtilityMethods.UtilMethods.ZVector((other.GetComponentInParent<SpawnableTerrain>().Size + GetComponentInParent<SpawnableTerrain>().Size) / 2f); // Position the other terrain to the correct place
        if (LevelManager.Instance.GenerateTerrainOnTrigger) LevelManager.Instance.GenerateTerrain();
    }
}
