using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class SaveScoreButton : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI savedName;

    void Awake()
    {
        GetComponent<Button>().onClick.AddListener(
            async () => {
                if (GameManager.Instance != null && DatabaseManager.Instance != null && savedName.text != "NO NAME")
                {
                    await DatabaseManager.Instance.SaveScore(savedName.text, GameManager.Instance.TotalScore);
                    await Task.Delay(1000);
                    GameUIManager.Instance.CurrentScreen = GameUIManager.Instance.Screens[1];
                }
            }
        );
    }
}
