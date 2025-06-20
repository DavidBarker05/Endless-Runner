using GameEvents = GameUtilities.GameEvents;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


/// <summary>
/// Manages the game State and UI elements
/// </summary>
public class GameManager : MonoBehaviour
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
        /// <summary>
        /// 
        /// </summary>
        Paused,
    }

    /// <summary>
    /// The current State of the game
    /// </summary>
    public GameState State { get; set; }
    public int TotalScore { get; set; }

    Dictionary<GameEvents::EventType, List<GameEvents::IEventListener>> eventListeners = new Dictionary<GameEvents::EventType, List<GameEvents::IEventListener>>();

    void Awake()
    {
        if (Instance != null && Instance != this) Destroy(gameObject);
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            State = GameState.None;
        }
    }

    public void AddListener(GameEvents::EventType eventType, GameEvents::IEventListener eventListener)
    {
        if (eventListener == null) return;
        if (eventType == GameEvents::EventType.Empty) return;
        if (!eventListeners.ContainsKey(eventType)) eventListeners.Add(eventType, new List<GameEvents::IEventListener>());
        if (!eventListeners[eventType].Contains(eventListener)) eventListeners[eventType].Add(eventListener);
    }

    public void RemoveListener(GameEvents::EventType eventType, GameEvents::IEventListener eventListener)
    {
        if (eventListener == null) return;
        if (eventType == GameEvents::EventType.Empty) return;
        if (!eventListeners.ContainsKey(eventType)) return;
        eventListeners[eventType].Remove(eventListener);
    }

    public void InvokeEvent(GameEvents::EventType eventType, Component sender, object param = null)
    {
        if (!eventListeners.ContainsKey(eventType)) return;
        if (eventType == GameEvents::EventType.Empty) return;
        List<GameEvents::IEventListener> eventListenerList = new List<GameEvents::IEventListener>(eventListeners[eventType]);
        foreach (GameEvents::IEventListener listener in eventListenerList)
        {
            listener?.OnEvent(eventType, sender, param);
        }
    }

    public void ClearEvents() => eventListeners.Clear();
}
