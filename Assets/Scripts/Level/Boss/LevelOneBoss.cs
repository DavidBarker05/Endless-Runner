using GameUtilities;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class LevelOneBoss : Boss
{
    public enum BossState
    {
        Run,
        Slide,
        Setback,
        Disengage,
        Catch,
    }

    [SerializeField]
    float speed;
    [SerializeField]
    float setbackSpeed;
    [SerializeField]
    ParticleSystem setbackParticles;

    Animator animator;
    CapsuleCollider c;

    public BossState State { get; set; }
    public ParticleSystem SetbackParticles { get => setbackParticles; set => setbackParticles = value; }

    void Awake()
    {
        State = BossState.Run;
        animator = GetComponent<Animator>();
        c = GetComponent<CapsuleCollider>();
    }

    void FixedUpdate()
    {
        if (GameManager.Instance.State == GameManager.GameState.Dead) State = BossState.Catch;
        animator.SetInteger("AnimationState", (int)State);
        if (GameManager.Instance.State != GameManager.GameState.Alive) return;
        if (State != BossState.Slide) transform.position = UtilityMethods.YZVector(transform.position) + UtilityMethods.XVector(PlayerManager.Instance.transform.position);
        transform.position += UtilityMethods.ZVector((State == BossState.Run ? speed : State == BossState.Setback ? -setbackSpeed : State == BossState.Disengage ? -LevelManager.Instance.Speed / Time.fixedDeltaTime : 0f) * Time.fixedDeltaTime);
        transform.position = new Vector3(transform.position.x, transform.position.y, Mathf.Clamp(transform.position.z, -5f, 0f));
    }

    void OnTriggerEnter(Collider other)
    {
        if (State == BossState.Disengage || State == BossState.Setback) return;
        if (other.CompareTag("BossSlideToggle"))
        {
            State = BossState.Slide;
            c.height = 1.1f;
            c.center = new Vector3(0f, 0.55f, 0f);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (State == BossState.Disengage || State == BossState.Setback) return;
        if (other.CompareTag("BossSlideToggle"))
        {
            State = BossState.Run;
            c.height = 1.8f;
            c.center = new Vector3(0f, 0.9f, 0f);
        }
    }

    public override void Disengage()
    {
        State = BossState.Disengage;
        base.Disengage();
    }
}
