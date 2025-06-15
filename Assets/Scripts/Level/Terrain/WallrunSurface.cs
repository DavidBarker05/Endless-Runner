using System.Collections;
using UnityEngine;

public class WallrunSurface : MonoBehaviour
{
    public enum SurfaceSide
    {
        Right,
        Left,
    }

    [SerializeField]
    SurfaceSide surfaceSide;
    [SerializeField]
    float wallrunY;
    [SerializeField]
    float yTransitionTime;

    Coroutine changePlayerY;

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        PlayerManager.Instance.State = surfaceSide == SurfaceSide.Right ? PlayerManager.AnimationState.WallrunRight : PlayerManager.AnimationState.WallrunLeft;
        if (changePlayerY != null) StopCoroutine(changePlayerY);
        changePlayerY = StartCoroutine(ChangePlayerY());
    }

    void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        PlayerManager.Instance.State = PlayerManager.AnimationState.Fall;
        if(changePlayerY != null) StopCoroutine(changePlayerY);
    }

    IEnumerator ChangePlayerY()
    {
        Vector3 target = new Vector3(PlayerManager.Instance.transform.position.x, Mathf.Max(PlayerManager.Instance.transform.position.y, wallrunY));
        while (PlayerManager.Instance.transform.position.y < target.y)
        {
            // while (paused) yield return null;
            float y = target.y - PlayerManager.Instance.transform.position.y;
            float dy = (y / yTransitionTime) * Time.fixedDeltaTime;
            Vector3 dyVector = GameUtilities.UtilityMethods.UtilMethods.YVector(dy);
            PlayerManager.Instance.transform.position += dyVector;
            yield return new WaitForFixedUpdate();
        }
        PlayerManager.Instance.transform.position = target;
    }
}
