using System.Collections.Generic;
using UnityEngine;

public class GameUIManager : MonoBehaviour
{
    public static GameUIManager Instance { get; private set; }

    [SerializeField]
    List<GameScreen> screens = new List<GameScreen>();

    public GameScreen[] Screens { get; private set; }

    GameScreen _screen;
    public GameScreen CurrentScreen
    {
        get => _screen;
        set
        {
            if (_screen != value)
            {
                _screen.panel.SetActive(false);
                _screen = value;
                _screen.panel.SetActive(true);
            }
        }
    }

    void Awake()
    {
        if (Instance != null && Instance != this) Destroy(gameObject);
        else Instance = this;
        Screens = screens.ToArray();
        CurrentScreen = Screens[0];
    }
}
