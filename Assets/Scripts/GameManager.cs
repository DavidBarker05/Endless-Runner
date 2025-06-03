using GameEvents = GameUtilities.GameEvents;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Manages the game State and UI elements
/// </summary>
public class GameManager : MonoBehaviour, GameEvents::IEventListener
{
    public static GameManager Instance { get; private set; }

    /// <summary>
    /// Different states the game can be in
    /// </summary>
    public enum GameState
    {
        /// <summary>
        /// Empty State
        /// </summary>
        None,
        /// <summary>
        /// Player is currently alive
        /// </summary>
        Alive,
        /// <summary>
        /// Player is currently dead
        /// </summary>
        Dead,
    }

    [Header("UI")]
    [SerializeField]
    [Tooltip("Text used to display the score while the player is alive")]
    TextMeshProUGUI scoreCounter;
    [SerializeField]
    [Tooltip("Text used to explain how to restart")]
    TextMeshProUGUI restartText;
    [SerializeField]
    [Tooltip("Text used to display the score when the player is dead")]
    TextMeshProUGUI deathScore;
    [SerializeField]
    [Tooltip("Button used to restart the game when the player is dead")]
    Button restartButton;

    /// <summary>
    /// The current State of the game
    /// </summary>
    public GameState State { get; set; }

    Dictionary<GameEvents::EventType, List<GameEvents::IEventListener>> eventListeners = new Dictionary<GameEvents::EventType, List<GameEvents::IEventListener>>();

    void Awake()
    {
        if (Instance != null && Instance != this) Destroy(gameObject);
        else Instance = this;
    }

    void Start()
    {
        AddListener(GameEvents::EventType.ObstaclePassed, this);
        AddListener(GameEvents::EventType.BonusPickupEffect, this);
        StartGame();
    }

    void Update()
    {
        switch (State)
        {
            case GameState.Alive: // UI while the player is alive
                scoreCounter.enabled = true;
                restartText.enabled = true;
                deathScore.enabled = false;
                restartButton.gameObject.SetActive(false);
                break;
            case GameState.Dead: // UI while the player is dead
                scoreCounter.enabled = false;
                restartText.enabled = false;
                deathScore.enabled = true;
                restartButton.gameObject.SetActive(true);
                break;
        }
    }

    void OnDestroy()
    {
        RemoveListener(GameEvents::EventType.ObstaclePassed, this);
        RemoveListener(GameEvents::EventType.BonusPickupEffect, this);
    }

    /// <summary>
    /// Starts the game
    /// </summary>
    public void StartGame() => LevelManager.Instance.ResetGame();

    public void AddListener(GameEvents::EventType eventType, GameEvents::IEventListener eventListener)
    {
        if (eventListener == null) return;
        if (!eventListeners.ContainsKey(eventType)) eventListeners.Add(eventType, new List<GameEvents::IEventListener>());
        if (!eventListeners[eventType].Contains(eventListener)) eventListeners[eventType].Add(eventListener);
    }

    public void RemoveListener(GameEvents::EventType eventType, GameEvents::IEventListener eventListener)
    {
        if (eventListener == null) return;
        if (!eventListeners.ContainsKey(eventType)) return;
        eventListeners[eventType].Remove(eventListener);
    }

    public void InvokeEvent(GameEvents::EventType eventType, Component sender, object param = null)
    {
        if (!eventListeners.ContainsKey(eventType)) return;
        List<GameEvents::IEventListener> eventListenerList = new List<GameEvents::IEventListener>(eventListeners[eventType]);
        System.Array.ForEach<GameEvents::IEventListener>(eventListenerList.ToArray(), l => l?.OnEvent(eventType, sender, param));
    }

    public void ClearEvents() => eventListeners.Clear();

    public void OnEvent(GameEvents::EventType eventType, Component sender, object param = null)
    {
        if (eventType == GameEvents::EventType.ObstaclePassed)
        {
            scoreCounter.text = $"SCORE: {param}";
            deathScore.text = $"FINAL SCORE: {param}";
        }
        else if (eventType == GameEvents.EventType.BonusPickupEffect)
        {
            scoreCounter.text = $"SCORE: {param}";
            deathScore.text = $"FINAL SCORE: {param}";
        }
    }
}
