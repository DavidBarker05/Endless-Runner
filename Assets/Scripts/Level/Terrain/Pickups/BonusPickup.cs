using UnityEngine;

public class BonusPickup : MonoBehaviour, IPickup
{
    public string Name => "Bonus";

    public float Duration => 0f;

    public float UseTime { get; set; }

    public void Effect() => LevelManager.Instance.Score += 5;
}
