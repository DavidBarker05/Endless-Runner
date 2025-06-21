using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_Dropdown))]
public class VsyncOptions : MonoBehaviour
{
    [SerializeField]
    TMP_Dropdown frameLimitDropdown;

    TMP_Dropdown dropdown;

    void Awake() => dropdown = GetComponent<TMP_Dropdown>();

    void OnEnable()
    {
        if (UserSettingsManager.Instance == null) return;
        int vSyncMode = UserSettingsManager.Instance.UserSettings.vsyncCount;
        int vSyncClamp = Mathf.Clamp(vSyncMode, 0, 2);
        dropdown.onValueChanged.RemoveAllListeners();
        dropdown.value = vSyncClamp;
        dropdown.onValueChanged.AddListener(ChangeVsync);
        UserSettingsManager.Instance.UserSettings.vsyncCount = vSyncClamp;
    }

    void ChangeVsync(int index)
    {
        UserSettingsManager.Instance.UserSettings.vsyncCount = index;
        if (frameLimitDropdown == null) return;
        if (index > 0)
        {
            frameLimitDropdown.onValueChanged.RemoveAllListeners();
            frameLimitDropdown.ClearOptions();
            frameLimitDropdown.AddOptions(new System.Collections.Generic.List<string>() { "VSYNC" });
            frameLimitDropdown.enabled = false;
        }
        else
        {
            frameLimitDropdown.enabled = true;
            frameLimitDropdown.gameObject.SetActive(false);
            frameLimitDropdown.gameObject.SetActive(true);
        }
    }
}
