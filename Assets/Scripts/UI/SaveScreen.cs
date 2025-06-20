using TMPro;
using UnityEngine;

public class SaveScreen : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI totalScoreValueLabel;

    void OnEnable()
    {
        if (GameManager.Instance != null) totalScoreValueLabel.text = $"{GameManager.Instance.TotalScore}";
    }
}
