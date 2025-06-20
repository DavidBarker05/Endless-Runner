using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class EffectsSlider : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI effectsValueLabel;

    Slider slider;

    void Awake() => slider = GetComponent<Slider>();

    void OnEnable()
    {
        if (UserSettingsManager.Instance == null) return;
        slider.onValueChanged.RemoveAllListeners();
        float volume = UserSettingsManager.Instance.UserSettings.soundVolume * slider.maxValue;
        slider.value = Mathf.Clamp(volume, slider.minValue, slider.maxValue);
        effectsValueLabel.text = $"{volume}";
        slider.onValueChanged.AddListener(ChangeEffectsVolume);
    }

    void ChangeEffectsVolume(float value)
    {
        float volume = Mathf.Round(value * 10f) / 10f; // Make volume have only 1 decimal
        effectsValueLabel.text = $"{volume}";
        UserSettingsManager.Instance.UserSettings.soundVolume = volume / slider.maxValue;
    }
}
