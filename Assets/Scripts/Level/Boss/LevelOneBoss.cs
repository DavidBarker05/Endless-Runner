using GameUtilities;
using GameEvents = GameUtilities.GameEvents;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class LevelOneBoss : Boss, GameEvents::IEventListener
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

    public BossState State { get; private set; }

    void Awake()
    {
        State = BossState.Run;
        animator = GetComponent<Animator>();
        c = GetComponent<CapsuleCollider>();
    }

    void Start()
    {
        GameManager.Instance.AddListener(GameEvents::EventType.BossOnePickupEffect, this);
        GameManager.Instance.AddListener(GameEvents::EventType.BossOneBeaten, this);
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

    public void OnEvent(GameEvents::EventType eventType, Component sender, object param = null)
    {
        if (eventType == GameEvents::EventType.BossOnePickupEffect)
        {
            if (State == BossState.Disengage || State == BossState.Slide) return;
            State = (float)param >= 0f ? BossState.Setback : BossState.Run;
            setbackParticles.gameObject.SetActive((float)param >= 0f);
        }
        else if (eventType == GameEvents::EventType.BossOneBeaten)
        {
            State = BossState.Disengage;
            base.Disengage();
        }
    }
}
