using GameUtilities.UtilityMethods;
using GameEvents = GameUtilities.GameEvents;
using UnityEngine;


/// <summary>
/// Handles player input and movement
/// </summary>
[RequireComponent(typeof(CharacterController))]
public class PlayerManager : MonoBehaviour, GameEvents::IEventListener
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
        WallrunRight,
        WallrunLeft,
    }

    public static PlayerManager Instance { get; private set; }

    [SerializeField]
    AudioClip running;
    [SerializeField]
    LayerMask groundLayer;
    [SerializeField]
    [Min(0f)]
    float maxGroundCheckDistance;
    [SerializeField]
    GameObject playerMesh;
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
    

    const float SNAP_DISTANCE = 0.125f; // The distance between the player and row needed to snap to the row
    const float SNAP_TO_GROUND_SPEED = -0.01f; // The speed the player moves into the ground when grounded so that character controller grounded State works
    const float TERRAIN_RAY_THRESHOLD = -0.001f;

    CharacterController cc;
    int currentLane = 1;
    int targetLane = 1;
    int horizontalDirection;
    float vVel = SNAP_TO_GROUND_SPEED;
    float standHeight;
    float currentSlideTime;
    float currentResetHoldTime;
    float extraJumpHeight;
    bool pressingSlide;
    Animator animator;
    bool wasGroundedLastFrame = true;
    bool isGrounded = true;

    /// <summary>
    /// 
    /// </summary>
    public bool IsForcedSlide { get; set; }
    /// <summary>
    /// Indicates if the player can slide
    /// </summary>
    public bool CanSlide => currentSlideTime <= maxSlideTime && GameManager.Instance.State == GameManager.GameState.Alive; // Needs to be alive and sliding for less time than max time
    /// <summary>
    /// Indicates if the player is currently sliding
    /// </summary>
    public bool IsSliding => (CanSlide && pressingSlide || IsForcedSlide) && isGrounded;
    /// <summary>
    /// Indicates if the player can move
    /// </summary>
    public bool CanMove => !IsSliding && GameManager.Instance.State == GameManager.GameState.Alive;
    /// <summary>
    /// Indicates if the player can jump
    /// </summary>
    public bool CanJump => isGrounded && !IsSliding && GameManager.Instance.State == GameManager.GameState.Alive;
    public AnimationState State { get; set; }
    public bool Caught { get; set; }
    public bool Invulnerable { get; private set; }

    void Awake()
    {
        if (Instance != null && Instance != this) Destroy(gameObject);
        else Instance = this;
        cc = GetComponent<CharacterController>();
        standHeight = cc.height;
        animator = GetComponentInChildren<Animator>();
    }

    void Start()
    {
        GameManager.Instance.AddListener(GameEvents::EventType.JumpBoostPickupEffect, this);
        GameManager.Instance.AddListener(GameEvents::EventType.BonusPickupEffect, this);
        GameManager.Instance.AddListener(GameEvents::EventType.InvulnerabilityPickupEffect, this);
    }

    void Update()
    {
        if (GameManager.Instance.State == GameManager.GameState.Dead) return;
        if (Input.GetKey(KeyCode.R) && GameManager.Instance.State == GameManager.GameState.Alive) currentResetHoldTime += Time.deltaTime; // Make time increase while player holds r
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameManager.Instance.State == GameManager.GameState.Alive)
            {
                GameManager.Instance.State = GameManager.GameState.Paused;
                GameUIManager.Instance.CurrentScreen = GameUIManager.Instance.Screens[1];
            }
            else if (GameManager.Instance.State == GameManager.GameState.Paused)
            {
                GameManager.Instance.State = GameManager.GameState.Alive;
                GameUIManager.Instance.CurrentScreen = GameUIManager.Instance.Screens[0];
            }
        }
        else currentResetHoldTime = 0f; // Reset timer when release r
        if (currentResetHoldTime >= resetHoldTime) LevelManager.Instance.ResetGame(); // Reset game once timer excedes time
        if (State == AnimationState.WallrunRight || State == AnimationState.WallrunLeft)
        {
            pressingSlide = false; // Reset slide when wallrunning
            currentSlideTime = 0f; // Reset slide when wallrunning
            return; // Stop player from inputting anything while wallrunning
        }
        if ((Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D)) && CanMove)
        {
            currentLane = targetLane;
            if (Input.GetKeyDown(KeyCode.A)) horizontalDirection = -1;
            if (Input.GetKeyDown(KeyCode.D)) horizontalDirection = 1;
            targetLane += horizontalDirection;
            targetLane = Mathf.Clamp(targetLane, 0, 2); // Make sure target lane can't be out of bounds
        }
        if (Input.GetKeyUp(KeyCode.A) && horizontalDirection == -1 || Input.GetKeyUp(KeyCode.D) && horizontalDirection == 1) horizontalDirection = 0; // Stop player from being able to move again if they release the key related to the direction they're moving in
        if (Input.GetKey(KeyCode.Space) && CanJump) vVel = Mathf.Sqrt(2 * gravity * (jumpHeight + extraJumpHeight));
        pressingSlide = Input.GetKey(KeyCode.LeftControl);
        if (!pressingSlide) currentSlideTime = 0f; // Reset time player is sliding for
    }

    [System.Obsolete] // Because particle.playbackSpeed is deprecated
    void FixedUpdate()
    {
        if (GameManager.Instance.State == GameManager.GameState.Paused)
        {
            if (animator.speed != 0f) animator.speed = 0f;
            if (jumpParticles.playbackSpeed != 0f) jumpParticles.playbackSpeed = 0f;
            if (bonusParticles.playbackSpeed != 0f) bonusParticles.playbackSpeed = 0f;
            return;
        }
        else
        {
            if (animator.speed == 0f) animator.speed = 1f;
            if (jumpParticles.playbackSpeed == 0f) jumpParticles.playbackSpeed = 1f;
            if (bonusParticles.playbackSpeed == 0f) bonusParticles.playbackSpeed = 1f;
        }
        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit rayHit, maxGroundCheckDistance, groundLayer)) // Make sure player is directly above ground not a gap
        {
            if (rayHit.point.y < TERRAIN_RAY_THRESHOLD) // Check that the player's feet would be below 0 but also below floating-pont threshold set
            {
                if (Physics.SphereCast(transform.position, cc.radius, Vector3.down, out RaycastHit sphereHit, maxGroundCheckDistance, groundLayer)) // Check where the player would be touching the ground
                {
                    float centreOffset = Vector3.Dot(sphereHit.normal.normalized, Vector3.down) * cc.radius; // Figure out how far below the contact point is compared to the centre of the sphere
                    float centreY = sphereHit.point.y - centreOffset; // Calculate the y-position of the centre
                    float bottomY = centreY - cc.radius; // Figure out where the bottom would for the sphere
                    float yShift = -bottomY; // The amount to shift the level up by so the player's theoretical bottom point remains at [0, 0, 0]
                    LevelManager.Instance.OffsetTerrainAndSpawnY(yShift); // Offset the level
                    transform.position += UtilMethods.YVector(yShift); // Offset the player too
                }
            }
        }
        if (State == AnimationState.WallrunRight || State == AnimationState.WallrunLeft)
        {
            playerMesh.transform.rotation = Quaternion.Euler(0f, 0f, State == AnimationState.WallrunRight ? 25f : -25f);
            vVel = 0f;
            animator.SetInteger("AnimationState", (int)State); // Set animation state
            return; // Stop CharacterController from doing any movement while wallrunning
        }
        playerMesh.transform.rotation = Quaternion.Euler(0f, 0f, 0f); // Make sure player mesh goes back to normal rotation
        if (IsSliding)
        {
            currentSlideTime += Time.fixedDeltaTime; // If sliding increase slide time
            State = AnimationState.Slide;
        }
        cc.height = IsSliding ? slideHeight : standHeight; // Set appropriate height
        cc.center = UtilMethods.YVector(cc.height / 2f); // Set appropriate center
        Vector3 laneDisplacement = new Vector3(lanes[targetLane].position.x - lanes[currentLane].position.x, 0f);
        Vector3 hVel = laneDisplacement / switchTime;
        Vector3 movement = (hVel + UtilMethods.YVector(vVel)) * Time.fixedDeltaTime;
        if (GameManager.Instance.State == GameManager.GameState.Alive) cc.Move(movement);
        isGrounded = cc.isGrounded || wasGroundedLastFrame;
        if (cc.isGrounded)
        {
            vVel = SNAP_TO_GROUND_SPEED;
            wasGroundedLastFrame = true;
            if (GameManager.Instance.State == GameManager.GameState.Alive && !IsSliding) State = AnimationState.Run;
            if (Caught)
            {
                State = AnimationState.Caught;
                GameManager.Instance.State = GameManager.GameState.Dead;
            }
        }
        else
        {
            vVel -= gravity * Time.fixedDeltaTime;
            if (wasGroundedLastFrame) wasGroundedLastFrame = false;
            else if (GameManager.Instance.State == GameManager.GameState.Alive) State = AnimationState.Fall;
        }
        animator.SetInteger("AnimationState", (int)State);
        if (Mathf.Abs(transform.position.x - lanes[targetLane].position.x) > SNAP_DISTANCE) return; // If distance is greater than snap distance then don't snap
        transform.position = new Vector3(lanes[targetLane].position.x, transform.position.y, transform.position.z);
        currentLane = targetLane;
        targetLane += horizontalDirection;
        targetLane = Mathf.Clamp(targetLane, 0, 2);
    }

    void OnDestroy()
    {
        GameManager.Instance.RemoveListener(GameEvents::EventType.JumpBoostPickupEffect, this);
        GameManager.Instance.RemoveListener(GameEvents::EventType.BonusPickupEffect, this);
        GameManager.Instance.RemoveListener(GameEvents::EventType.InvulnerabilityPickupEffect, this);
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
        extraJumpHeight = 0f;
        cc.height = standHeight;
        cc.center = UtilMethods.YVector(cc.height / 2f);
        cc.enabled = false;
        transform.position = lanes[currentLane].position;
        cc.enabled = true;
        cc.Move(UtilMethods.YVector(vVel)); // Move into ground so that is grounded can start working
        wasGroundedLastFrame = true;
        State = AnimationState.Run;
        Caught = false;
        Invulnerable = false;
    }

    [System.Obsolete] // Because particle.startColor is deprecated, but particle.main.startColor doesn't let you change colours
    public void OnEvent(GameEvents::EventType eventType, Component sender, object param = null)
    {
        switch (eventType)
        {
            case GameEvents::EventType.JumpBoostPickupEffect:
                if (param is JumpBoostPickup jumpBoostPickup)
                {
                    extraJumpHeight = jumpBoostPickup.UseTime >= 0f ? 2f : 0f;
                    Color _colour = jumpParticles.startColor;
                    _colour.a = (jumpBoostPickup.UseTime * jumpBoostPickup.UseTime) / (jumpBoostPickup.Duration * jumpBoostPickup.Duration); // UseTime^2 / Duration^2 looks best
                    jumpParticles.startColor = _colour;
                    jumpParticles.gameObject.SetActive(jumpBoostPickup.UseTime >= 0f);
                }
                break;
            case GameEvents::EventType.BonusPickupEffect:
                bonusParticles.Play();
                break;
            case GameEvents::EventType.InvulnerabilityPickupEffect:
                if (param is InvulnerabilityPickup invulnerabilityPickup)
                {
                    Invulnerable = invulnerabilityPickup.UseTime >= 0f;
                    Color _color = shieldBubble.gameObject.GetComponent<MeshRenderer>().material.color;
                    _color.a = invulnerabilityPickup.UseTime / invulnerabilityPickup.Duration;
                    shieldBubble.gameObject.GetComponent<MeshRenderer>().material.color = _color;
                    shieldBubble.gameObject.SetActive(invulnerabilityPickup.UseTime >= 0f);
                }
                break;
        }
    }
}
