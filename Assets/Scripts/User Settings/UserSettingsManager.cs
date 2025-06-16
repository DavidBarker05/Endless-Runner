using System.IO;
using UnityEngine;

public class UserSettingsManager : MonoBehaviour
{
    public static UserSettingsManager Instance { get; private set; }

    string path = "";
    UserSettings previousSettings;

    [SerializeField]
    UserSettings _userSettings;
    public UserSettings UserSettings { get => _userSettings; set => _userSettings = value; }

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
        previousSettings = new UserSettings();
        previousSettings.screenWidth = UserSettings.screenWidth;
        previousSettings.screenHeight = UserSettings.screenHeight;
        previousSettings.vSyncMode = UserSettings.vSyncMode;
        previousSettings.frameRateLimit = UserSettings.frameRateLimit;
        previousSettings.musicVolume = UserSettings.musicVolume;
        previousSettings.soundVolume = UserSettings.soundVolume;
    }

    void Update()
    {
        if (previousSettings.screenWidth != UserSettings.screenWidth || previousSettings.screenHeight != UserSettings.screenHeight)
        {
            Screen.SetResolution(UserSettings.screenWidth, UserSettings.screenHeight, fullscreen: true);
            previousSettings.screenWidth = UserSettings.screenWidth;
            previousSettings.screenHeight = UserSettings.screenHeight;
        }
        if (previousSettings.vSyncMode != UserSettings.vSyncMode)
        {
            QualitySettings.vSyncCount = UserSettings.vSyncMode;
            previousSettings.vSyncMode = UserSettings.vSyncMode;
            if (UserSettings.vSyncMode > 0) UserSettings.frameRateLimit = 0;
        }
        if (previousSettings.frameRateLimit != UserSettings.frameRateLimit)
        {
            Application.targetFrameRate = UserSettings.frameRateLimit;
            previousSettings.frameRateLimit = UserSettings.frameRateLimit;
        }
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
