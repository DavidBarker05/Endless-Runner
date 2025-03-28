using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField]
    PlayerManager playerManager;

    void Start()
    {
    }

    void Update()
    {
    }

    public void ResetGame()
    {
        playerManager.ResetPlayer();
    }
}
