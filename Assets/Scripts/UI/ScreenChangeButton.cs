using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ScreenChangeButton : MonoBehaviour
{
    [SerializeField]
    [Min(0)]
    int screenNumber;

    void Start()
    {
        if (GameUIManager.Instance.Screens.Length == 0) return;
        screenNumber = Mathf.Min(screenNumber, GameUIManager.Instance.Screens.Length - 1);
        GetComponent<Button>().onClick.AddListener(() => GameUIManager.Instance.CurrentScreen = GameUIManager.Instance.Screens[screenNumber]);
    }
}
