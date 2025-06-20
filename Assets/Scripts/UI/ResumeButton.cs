using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent (typeof(Button))]
public class ResumeButton : MonoBehaviour
{
    void Awake()
    {
        GetComponent<Button>().onClick.AddListener(
            () => {
                if (GameManager.Instance != null) GameManager.Instance.State = GameManager.GameState.Alive;
                if (EventSystem.current != null) EventSystem.current.SetSelectedGameObject(null);
            }
        );
    }
}
