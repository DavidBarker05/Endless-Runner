using TMPro;
using UnityEngine;

/// <summary>
/// Manages the game state and UI elements
/// </summary>
public class GameManager : MonoBehaviour
{
    /// <summary>
    /// Different states the game can be in
    /// </summary>
    public enum GameState
    {
        /// <summary>
        /// Empty state
        /// </summary>
        None,
        /// <summary>
        /// Player is currently alive
        /// </summary>
        Alive,
        /// <summary>
        /// Player is currently dead
        /// </summary>
        Dead
    }

    [Header("Player")]
    [SerializeField]
    PlayerManager playerManager;
    [Header("UI")]
    [SerializeField]
    [Tooltip("Text used to display the score while the player is alive")]
    TextMeshProUGUI scoreCounter;

    /// <summary>
    /// The current state of the game
    /// </summary>
    public GameState State { get; set; }
    /// <summary>
    /// The level manager of the game
    /// </summary>
    public LevelManager LevelManager { get; private set; }
    /// <summary>
    /// The player manager of the game
    /// </summary>
    public PlayerManager PlayerManager => playerManager;
    /// <summary>
    /// The pickup manager of the game
    /// </summary>
    public PickupManager PickupManager { get; private set; }


    void Awake()
    {
        LevelManager = GetComponent<LevelManager>();
        PickupManager = GetComponent<PickupManager>();
    }

    void Start() => StartGame();

    void Update()
    {
        if (State == GameState.Alive) // UI while the player is alive
        {
            scoreCounter.enabled = true;
            scoreCounter.text = $"SCORE: {LevelManager.Score}";
        }
    }

    /// <summary>
    /// Starts the game
    /// </summary>
    public void StartGame() => LevelManager.ResetGame();
}
