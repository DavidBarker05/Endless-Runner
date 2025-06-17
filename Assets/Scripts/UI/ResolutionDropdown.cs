using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_Dropdown))]
public class ResolutionDropdown : MonoBehaviour
{
    readonly string[] resolutions = {
        "1280x720",
        "1920x1080",
        "2560x1440",
        "3840x2160"
    };

    TMP_Dropdown dropdown;

    void Awake() => dropdown = GetComponent<TMP_Dropdown>();

    void OnEnable()
    {
        if (UserSettingsManager.Instance == null) return;
        dropdown.onValueChanged.RemoveAllListeners();
        dropdown.ClearOptions();
        string resolution = $"{UserSettingsManager.Instance.UserSettings.screenWidth}x{UserSettingsManager.Instance.UserSettings.screenHeight}";
        if (resolutions.Contains(resolution))
        {
            dropdown.AddOptions(resolutions.ToList());
            dropdown.value = resolutions.ToList().IndexOf(resolution);
        }
        else
        {
            List<string> customOption = new List<string> { "CUSTOM" };
            customOption.AddRange(resolutions);
            dropdown.AddOptions(customOption);
        }
        dropdown.onValueChanged.AddListener(ChangeResolution);
    }

    void ChangeResolution(int index)
    {
        string resolution = dropdown.options[index].text;
        if (resolutions.Contains(resolution) && dropdown.options[0].text == "CUSTOM")
        {
            dropdown.onValueChanged.RemoveAllListeners();
            int _index = resolutions.ToList().IndexOf(resolution);
            dropdown.ClearOptions();
            dropdown.AddOptions(resolutions.ToList());
            dropdown.value = _index;
            dropdown.onValueChanged.AddListener(ChangeResolution);
        }
        UserSettingsManager.Instance.UserSettings.screenWidth = resolution switch
        {
            "1280x720" => 1280,
            "1920x1080" => 1920,
            "2560x1440" => 2560,
            "3840x2160" => 3840,
            _ => UserSettingsManager.Instance.UserSettings.screenWidth
        };
        UserSettingsManager.Instance.UserSettings.screenHeight = resolution switch
        {
            "1280x720" => 720,
            "1920x1080" => 1080,
            "2560x1440" => 1440,
            "3840x2160" => 2160,
            _ => UserSettingsManager.Instance.UserSettings.screenHeight
        };
    }
}
