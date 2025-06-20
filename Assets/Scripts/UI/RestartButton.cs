using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RestartButton : MonoBehaviour
{
    void Awake()
    {
        GetComponent<Button>().onClick.AddListener(
            () => {
                if (LevelManager.Instance != null) LevelManager.Instance.ResetGame();
                if (EventSystem.current != null) EventSystem.current.SetSelectedGameObject(null);
            }
        );
    }
}
