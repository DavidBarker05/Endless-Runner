using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI scoreCounter;
    [SerializeField]
    LevelManager levelManager;

    void Start()
    {
    }

    void Update()
    {
        scoreCounter.text = $"SCORE: {levelManager.Score}";
    }
}
