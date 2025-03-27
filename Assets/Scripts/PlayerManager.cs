using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [SerializeField]
    Transform[] lanes = new Transform[3];
    [SerializeField]
    [Range(0.01f, 1f)]
    float switchTime;
    [SerializeField]
    float gravity;

    CharacterController cc;

    int currentLane = 1;
    int targetLane = 1;
    float snapDistance = 0.5f;

    void Start()
    {
        cc = GetComponent<CharacterController>();
        transform.position = lanes[currentLane].position;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            currentLane = targetLane;
            targetLane--;
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            currentLane = targetLane;
            targetLane++;
        }
        targetLane = Mathf.Clamp(targetLane, 0, 2);
    }

    private void FixedUpdate()
    {
        if (currentLane != targetLane)
        {
            Vector3 dist = lanes[targetLane].position - lanes[currentLane].position;
            Vector3 movement = 1f / switchTime * Time.fixedDeltaTime * dist;
            cc.Move(movement);
            float currentDistsance = Vector3.Distance(new Vector3(transform.position.x, lanes[targetLane].position.y, transform.position.z), lanes[targetLane].position);
            if (currentDistsance <= snapDistance)
            {
                transform.position = new Vector3(lanes[targetLane].position.x, transform.position.y, lanes[targetLane].position.z);
                currentLane = targetLane;
            }
        }
    }
}
