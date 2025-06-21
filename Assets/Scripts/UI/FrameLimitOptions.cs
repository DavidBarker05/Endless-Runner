using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_Dropdown))]
public class FrameLimitOptions : MonoBehaviour
{
    readonly string[] limits = {
        "UNLIMITED",
        "30",
        "60",
        "80", // this is a vga standard if wondering (have seen some monitors use this)
        "120",
        "144",
        "165",
        "240",
        "360"
    };
    readonly List<string> limList = new List<string>();

    TMP_Dropdown dropdown;

    void Awake()
    {
        dropdown = GetComponent<TMP_Dropdown>();
        limList.AddRange(limits);
    }

    void OnEnable()
    {
        if (UserSettingsManager.Instance == null) return;
        dropdown.onValueChanged.RemoveAllListeners();
        dropdown.ClearOptions();
        limList.Clear();
        string limit = $"{UserSettingsManager.Instance.UserSettings.targetFrameRate}";
        if (UserSettingsManager.Instance.UserSettings.vsyncCount > 0) limList.Add("VSYNC");
        else
        {
            limList.AddRange(limits);
            if (!limList.Contains(limit) && limit != "0") limList.Insert(0, "CUSTOM");
        }
        dropdown.AddOptions(limList);
        dropdown.value = limList[0] == "CUSTOM" || limList[0] == "VSYNC" ? 0 : limList.IndexOf(limit);
        dropdown.onValueChanged.AddListener(ChangeFrameLimit);
        dropdown.enabled = limList[0] != "VSYNC";
    }

    void ChangeFrameLimit(int index)
    {
        string limit = dropdown.options[index].text;
        if (limList.Contains<string>("CUSTOM") && limit != "CUSTOM")
        {
            dropdown.onValueChanged.RemoveAllListeners();
            dropdown.ClearOptions();
            limList.RemoveAt(0);
            dropdown.AddOptions(limList);
            dropdown.value = limList.IndexOf(limit);
            dropdown.onValueChanged.AddListener(ChangeFrameLimit);
        }
        UserSettingsManager.Instance.UserSettings.targetFrameRate = (int.TryParse(limit, out int _limit) ? _limit : UserSettingsManager.Instance.UserSettings.targetFrameRate);
    }
}
