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

    TMP_Dropdown dropdown;

    void Awake() => dropdown = GetComponent<TMP_Dropdown>();

    void OnEnable()
    {
        if (UserSettingsManager.Instance == null) return;
        dropdown.onValueChanged.RemoveAllListeners();
        dropdown.ClearOptions();
        dropdown.AddOptions(limits.ToList());
        int limit = UserSettingsManager.Instance.UserSettings.frameRateLimit;
        int possibleIndex = limits.ToList().IndexOf($"{limit}");
        int index = limit == 0 ? 0 : (possibleIndex == -1 ? 0 : possibleIndex);
        dropdown.value = index;
        dropdown.onValueChanged.AddListener(ChangeFrameLimit);
    }

    void ChangeFrameLimit(int index) => UserSettingsManager.Instance.UserSettings.frameRateLimit = (int.TryParse(dropdown.options[index].text, out int limit) ? limit : 0);
}
