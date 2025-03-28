using UnityEngine;
using GameUtilities;

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

    public bool IsSliding { get; private set; }
    public float ExtraJumpHeight { private get; set; }

    CharacterController cc;
    int currentLane = 1;
    int targetLane = 1;
    float vVel;
    float standHeight;
    float currentSlideTime = 0f;
    float currentResetHoldTime = 0f;
    bool pressingSlide = false;

    const float SNAP_DISTANCE = 0.5f;
    const float SNAP_TO_GROUND_SPEED = -0.01f;

    void Start()
    {
        cc = GetComponent<CharacterController>();
        standHeight = cc.height;
        IsSliding = false;
        ExtraJumpHeight = 0f;
        cc.enabled = false;
        transform.position = lanes[currentLane].position;
        cc.enabled = true;
        vVel = SNAP_TO_GROUND_SPEED;
        cc.Move(UtilityMethods.YVector(vVel));
    }

    void Update()
    {
        if ((Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D)) && !IsSliding)
        {
            currentLane = targetLane;
            if (Input.GetKeyDown(KeyCode.A)) targetLane--;
            if (Input.GetKeyDown(KeyCode.D)) targetLane++;
            targetLane = Mathf.Clamp(targetLane, 0, 2);
        }
        if (Input.GetKey(KeyCode.Space) && cc.isGrounded && !IsSliding) vVel = 2 * (jumpHeight + ExtraJumpHeight) / jumpTime + gravity * jumpTime / 4f;
        pressingSlide = Input.GetKey(KeyCode.LeftControl);
        if (!pressingSlide) currentSlideTime = 0f;
        if (Input.GetKey(KeyCode.R)) currentResetHoldTime += Time.deltaTime;
        if (Input.GetKeyUp(KeyCode.R)) currentResetHoldTime = 0f;
        if (currentResetHoldTime >= resetHoldTime) levelManager.ResetGame();
    }

    private void FixedUpdate()
    {
        IsSliding = pressingSlide && currentSlideTime <= maxSlideTime;
        if (IsSliding)
        {
            currentSlideTime += Time.fixedDeltaTime;
            cc.height = slideHeight;
            cc.center = UtilityMethods.YVector(slideHeight / 2f);
        }
        else
        {
            cc.height = standHeight;
            cc.center = UtilityMethods.YVector(standHeight / 2f);
        }
        Vector3 laneDisplacement = UtilityMethods.HorizontalVector(lanes[targetLane].position - lanes[currentLane].position);
        Vector3 hVel = laneDisplacement / switchTime;
        Vector3 movement = (hVel + UtilityMethods.YVector(vVel)) * Time.fixedDeltaTime;
        cc.Move(movement);
        if (cc.isGrounded) vVel = SNAP_TO_GROUND_SPEED;
        else vVel -= gravity * Time.fixedDeltaTime;
        float currentDistsance = UtilityMethods.HorizontalDistance(transform.position, lanes[targetLane].position);
        if (currentDistsance <= SNAP_DISTANCE)
        {
            transform.position = UtilityMethods.YVector(transform.position) + UtilityMethods.HorizontalVector(lanes[targetLane].position);
            currentLane = targetLane;
        }
    }

    public void ResetPlayer()
    {
        IsSliding = false;
        ExtraJumpHeight = 0f;
        currentLane = 1;
        targetLane = 1;
        cc.enabled = false;
        transform.position = lanes[currentLane].position;
        cc.enabled = true;
        vVel = SNAP_TO_GROUND_SPEED;
        cc.Move(UtilityMethods.YVector(vVel));
        currentSlideTime = 0f;
        currentResetHoldTime = 0f;
        pressingSlide = false;
    }
}
