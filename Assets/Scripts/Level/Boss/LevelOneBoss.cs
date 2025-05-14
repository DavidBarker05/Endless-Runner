using GameUtilities;
using System.Collections;
using UnityEngine;

public class LevelOneBoss : MonoBehaviour, IBoss
{
    public enum BossState
    {
        Run,
        Slide,
        Setback,
        Disengage,
    }

    [SerializeField]
    float speed;
    [SerializeField]
    float setbackSpeed;
    [SerializeField]
    float disengageSpeed;

    public BossState State { get; set; }

    void Awake() => State = BossState.Run;

    void FixedUpdate()
    {
        if (GameManager.Instance.State != GameManager.GameState.Alive) return;
        transform.position = UtilityMethods.YZVector(transform.position) + UtilityMethods.XVector(PlayerManager.Instance.transform.position);
        switch (State)
        {
            case BossState.Run:
                transform.position += transform.forward * (speed * Time.fixedDeltaTime);
                // Play running animation
                break;
            case BossState.Slide:
                // Play sliding animation
                break;
            case BossState.Setback:
                transform.position -= transform.forward * (setbackSpeed * Time.fixedDeltaTime);
                // Play running animation
                break;
            case BossState.Disengage:
                transform.position -= transform.forward * (disengageSpeed * Time.fixedDeltaTime);
                // Play idle animation
                break;
        }
    }

    public void Disengage()
    {
        State = BossState.Disengage;
        StartCoroutine(Deactivate());
    }

    IEnumerator Deactivate()
    {
        yield return new WaitForSeconds(1);
        Destroy(gameObject);
    }
}
