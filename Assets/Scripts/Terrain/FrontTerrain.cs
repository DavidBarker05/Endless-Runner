using GameUtilities;
using UnityEngine;

/// <summary>
/// Attatched to the front of a terrain so that it can make the terrain behind it move forward
/// </summary>
public class FrontTerrain : MonoBehaviour
{
    public LevelManager LevelManager { get; set; }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("BackTerrain")) return; // Only do the following code if it collides with a back terrain
        other.enabled = false;
        GetComponentInParent<SpawnableTerrain>().CanMove = true;
        transform.parent.position = other.transform.parent.position + UtilityMethods.ZVector((other.GetComponentInParent<SpawnableTerrain>().Size + GetComponentInParent<SpawnableTerrain>().Size) / 2f); // Position the other terrain to the correct place
        if (LevelManager.GenerateTerrainOnTrigger) LevelManager.GenerateTerrain();
    }
}
