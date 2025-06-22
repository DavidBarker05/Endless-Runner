using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class MasterSlider : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI masterValueLabel;

    Slider slider;

    void Awake() => slider = GetComponent<Slider>();

    void OnEnable()
    {
        if (UserSettingsManager.Instance == null) return;
        slider.onValueChanged.RemoveAllListeners();
        float volume = UserSettingsManager.Instance.UserSettings.masterVolume;
        slider.value = Mathf.Clamp(volume, slider.minValue, slider.maxValue);
        masterValueLabel.text = $"{Mathf.Floor(volume * 1000f) / 10f}";
        slider.onValueChanged.AddListener(ChangeMasterVolume);
    }

    void ChangeMasterVolume(float value)
    {
        float volume = Mathf.Round(value * 10000f) / 10000f; // Make volume have only 4 decimal
        masterValueLabel.text = $"{Mathf.Floor(volume * 1000f) / 10f}";
        UserSettingsManager.Instance.UserSettings.masterVolume = volume;
    }
}
