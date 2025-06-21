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
        float volume = UserSettingsManager.Instance.UserSettings.masterVolume * slider.maxValue;
        slider.value = Mathf.Clamp(volume, slider.minValue, slider.maxValue);
        masterValueLabel.text = $"{volume}";
        slider.onValueChanged.AddListener(ChangeMasterVolume);
    }

    void ChangeMasterVolume(float value)
    {
        float volume = Mathf.Round(value * 10f) / 10f; // Make volume have only 1 decimal
        masterValueLabel.text = $"{volume}";
        UserSettingsManager.Instance.UserSettings.masterVolume = volume / slider.maxValue;
    }
}
