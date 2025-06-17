using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_Dropdown))]
public class VsyncOptions : MonoBehaviour
{
    TMP_Dropdown dropdown;

    void Awake() => dropdown = GetComponent<TMP_Dropdown>();

    void OnEnable()
    {
        if (UserSettingsManager.Instance == null) return;
        int vSyncMode = UserSettingsManager.Instance.UserSettings.vSyncMode;
        int vSyncClamp = Mathf.Clamp(vSyncMode, 0, 2);
        dropdown.onValueChanged.RemoveAllListeners();
        dropdown.value = vSyncClamp;
        dropdown.onValueChanged.AddListener(ChangeVsync);
        UserSettingsManager.Instance.UserSettings.vSyncMode = vSyncClamp;
    }

    void ChangeVsync(int index) => UserSettingsManager.Instance.UserSettings.vSyncMode = index;
}
