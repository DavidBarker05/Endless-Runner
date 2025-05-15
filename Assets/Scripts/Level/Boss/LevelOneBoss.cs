using GameUtilities;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Animator))]
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

    Animator animator;

    public BossState State { get; set; }

    void Awake()
    {
        State = BossState.Run;
        animator = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        if (GameManager.Instance.State != GameManager.GameState.Alive) return;
        if (State != BossState.Slide) transform.position = UtilityMethods.YZVector(transform.position) + UtilityMethods.XVector(PlayerManager.Instance.transform.position);
        transform.position += UtilityMethods.ZVector((State == BossState.Run ? speed : State == BossState.Setback ? -setbackSpeed : State == BossState.Disengage ? -LevelManager.Instance.Speed : 0f) * Time.fixedDeltaTime);
        animator.SetInteger("AnimationState", (int)State);
    }

    void OnTriggerEnter(Collider other)
    {
        if (State == BossState.Disengage) return;
        if (other.CompareTag("BossSlideToggle")) State = BossState.Slide;
    }

    void OnTriggerExit(Collider other)
    {
        if (State == BossState.Disengage) return;
        if (other.CompareTag("BossSlideToggle")) State = BossState.Run;
    }

    public void Disengage()
    {
        State = BossState.Disengage;
        StartCoroutine(Deactivate());
    }

    IEnumerator Deactivate()
    {
        yield return new WaitForSeconds(2.5f);
        Destroy(gameObject);
    }
}
