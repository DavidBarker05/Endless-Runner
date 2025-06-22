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
        slider.onValueChanged.RemoveAllListeners();
        float volume = UserSettingsManager.Instance.UserSettings.musicVolume;
        slider.value = Mathf.Clamp(volume, slider.minValue, slider.maxValue);
        musicValueLabel.text = $"{Mathf.Floor(volume * 1000f) / 10f}";
        slider.onValueChanged.AddListener(ChangeMusicVolume);
    }

    void ChangeMusicVolume(float value)
    {
        float volume = Mathf.Round(value * 10000f) / 10000f; // Make volume have only 4 decimal
        musicValueLabel.text = $"{Mathf.Floor(volume * 1000f) / 10f}";
        UserSettingsManager.Instance.UserSettings.musicVolume = volume;
    }
}
