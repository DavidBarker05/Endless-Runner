using UnityEngine;

/// <summary>
/// Attatched to the front of a terrain so that it can make the terrain behind it move forward
/// </summary>
public class FrontTerrain : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("BackTerrain")) return; // Only do the following code if it collides with a back terrain
        if (GetComponentInParent<SpawnableTerrain>().IsLocked) return;
        if (!LevelManager.Instance.IsCorrectTrigger(other.GetComponentInParent<SpawnableTerrain>(), GetComponentInParent<SpawnableTerrain>())) return;
        other.enabled = false;
        GetComponentInParent<SpawnableTerrain>().CanMove = true;
        transform.parent.position = new Vector3(transform.parent.position.x, transform.parent.position.y, other.transform.parent.position.z + (other.GetComponentInParent<SpawnableTerrain>().Size + GetComponentInParent<SpawnableTerrain>().Size) / 2f);
        if (LevelManager.Instance.GenerateTerrainOnTrigger) LevelManager.Instance.GenerateTerrain();
    }
}
