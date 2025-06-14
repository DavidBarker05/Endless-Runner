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

    bool isWallrunning = false;

    void Awake()
    {
    }

    void Start()
    {
    }

    void Update()
    {
    }

    void FixedUpdate()
    {
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        PlayerManager.Instance.State = surfaceSide == SurfaceSide.Right ? PlayerManager.AnimationState.WallrunRight : PlayerManager.AnimationState.WallrunLeft;
        isWallrunning = true;
    }

    void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        PlayerManager.Instance.State = PlayerManager.AnimationState.Fall;
        isWallrunning = false;
    }
}
