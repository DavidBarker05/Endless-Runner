using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class SceneChangeButton : MonoBehaviour
{
    [SerializeField]
    [Min(0)]
    int sceneNumber;

    void Awake() => GetComponent<Button>().onClick.AddListener(async () => await SceneManager.LoadSceneAsync(sceneNumber));
}
