using System.IO;
using UnityEngine;

public class UserSettingsManager : MonoBehaviour
{
    public static UserSettingsManager Instance { get; private set; }

    string path = "";
    UserSettings previousSettings;

    public UserSettings UserSettings { get; set; }

    void Awake()
    {
        if (Instance != null && Instance != this) Destroy(gameObject);
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        path = Path.Combine(Application.persistentDataPath, "user_settings.json");
        if (!File.Exists(path))
        {
            UserSettings = new UserSettings();
            SaveSettings();
        }
        else LoadSettings();
        previousSettings = new UserSettings(UserSettings);
    }

    void Update()
    {
        if (previousSettings.resolution[0] != UserSettings.resolution[0] || previousSettings.resolution[1] != UserSettings.resolution[1])
        {
            Screen.SetResolution(UserSettings.resolution[0], UserSettings.resolution[1], fullscreen: true);
            if (previousSettings.resolution[0] != UserSettings.resolution[0]) previousSettings.resolution[0] = UserSettings.resolution[0];
            if (previousSettings.resolution[1] != UserSettings.resolution[1]) previousSettings.resolution[1] = UserSettings.resolution[1];
        }
        if (previousSettings.vsyncCount != UserSettings.vsyncCount)
        {
            if (UserSettings.vsyncCount > 0) UserSettings.targetFrameRate = 0;
            QualitySettings.vSyncCount = UserSettings.vsyncCount;
            previousSettings.vsyncCount = UserSettings.vsyncCount;
        }
        if (previousSettings.targetFrameRate != UserSettings.targetFrameRate)
        {
            Application.targetFrameRate = UserSettings.targetFrameRate;
            previousSettings.targetFrameRate = UserSettings.targetFrameRate;
        }
        if (previousSettings.masterVolume != UserSettings.masterVolume) previousSettings.musicVolume = UserSettings.musicVolume;
        if (previousSettings.musicVolume != UserSettings.musicVolume) previousSettings.musicVolume = UserSettings.musicVolume;
        if (previousSettings.soundVolume != UserSettings.soundVolume) previousSettings.soundVolume = UserSettings.soundVolume;
    }

    void OnApplicationQuit() => SaveSettings();

    public void SaveSettings()
    {
        string json = JsonUtility.ToJson(UserSettings, prettyPrint: true);
        File.WriteAllText(path, json);
    }

    void LoadSettings()
    {
        string json = File.ReadAllText(path);
        UserSettings = JsonUtility.FromJson<UserSettings>(json);
    }
}
