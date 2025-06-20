using GameUtilities.UtilityMethods;
using GameEvents = GameUtilities.GameEvents;
using System.Collections;
using UnityEngine;

public class LevelTwoBoss : Boss, GameEvents::IEventListener
{
    [SerializeField]
    float flyInTime;
    [SerializeField]
    float activeY;
    [SerializeField]
    Transform[] lanes = new Transform[3];
    [SerializeField]
    float laneSwitchTime;
    [SerializeField]
    float shotChargeTime;
    [SerializeField]
    GameObject laserShot;
    [SerializeField]
    float shotDuration;
    [SerializeField]
    float cooldownTime;
    [SerializeField]
    float disengageSpeed;

    float startY;
    int currentLane = 1;
    int targetLane = 1;
    bool isDisengaging = false;

    void Awake()
    {
        startY = transform.position.y;
        StartCoroutine(FlyIn());
    }

    void Start() => GameManager.Instance.AddListener(GameEvents::EventType.BossTwoBeaten, this);

    void OnDestroy() => GameManager.Instance.RemoveListener(GameEvents::EventType.BossTwoBeaten, this);

    IEnumerator FlyIn()
    {
        float timer = 0f;
        while (timer < flyInTime)
        {
            while (GameManager.Instance.State == GameManager.GameState.Paused) yield return null;
            timer += Time.fixedDeltaTime;
            transform.position += UtilMethods.YVector((activeY - startY) / flyInTime * Time.fixedDeltaTime);
            yield return new WaitForFixedUpdate();
        }
        if (GameManager.Instance.State == GameManager.GameState.Dead) yield break; // If dead don't start next action
        StartCoroutine(SwitchLanes());
    }

    IEnumerator SwitchLanes()
    {
        if (isDisengaging) yield break; // If disengaging don't switch lanes
        targetLane = Random.Range(0, lanes.Length);
        float timer = 0f;
        while (timer < laneSwitchTime && !isDisengaging)
        {
            while (GameManager.Instance.State == GameManager.GameState.Paused) yield return null;
            timer += Time.fixedDeltaTime;
            transform.position += new Vector3((lanes[targetLane].position.x - lanes[currentLane].position.x) / laneSwitchTime * Time.fixedDeltaTime, 0f);
            yield return new WaitForFixedUpdate();
        }
        if (isDisengaging) yield break; // If disengaging don't snap to target lane and don't start next action
        currentLane = targetLane;
        transform.position = new Vector3(lanes[currentLane].position.x, transform.position.y, transform.position.z);
        if (GameManager.Instance.State == GameManager.GameState.Dead) yield break; // If dead don't start next action
        StartCoroutine(ChargeShot());
    }

    IEnumerator ChargeShot()
    {
        if (isDisengaging) yield break; // If disengaging don't charge shot
        float timer = 0f;
        while (timer < shotChargeTime && !isDisengaging)
        {
            while (GameManager.Instance.State == GameManager.GameState.Paused) yield return null;
            timer += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }
        if (isDisengaging || GameManager.Instance.State == GameManager.GameState.Dead) yield break; // If disengaging or dead don't start next action
        StartCoroutine(Shoot());
    }

    IEnumerator Shoot()
    {
        if (isDisengaging || GameManager.Instance.State == GameManager.GameState.Dead) yield break; // If disengaging or dead don't shoot
        GameObject _laserShot = Instantiate(laserShot, transform);
        _laserShot.transform.position = transform.position;
        float timer = 0f;
        while (timer < shotDuration && !isDisengaging)
        {
            while (GameManager.Instance.State == GameManager.GameState.Paused) yield return null;
            timer += Time.fixedDeltaTime;
            if (PlayerManager.Instance.transform.position.x == lanes[currentLane].position.x && !PlayerManager.Instance.Invulnerable) GameManager.Instance.State = GameManager.GameState.Dead;
            yield return new WaitForFixedUpdate();
        }
        Destroy(_laserShot);
        if (isDisengaging || GameManager.Instance.State == GameManager.GameState.Dead) yield break; // If disengaging or dead don't start next action
        StartCoroutine(CoolDown());
    }

    IEnumerator CoolDown()
    {
        if (isDisengaging) yield break; // If disengaging don't cool down
        float timer = 0f;
        while (timer < cooldownTime && !isDisengaging)
        {
            while (GameManager.Instance.State == GameManager.GameState.Paused) yield return null;
            timer += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }
        if (isDisengaging || GameManager.Instance.State == GameManager.GameState.Dead) yield break;
        StartCoroutine(SwitchLanes()); // If disengaging or dead don't start next action
    }

    IEnumerator FlyUp()
    {
        while (true)
        {
            if (GameManager.Instance.State == GameManager.GameState.Dead) yield break;
            while (GameManager.Instance.State == GameManager.GameState.Paused) yield return null;
            transform.position += UtilMethods.YVector(disengageSpeed * Time.fixedDeltaTime);
            yield return new WaitForFixedUpdate();
        }
    }

    public void OnEvent(GameEvents::EventType eventType, Component sender, object param = null)
    {
        if (eventType != GameEvents::EventType.BossTwoBeaten) return;
        isDisengaging = true;
        StartCoroutine(FlyUp());
        base.Disengage();
    }
}
