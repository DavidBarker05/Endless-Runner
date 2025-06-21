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
        float volume = UserSettingsManager.Instance.UserSettings.effectsVolume;
        slider.value = Mathf.Clamp(volume, slider.minValue, slider.maxValue);
        effectsValueLabel.text = $"{volume * 100f}";
        slider.onValueChanged.AddListener(ChangeEffectsVolume);
    }

    void ChangeEffectsVolume(float value)
    {
        float volume = Mathf.Round(value * 10000f) / 10000f; // Make volume have only 4 decimal
        effectsValueLabel.text = $"{Mathf.Round(volume * 1000f) / 1000f * 100f}";
        UserSettingsManager.Instance.UserSettings.effectsVolume = volume;
    }
}
