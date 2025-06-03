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
}
