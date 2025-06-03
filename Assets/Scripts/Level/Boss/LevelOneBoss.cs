using GameUtilities;
using GameEvents = GameUtilities.GameEvents;
using UnityEngine;

/// <summary>
/// <para>
/// The script for the level one boss.
/// </para>
/// <para>
/// Derives from the <see cref="Boss"/> script and implements the <see cref="GameEvents::IEventListener"/> interface.
/// </para>
/// <para>
/// Requires an <see cref="Animator"/> component to be attached.
/// </para>
/// </summary>
[RequireComponent(typeof(Animator))]
public class LevelOneBoss : Boss, GameEvents::IEventListener
{
    /// <summary>
    /// <para>
    /// The different states that the boss can be in.
    /// </para>
    /// <para>
    /// Determines how the boss behaves as well as what animation to play.
    /// </para>
    /// </summary>
    public enum BossState
    {
        /// <summary>
        /// <para>
        /// The boss runs forward.
        /// </para>
        /// </summary>
        Run,
        /// <summary>
        /// <para>
        /// The boss slides.
        /// </para>
        /// </summary>
        Slide,
        /// <summary>
        /// <para>
        /// The boss is being set back by the <see cref="BossOnePickup"/>.
        /// </para>
        /// </summary>
        Setback,
        /// <summary>
        /// <para>
        /// The boss disengages.
        /// </para>
        /// <para>
        /// Calls the base method <see cref="Boss.Disengage"/>.
        /// </para>
        /// <para>
        /// Is used when the <see cref="GameEvents::EventType.BossOneBeaten"/> event is invoked by <see cref="GameManager.InvokeEvent(GameEvents.EventType, Component, object)"/>.
        /// </para>
        /// </summary>
        Disengage,
        /// <summary>
        /// <para>
        /// The boss has caught the player.
        /// </para>
        /// </summary>
        Catch,
    }

    [Tooltip("The speed at which the boss moves forward.")]
    [SerializeField]
    float speed;
    [Tooltip("The speed at which the boss moves backwards when they are set back.")]
    [SerializeField]
    float setbackSpeed;
    [Tooltip("The particles that play when the boss is set back.")]
    [SerializeField]
    ParticleSystem setbackParticles;

    Animator animator;
    CapsuleCollider c;

    /// <summary>
    /// <para>
    /// The current <see cref="BossState"/> that the boss is in.
    /// </para>
    /// </summary>
    public BossState State { get; private set; }

    void Awake()
    {
        State = BossState.Run; // Start in the running state
        animator = GetComponent<Animator>();
        c = GetComponent<CapsuleCollider>();
    }

    void Start()
    {
        GameManager.Instance.AddListener(GameEvents::EventType.BossOnePickupEffect, this); // Add this to the list of listeners for the BossOnePickupEffect event when the boss is created
        GameManager.Instance.AddListener(GameEvents::EventType.BossOneBeaten, this); // Add this to the list of listeners for the BossOneBeaten event when the boss is created
    }

    void FixedUpdate()
    {
        if (GameManager.Instance.State == GameManager.GameState.Dead) State = BossState.Catch; // Force boss to idle even if player dies to something that isn't the boss
        animator.SetInteger("AnimationState", (int)State); // Set the animation state to the boss state
        if (GameManager.Instance.State != GameManager.GameState.Alive) return; // If the player is dead don't bother with the following movement code
        if (State != BossState.Slide) transform.position = UtilityMethods.YZVector(transform.position) + UtilityMethods.XVector(PlayerManager.Instance.transform.position); // Match the horizontal position of the player if the boss isn't sliding
        transform.position += UtilityMethods.ZVector((State == BossState.Run ? speed : State == BossState.Setback ? -setbackSpeed : State == BossState.Disengage ? -LevelManager.Instance.Speed / Time.fixedDeltaTime : 0f) * Time.fixedDeltaTime); // The speed at which the boss moves
        transform.position = new Vector3(transform.position.x, transform.position.y, Mathf.Clamp(transform.position.z, -5f, 0f)); // Make sure boss can't go too far back
    }

    void OnTriggerEnter(Collider other)
    {
        if (State == BossState.Disengage || State == BossState.Setback) return; // If the boss is disengaging or being set back then don't slide
        if (other.CompareTag("BossSlideToggle")) // If boss enters slide toggle then slide
        {
            State = BossState.Slide; // Set State to Slide
            c.height = 1.1f; // Change collider height to be the sliding height
            c.center = new Vector3(0f, 0.55f, 0f); // Adjust the collider centre so that the bottom touches the ground
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (State == BossState.Disengage || State == BossState.Setback) return;  // If the boss is disengaging or being set back then don't bother making them run out of a slide
        if (other.CompareTag("BossSlideToggle")) // If boss exits slide toggle then stop sliding
        {
            State = BossState.Run; // Set State to Run
            c.height = 1.8f; // Change collider height to be the standing height
            c.center = new Vector3(0f, 0.9f, 0f); // Adjust the collider centre so that the bottom touches the ground
        }
    }

    void OnDestroy()
    {
        GameManager.Instance.RemoveListener(GameEvents::EventType.BossOnePickupEffect, this); // Remove this from the list of listeners for the BossOnePickupEffect event when the boss is destroyed
        GameManager.Instance.RemoveListener(GameEvents::EventType.BossOneBeaten, this); // Remove this from the list of listeners for the BossOneBeaten event when the boss is destroyed
    }

    public void OnEvent(GameEvents::EventType eventType, Component sender, object param = null)
    {
        if (eventType == GameEvents::EventType.BossOnePickupEffect) // Code that executes during the BossOnePickupEffect event
        {
            if (State == BossState.Disengage || State == BossState.Slide) return; // If the boss is currently disengaging or sliding then don't set them back
            State = (float)param >= 0f ? BossState.Setback : BossState.Run; // If the use time for the pickup is greater than or equal to 0 set State to Setback otherwise set State to Run
            setbackParticles.gameObject.SetActive((float)param >= 0f); // Make the setback particles visible if the use time is greater than or equal to 0
        }
        else if (eventType == GameEvents::EventType.BossOneBeaten) // Code that executes during the BossOneBeaten event
        {
            State = BossState.Disengage; // Set State to Disengage
            base.Disengage(); // Call the base Disengage method
        }
    }
}
