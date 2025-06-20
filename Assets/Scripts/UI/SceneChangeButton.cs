using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class SceneChangeButton : MonoBehaviour
{
    [SerializeField]
    [Min(0)]
    int sceneNumber;

    void Awake()
    {
        GetComponent<Button>().onClick.AddListener(
            async () => {
                if (GameManager.Instance != null) GameManager.Instance.State = GameManager.GameState.None;
                await SceneManager.LoadSceneAsync(sceneNumber);
            }
        );
    }
}
