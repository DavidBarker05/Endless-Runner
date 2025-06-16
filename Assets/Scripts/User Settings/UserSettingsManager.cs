using System.IO;
using UnityEngine;

public class UserSettingsManager : MonoBehaviour
{
    public static UserSettingsManager Instance { get; private set; }

    string path = "";

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
