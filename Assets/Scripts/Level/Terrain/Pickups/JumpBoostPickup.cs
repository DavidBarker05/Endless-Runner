using UnityEngine;

public class JumpBoostPickup : MonoBehaviour, IPickup
{
    /// <summary>
    /// Jump boost name
    /// </summary>
    public string Name => "JumpBoostPickup";
    /// <summary>
    /// Jump boost duration
    /// </summary>
    public float Duration => 3f;
    /// <summary>
    /// The current use time of the jump boost
    /// </summary>
    public float UseTime { get; set; }

    private void Awake() => UseTime = Duration;

    /// <summary>
    /// Jump boost effect
    /// </summary>
    public void Effect()
    {
        PlayerManager.Instance.ExtraJumpHeight = UseTime >= 0f ? 2f : 0f; // If greater than 0 give jump boost else don't
        PlayerManager.Instance.JumpParticles.gameObject.SetActive(UseTime >= 0f);
    }
}
