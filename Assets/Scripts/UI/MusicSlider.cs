using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class MusicSlider : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI musicValueLabel;

    Slider slider;

    void Awake() => slider = GetComponent<Slider>();

    void OnEnable()
    {
        if (UserSettingsManager.Instance == null) return;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
