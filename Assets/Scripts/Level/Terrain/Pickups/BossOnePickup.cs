using UnityEngine;

public class BossOnePickup : MonoBehaviour, IPickup
{
    public string Name => "BossOnePickup";

    public float Duration => 5f;

    public float UseTime { get; set; }

    void Awake() => UseTime = Duration;

    LevelOneBoss levelOneBoss;

    public void Effect()
    {
        levelOneBoss = FindFirstObjectByType<LevelOneBoss>();
        if (levelOneBoss.State == LevelOneBoss.BossState.Disengage || levelOneBoss.State == LevelOneBoss.BossState.Slide) return;
        levelOneBoss.State = UseTime >= 0f ? LevelOneBoss.BossState.Setback : LevelOneBoss.BossState.Run;
    }
}
