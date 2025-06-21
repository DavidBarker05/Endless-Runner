using System;
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
    List<string> resList;
    List<string> customResList = new List<string>() { "CUSTOM" };

    TMP_Dropdown dropdown;

    void Awake()
    {
        dropdown = GetComponent<TMP_Dropdown>();
        resList = resolutions.ToList();
        customResList.AddRange(resolutions);
    }

    void OnEnable()
    {
        if (UserSettingsManager.Instance == null) return;
        dropdown.onValueChanged.RemoveAllListeners();
        dropdown.ClearOptions();
        string resolution = $"{UserSettingsManager.Instance.UserSettings.resolution[0]}x{UserSettingsManager.Instance.UserSettings.resolution[1]}";
        if (resList.Contains<string>(resolution))
        {
            dropdown.AddOptions(resList);
            dropdown.value = resList.IndexOf(resolution);
        }
        else dropdown.AddOptions(customResList);
        dropdown.onValueChanged.AddListener(ChangeResolution);
    }

    void ChangeResolution(int index)
    {
        string resolution = dropdown.options[index].text;
        if (resolutions.Contains(resolution) && dropdown.options[0].text == "CUSTOM")
        {
            dropdown.onValueChanged.RemoveAllListeners();
            dropdown.ClearOptions();
            dropdown.AddOptions(resList);
            dropdown.value = resList.IndexOf(resolution);
            dropdown.onValueChanged.AddListener(ChangeResolution);
        }
        UserSettingsManager.Instance.UserSettings.resolution[0] = resolution switch
        {
            "1280x720" => 1280,
            "1920x1080" => 1920,
            "2560x1440" => 2560,
            "3840x2160" => 3840,
            _ => UserSettingsManager.Instance.UserSettings.resolution[0]
        };
        UserSettingsManager.Instance.UserSettings.resolution[1] = resolution switch
        {
            "1280x720" => 720,
            "1920x1080" => 1080,
            "2560x1440" => 1440,
            "3840x2160" => 2160,
            _ => UserSettingsManager.Instance.UserSettings.resolution[1]
        };
    }
}
