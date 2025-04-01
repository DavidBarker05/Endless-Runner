using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI scoreCounter;
    [SerializeField]
    LevelManager levelManager;

    void Start() => StartGame();

    void Update()
    {
        scoreCounter.text = $"SCORE: {levelManager.Score}";
    }

    public void StartGame()
    {
        levelManager.ResetGame();
    }
}
