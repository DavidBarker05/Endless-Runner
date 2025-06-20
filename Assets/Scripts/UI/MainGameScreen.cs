using GameUtilities.UtilityMethods;
using GameEvents = GameUtilities.GameEvents;
using System.Collections.Generic;
using Array = System.Array;
using TMPro;
using UnityEngine;
using GameUtilities.GameEvents;

public class MainGameScreen : MonoBehaviour, IEventListener
{
    [SerializeField]
    TextMeshProUGUI currentScoreLabel;
    [SerializeField]
    TextMeshProUGUI bossesBeatenLabel;

    void Start()
    {
        GameManager.Instance.AddListener(GameEvents::EventType.ObstaclePassed, this);
        GameManager.Instance.AddListener(GameEvents::EventType.BonusPickupEffect, this);
        GameManager.Instance.AddListener(GameEvents::EventType.BossOneBeaten, this);
        GameManager.Instance.AddListener(GameEvents::EventType.BossTwoBeaten, this);
    }

    void OnDestroy()
    {
        GameManager.Instance.RemoveListener(GameEvents::EventType.ObstaclePassed, this);
        GameManager.Instance.RemoveListener(GameEvents::EventType.BonusPickupEffect, this);
        GameManager.Instance.RemoveListener(GameEvents::EventType.BossOneBeaten, this);
        GameManager.Instance.RemoveListener(GameEvents::EventType.BossTwoBeaten, this);
    }

    public void OnEvent(GameEvents.EventType eventType, Component sender, object param = null)
    {
        if (eventType == GameEvents::EventType.ObstaclePassed || eventType == GameEvents::EventType.BonusPickupEffect)
        {
            if (param is int score) currentScoreLabel.text = $"SCORE: {score}";
        }
        else if (eventType == GameEvents::EventType.BossOneBeaten || eventType == GameEvents::EventType.BossTwoBeaten)
        {
            if (param is int beaten) bossesBeatenLabel.text = $"BOSSES BEATEN: {beaten}";
        }
    }
}
