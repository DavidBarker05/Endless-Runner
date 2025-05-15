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
        switch (State)
        {
            case BossState.Run:
                transform.position += transform.forward * (speed * Time.fixedDeltaTime);
                animator.SetInteger("AnimationState", 0);
                break;
            case BossState.Slide:
                animator.SetInteger("AnimationState", 1);
                break;
            case BossState.Setback:
                transform.position -= transform.forward * (setbackSpeed * Time.fixedDeltaTime);
                animator.SetInteger("AnimationState", 2);
                break;
            case BossState.Disengage:
                transform.position -= UtilityMethods.ZVector(LevelManager.Instance.Speed);
                animator.SetInteger("AnimationState", 3);
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
        yield return new WaitForSeconds(2.5f);
        Destroy(gameObject);
    }
}
