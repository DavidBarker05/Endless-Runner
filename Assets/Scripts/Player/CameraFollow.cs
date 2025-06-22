using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraFollow : MonoBehaviour
{
    void FixedUpdate() => transform.position = new Vector3(PlayerManager.Instance.transform.position.x, transform.position.y, transform.position.z);
}
