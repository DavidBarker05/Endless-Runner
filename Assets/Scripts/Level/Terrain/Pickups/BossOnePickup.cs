using UnityEngine;

public class BossOnePickup : MonoBehaviour, IPickup
{
    public string Name => "BossOnePickup";

    public float Duration => 3f;

    public float UseTime { get; set; }

    void Awake() => UseTime = Duration;
}
