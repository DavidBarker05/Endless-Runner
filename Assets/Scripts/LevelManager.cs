using GameUtilities;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField]
    PlayerManager playerManager;

    [SerializeField]
    List<GameObject> visibleTerrain = new List<GameObject>();

    void Start()
    {
    }

    void Update()
    {
        transform.position = UtilityMethods.HorizontalVector(transform.position) + UtilityMethods.YVector(playerManager.transform.position);
    }

    public void ResetGame()
    {
        playerManager.ResetPlayer();
    }
}
