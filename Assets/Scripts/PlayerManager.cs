using GameUtilities;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
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
    [SerializeField]
    [Tooltip("The time it takes the player to complete the jump (beginning the jump to the next time the player lands)")]
    [Min(0.001f)]
    float jumpTime;
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
    [Tooltip("It's gravity")]
    [Min(0.001f)]
    float gravity;
    [Header("Reset")]
    [SerializeField]
    [Tooltip("The time in seconds that the player must hold the reset button in order to reset")]
    [Min(0.001f)]
    float resetHoldTime;
    [SerializeField]
    [Tooltip("The level manager of the game")]
    LevelManager levelManager;

    const float SNAP_DISTANCE = 0.5f;
    const float SNAP_TO_GROUND_SPEED = -0.01f;

    CharacterController cc;
    int currentLane = 1;
    int targetLane = 1;
    int horizontalDirection;
    float vVel = SNAP_TO_GROUND_SPEED;
    float standHeight;
    float currentSlideTime;
    float currentResetHoldTime;
    bool pressingSlide;

    public bool CanSlide => currentSlideTime <= maxSlideTime;
    public bool IsSliding => CanSlide && pressingSlide;
    public bool CanMove => !IsSliding;
    public bool CanJump => cc.isGrounded && !IsSliding;
    public float ExtraJumpHeight { get; set; }

    void Awake()
    {
        cc = GetComponent<CharacterController>();
        standHeight = cc.height;
    }

    void Update()
    {
        if ((Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D)) && CanMove)
        {
            currentLane = targetLane;
            if (Input.GetKeyDown(KeyCode.A)) horizontalDirection = -1;
            if (Input.GetKeyDown(KeyCode.D)) horizontalDirection = 1;
            targetLane += horizontalDirection;
            targetLane = Mathf.Clamp(targetLane, 0, 2);
        }
        if (Input.GetKeyUp(KeyCode.A) && horizontalDirection == -1 || Input.GetKeyUp(KeyCode.D) && horizontalDirection == 1) horizontalDirection = 0;
        if (Input.GetKey(KeyCode.Space) && CanJump) vVel = 2 * (jumpHeight + ExtraJumpHeight) / jumpTime + gravity * jumpTime / 4f; // dx = (Vi)*(dt) + (1/2)*(a)*(dt)^2
        pressingSlide = Input.GetKey(KeyCode.LeftControl);
        if (!pressingSlide) currentSlideTime = 0f;
        if (Input.GetKey(KeyCode.R)) currentResetHoldTime += Time.deltaTime;
        else currentResetHoldTime = 0f;
        if (currentResetHoldTime >= resetHoldTime) levelManager.ResetGame();
    }

    void FixedUpdate()
    {
        if (IsSliding) currentSlideTime += Time.fixedDeltaTime;
        cc.height = IsSliding ? slideHeight : standHeight;
        cc.center = UtilityMethods.YVector(cc.height / 2f);
        Vector3 laneDisplacement = UtilityMethods.XVector(lanes[targetLane].position - lanes[currentLane].position);
        Vector3 hVel = laneDisplacement / switchTime;
        Vector3 movement = (hVel + UtilityMethods.YVector(vVel)) * Time.fixedDeltaTime;
        cc.Move(movement);
        if (cc.isGrounded) vVel = SNAP_TO_GROUND_SPEED;
        else vVel -= gravity * Time.fixedDeltaTime;
        float targetDistance = UtilityMethods.XDistance(transform.position, lanes[targetLane].position);
        if (targetDistance > SNAP_DISTANCE) return;
        transform.position = UtilityMethods.YZVector(transform.position) + UtilityMethods.XVector(lanes[targetLane].position);
        currentLane = targetLane;
        targetLane += horizontalDirection;
        targetLane = Mathf.Clamp(targetLane, 0, 2);
    }

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
        cc.Move(UtilityMethods.YVector(vVel));
    }
}
