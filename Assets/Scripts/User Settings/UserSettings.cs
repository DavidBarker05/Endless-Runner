[System.Serializable]
public class UserSettings
{
    readonly public int[] resolution = new int[2];
    public int vsyncCount;
    public int targetFrameRate = 0;
    public float masterVolume = 1f;
    public float musicVolume = 1f;
    public float effectsVolume = 1f;

    public UserSettings()
    {
        resolution = new int[2] { 1920, 1080 };
        vsyncCount = 1;
        targetFrameRate = 0;
        masterVolume = 1f;
        musicVolume = 1f;
        effectsVolume = 1f;
    }

    public UserSettings(UserSettings other)
    {
        resolution = new int[2] { other.resolution[0], other.resolution[1] };
        vsyncCount = other.vsyncCount;
        targetFrameRate = other.targetFrameRate;
        masterVolume = other.masterVolume;
        musicVolume = other.musicVolume;
        effectsVolume = other.effectsVolume;
    }
}
