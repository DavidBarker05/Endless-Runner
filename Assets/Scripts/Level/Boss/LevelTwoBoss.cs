using GameEvents = GameUtilities.GameEvents;
using UnityEngine;

public class LevelTwoBoss : Boss, GameEvents::IEventListener
{
    void Awake()
    {
    }

    void Start()
    {
        GameManager.Instance.AddListener(GameEvents::EventType.BossTwoBeaten, this);
    }

    void Update()
    {
    }

    public void OnEvent(GameEvents::EventType eventType, Component sender, object param = null)
    {
        if (eventType != GameEvents::EventType.BossTwoBeaten) return;
        // Fly up?
        base.Disengage();
    }
}
