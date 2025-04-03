using UnityEngine;

public class PickupsGeneral : MonoBehaviour
{

    [SerializeField]
    private float durationPickup = 10f;
    private bool pickUpActive = false;
    private float timer = 0;
    private GameObject player;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (pickUpActive)
        {
            if (timer <= durationPickup)
            {
                timer += Time.deltaTime;
            }
            else
            {
                pickUpActive = false;
                player.GetComponent<PlayerManager>().ExtraJumpHeight = 0;//Back to normal Jump heiaght
                Destroy(gameObject);
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision != null)
        {
            if (collision.gameObject.GetComponent<PlayerManager>() != null)
            {
                player = collision.gameObject;
                player.GetComponent<PlayerManager>().ExtraJumpHeight = 10;//Change to desired strength
                pickUpActive = true;
                GetComponent<Collider>().enabled = false;
                GetComponent<MeshRenderer>().enabled = false;
            }
        }
    }
}