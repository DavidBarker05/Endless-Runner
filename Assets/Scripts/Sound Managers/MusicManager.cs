using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[RequireComponent(typeof(AudioSource))]
public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance { get; private set; }

    [SerializeField]
    AudioMixer audioMixer;
    [SerializeField]
    float normalCuttof = 22000f;
    [SerializeField]
    float muffledCuttof = 5000f;
    [SerializeField]
    [Range(0f, 1f)]
    float firstMusicVolume;
    [SerializeField]
    List<AudioClip> musicTracks = new List<AudioClip>();

    AudioSource audioSource;

    public bool MusicMuffled { set => audioMixer.SetFloat("MusicMuffle", value ? muffledCuttof : normalCuttof); }

    void Awake()
    {
        if (Instance != null && Instance != this) Destroy(gameObject);
        else Instance = this;
        audioSource = GetComponent<AudioSource>();
        PlayMusic(0, firstMusicVolume);
    }

    void Update()
    {
        if (GameManager.Instance == null) return;
        MusicMuffled = GameManager.Instance.State == GameManager.GameState.Paused;
        if (GameManager.Instance.State == GameManager.GameState.Alive && audioSource.clip != musicTracks[0]) PlayMusic(0, firstMusicVolume);
        if (GameManager.Instance.State == GameManager.GameState.Dead && audioSource.clip != musicTracks[1]) PlayMusic(1, 0.3f);
    }

    public void PlayMusic(int trackNumber = 0, float volume = 1f)
    {
        if (musicTracks.Count == 0) return;
        int track = Mathf.Clamp(trackNumber, 0, musicTracks.Count - 1);
        if (musicTracks[track] == null) return;
        if (audioSource.isPlaying) audioSource.Stop();
        audioSource.clip = musicTracks[track];
        audioSource.volume = Mathf.Clamp01(volume);
        audioSource.Play();
    }
}
