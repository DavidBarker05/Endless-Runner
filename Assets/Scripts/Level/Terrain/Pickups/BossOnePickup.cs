using UnityEngine;

public class BossOnePickup : MonoBehaviour, IPickup
{
    public string Name => "Boss One";

    public float Duration => 5f;

    public float UseTime { get; set; }

    LevelOneBoss levelOneBoss;

    public void Effect()
    {
        if (levelOneBoss == null) levelOneBoss = levelOneBoss = FindFirstObjectByType<LevelOneBoss>();
        if (levelOneBoss.State == LevelOneBoss.BossState.Disengage) return;
        levelOneBoss.State = UseTime >= 0f ? LevelOneBoss.BossState.Setback : LevelOneBoss.BossState.Run;
    }
}
