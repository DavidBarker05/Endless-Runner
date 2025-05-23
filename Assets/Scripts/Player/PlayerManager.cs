using GameUtilities;
using UnityEngine;

/// <summary>
/// Handles player input and movement
/// </summary>
[RequireComponent(typeof(CharacterController))]
public class PlayerManager : MonoBehaviour
{
    public enum AnimationState
    {
        Run,
        Fall,
        Crash,
        Shot,
        Exploded,
        Caught,
        Slide,
    }

    public static PlayerManager Instance { get; private set; }

    [SerializeField]
    ParticleSystem jumpParticles;
    [SerializeField]
    ParticleSystem bonusParticles;
    [SerializeField]
    GameObject shieldBubble;
    [Header("Lane Switching")]
    [SerializeField]
    [Tooltip("The three lanes the player can switch between")]
    Transform[] lanes = new Transform[3];
    [SerializeField]
    [Tooltip("The time in seconds that it takes for the player to switch from one lane to another")]
    [Min(0.001f)]
    float switchTime;
    [Header("Jumping")]
    [SerializeField]
    [Tooltip("The max height the player can jump")]
    [Min(0f)]
    float jumpHeight;
    [Header("Sliding")]
    [SerializeField]
    [Tooltip("The height of the player's hitbox while sliding")]
    [Min(0.001f)]
    float slideHeight;
    [SerializeField]
    [Tooltip("The time in seconds that the player can slide for (they will stop sliding after that many seconds)")]
    [Min(0.001f)]
    float maxSlideTime;
    [Header("Gravity")]
    [SerializeField]
    [Min(0.001f)]
    float gravity;
    [Header("Reset")]
    [SerializeField]
    [Tooltip("The time in seconds that the player must hold the reset button in order to reset")]
    [Min(0.001f)]
    float resetHoldTime;
    

    const float SNAP_DISTANCE = 0.5f; // The distance between the player and row needed to snap to the row
    const float SNAP_TO_GROUND_SPEED = -0.01f; // The speed the player moves into the ground when grounded so that character controller grounded State works

    CharacterController cc;
    int currentLane = 1;
    int targetLane = 1;
    int horizontalDirection;
    float vVel = SNAP_TO_GROUND_SPEED;
    float standHeight;
    float currentSlideTime;
    float currentResetHoldTime;
    bool pressingSlide;
    Animator animator;
    bool groundedLastFrame = true;

    /// <summary>
    /// Indicates if the player can slide
    /// </summary>
    public bool CanSlide => currentSlideTime <= maxSlideTime && GameManager.Instance.State == GameManager.GameState.Alive; // Needs to be alive and sliding for less time than max time
    /// <summary>
    /// Indicates if the player is currently sliding
    /// </summary>
    public bool IsSliding => CanSlide && pressingSlide;
    /// <summary>
    /// Indicates if the player can move
    /// </summary>
    public bool CanMove => !IsSliding && GameManager.Instance.State == GameManager.GameState.Alive;
    /// <summary>
    /// Indicates if the player can jump
    /// </summary>
    public bool CanJump => cc.isGrounded && !IsSliding && GameManager.Instance.State == GameManager.GameState.Alive;
    /// <summary>
    /// Extra jump height added to the base jump height, used for the jump boost powerup
    /// </summary>
    public float ExtraJumpHeight { get; set; }
    public AnimationState State { get; set; }
    public bool Caught { get; set; }
    public bool Invulnerable { get; set; }
    public ParticleSystem JumpParticles { get => jumpParticles; set => jumpParticles = value; }
    public ParticleSystem BonusParticles { get => bonusParticles; set => bonusParticles = value; }
    public GameObject ShieldBubble { get => shieldBubble; set => shieldBubble = value; }

    void Awake()
    {
        if (Instance != null && Instance != this) Destroy(this);
        else Instance = this;
        cc = GetComponent<CharacterController>();
        standHeight = cc.height;
        animator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        if ((Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D)) && CanMove)
        {
            currentLane = targetLane;
            if (Input.GetKeyDown(KeyCode.A)) horizontalDirection = -1;
            if (Input.GetKeyDown(KeyCode.D)) horizontalDirection = 1;
            targetLane += horizontalDirection;
            targetLane = Mathf.Clamp(targetLane, 0, 2); // Make sure target lane can't be out of bounds
        }
        if (Input.GetKeyUp(KeyCode.A) && horizontalDirection == -1 || Input.GetKeyUp(KeyCode.D) && horizontalDirection == 1) horizontalDirection = 0; // Stop player from being able to move again if they release the key related to the direction they're moving in
        if (Input.GetKey(KeyCode.Space) && CanJump) vVel = Mathf.Sqrt(2 * gravity * (jumpHeight + ExtraJumpHeight));
        pressingSlide = Input.GetKey(KeyCode.LeftControl);
        if (!pressingSlide) currentSlideTime = 0f; // Reset time player is sliding for
        if (Input.GetKey(KeyCode.R) && GameManager.Instance.State == GameManager.GameState.Alive) currentResetHoldTime += Time.deltaTime; // Make time increase while player holds r
        else currentResetHoldTime = 0f; // Reset timer when release r
        if (currentResetHoldTime >= resetHoldTime) LevelManager.Instance.ResetGame(); // Reset game once timer excedes time
        if (Input.GetKey(KeyCode.L)) Time.timeScale = 0f;
    }

    void FixedUpdate()
    {
        if (IsSliding)
        {
            currentSlideTime += Time.fixedDeltaTime; // If sliding increase slide time
            State = AnimationState.Slide;
        }
        cc.height = IsSliding ? slideHeight : standHeight; // Set appropriate height
        cc.center = UtilityMethods.YVector(cc.height / 2f); // Set appropriate center
        Vector3 laneDisplacement = UtilityMethods.XVector(lanes[targetLane].position - lanes[currentLane].position);
        Vector3 hVel = laneDisplacement / switchTime;
        Vector3 movement = (hVel + UtilityMethods.YVector(vVel)) * Time.fixedDeltaTime;
        cc.Move(movement);
        if (cc.isGrounded)
        {
            vVel = SNAP_TO_GROUND_SPEED;
            groundedLastFrame = true;
            if (GameManager.Instance.State != GameManager.GameState.Dead && !IsSliding) State = AnimationState.Run;
            if (Caught) State = AnimationState.Caught;
        }
        else
        {
            vVel -= gravity * Time.fixedDeltaTime;
            if (groundedLastFrame) groundedLastFrame = false;
            else if (GameManager.Instance.State != GameManager.GameState.Dead) State = AnimationState.Fall;
        }
        float targetDistance = UtilityMethods.XDistance(transform.position, lanes[targetLane].position);
        if (targetDistance > SNAP_DISTANCE) return; // If distance is greater than snap distance then don't snap
        transform.position = UtilityMethods.YZVector(transform.position) + UtilityMethods.XVector(lanes[targetLane].position);
        currentLane = targetLane;
        targetLane += horizontalDirection;
        targetLane = Mathf.Clamp(targetLane, 0, 2);
        animator.SetInteger("AnimationState", (int)State);
    }

    /// <summary>
    /// Resets the player
    /// </summary>
    public void ResetPlayer()
    {
        currentLane = 1;
        targetLane = 1;
        horizontalDirection = 0;
        vVel = SNAP_TO_GROUND_SPEED;
        currentSlideTime = 0f;
        currentResetHoldTime = 0f;
        pressingSlide = false;
        ExtraJumpHeight = 0f;
        cc.height = standHeight;
        cc.center = UtilityMethods.YVector(cc.height / 2f);
        cc.enabled = false;
        transform.position = lanes[currentLane].position;
        cc.enabled = true;
        cc.Move(UtilityMethods.YVector(vVel)); // Move into ground so that is grounded can start working
        groundedLastFrame = true;
        State = AnimationState.Run;
        Caught = false;
        Invulnerable = false;
    }
}
