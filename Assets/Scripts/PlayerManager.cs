using UnityEngine;
using UtilityMethods = GameUtilities.UtilityMethods;

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
    [Header("Gravity")]
    [SerializeField]
    [Min(0.001f)]
    float gravity;

    Vector3[] pLanes = new Vector3[3];
    CharacterController cc;
    int currentLane = 1;
    int targetLane = 1;
    float vVel;

    const float SNAP_DISTANCE = 0.5f;
    const float SNAP_TO_GROUND_SPEED = -0.01f;

    void Start()
    {
        cc = GetComponent<CharacterController>();
        pLanes[0] = lanes[0].position;
        pLanes[1] = lanes[1].position;
        pLanes[2] = lanes[2].position;
        transform.position = pLanes[currentLane];
        vVel = SNAP_TO_GROUND_SPEED;
        cc.Move(UtilityMethods.YVector(vVel));
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D))
        {
            currentLane = targetLane;
            if (Input.GetKeyDown(KeyCode.A)) targetLane--;
            if (Input.GetKeyDown(KeyCode.D)) targetLane++;
            targetLane = Mathf.Clamp(targetLane, 0, 2);
        }
        if (Input.GetKey(KeyCode.Space) && cc.isGrounded) vVel = 2 * jumpHeight / jumpTime + gravity * jumpTime / 4f;
    }

    private void FixedUpdate()
    {
        Vector3 laneDisplacement = UtilityMethods.HorizontalVector(pLanes[targetLane] - pLanes[currentLane]);
        Vector3 hVel = laneDisplacement / switchTime;
        Vector3 movement = (hVel + UtilityMethods.YVector(vVel)) * Time.fixedDeltaTime;
        cc.Move(movement);
        if (cc.isGrounded) vVel = SNAP_TO_GROUND_SPEED;
        else vVel -= gravity * Time.fixedDeltaTime;
        float currentDistsance = UtilityMethods.HorizontalDistance(transform.position, pLanes[targetLane]);
        if (currentDistsance <= SNAP_DISTANCE)
        {
            transform.position = UtilityMethods.YVector(transform.position) + UtilityMethods.HorizontalVector(pLanes[targetLane]);
            currentLane = targetLane;
        }
    }
}
