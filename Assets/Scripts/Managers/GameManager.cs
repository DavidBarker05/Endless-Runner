using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Manages the game state and UI elements
/// </summary>
public class GameManager : MonoBehaviour
{
    public static GameManager instance;

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
    /// The current state of the game
    /// </summary>
    public GameState State { get; set; }

    void Awake()
    {
        if (instance != null && instance != this) Destroy(this);
        else instance = this;
    }

    void Start() => StartGame();

    void Update()
    {
        if (State == GameState.Alive) // UI while the player is alive
        {
            scoreCounter.enabled = true;
            restartText.enabled = true;
            deathScore.enabled = false;
            restartButton.gameObject.SetActive(false);
            scoreCounter.text = $"SCORE: {LevelManager.instance.Score}";
        }
        if (State == GameState.Dead) // UI while the player is dead
        {
            scoreCounter.enabled = false;
            restartText.enabled = false;
            deathScore.enabled = true;
            restartButton.gameObject.SetActive(true);
            deathScore.text = $"FINAL SCORE: {LevelManager.instance.Score}";
        }
    }

    /// <summary>
    /// Starts the game
    /// </summary>
    public void StartGame() => LevelManager.instance.ResetGame();
}
