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
        float volume = UserSettingsManager.Instance.UserSettings.musicVolume * slider.maxValue;
        slider.value = Mathf.Clamp(volume, slider.minValue, slider.maxValue);
        musicValueLabel.text = $"{volume}";
        slider.onValueChanged.AddListener(ChangeMusicVolume);
    }

    void ChangeMusicVolume(float value)
    {
        float volume = Mathf.Round(value * 1000f) / 1000f; // Make volume have only 3 decimal
        musicValueLabel.text = $"{volume * 100f}";
        UserSettingsManager.Instance.UserSettings.musicVolume = volume;
    }
}
