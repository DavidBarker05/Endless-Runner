using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public enum GameState
    {
        None,
        Alive,
        Dead
    }

    [SerializeField]
    TextMeshProUGUI scoreCounter;
    [SerializeField]
    LevelManager levelManager;

    public GameState State { get; set; }

    void Start() => StartGame();

    void Update()
    {
        if (State == GameState.Alive)
        {
            scoreCounter.text = $"SCORE: {levelManager.Score}";
        }
    }

    public void StartGame()
    {
        levelManager.ResetGame();
    }
}
