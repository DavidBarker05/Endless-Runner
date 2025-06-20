using TMPro;
using UnityEngine;

public class DefeatScreen : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI currentScoreLabel;
    [SerializeField]
    TextMeshProUGUI bossesBeatenLabel;
    [SerializeField]
    TextMeshProUGUI totalScoreValueLabel;

    void OnEnable()
    {
        if (GameManager.Instance == null || LevelManager.Instance == null) return;
        if (GameManager.Instance.State != GameManager.GameState.Dead) return;
        currentScoreLabel.text = $"SCORE: {LevelManager.Instance.Score}";
        bossesBeatenLabel.text = $"BOSSES BEATEN: {LevelManager.Instance.BossesBeaten} (X50)";
        GameManager.Instance.TotalScore = LevelManager.Instance.Score + LevelManager.Instance.BossesBeaten * 50;
        totalScoreValueLabel.text = $"{GameManager.Instance.TotalScore}";
    }
}
