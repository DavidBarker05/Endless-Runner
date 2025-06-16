using System.IO;
using UnityEngine;

public class UserSettingsManager : MonoBehaviour
{
    public static UserSettingsManager Instance { get; private set; }

    string path = "";

    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(gameObject);
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        path = Path.Combine(Application.persistentDataPath, "user_settings.json");
    }

    public void SaveSettings(UserSettings userSettings)
    {
        string json = JsonUtility.ToJson(userSettings, prettyPrint: true);
        File.WriteAllText(path, json);
    }

    public UserSettings LoadSettings()
    {
        if (!File.Exists(path)) return null;
        string json = File.ReadAllText(path);
        return JsonUtility.FromJson<UserSettings>(json);
    }
}
