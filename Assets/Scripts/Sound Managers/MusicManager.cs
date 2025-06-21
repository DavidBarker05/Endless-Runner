using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance { get; private set; }

    [SerializeField]
    List<AudioClip> musicTracks = new List<AudioClip>();

    AudioSource audioSource;

    void Awake()
    {
        if (Instance != null && Instance != this) Destroy(gameObject);
        else Instance = this;
        audioSource = GetComponent<AudioSource>();
        PlayMusic();
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
