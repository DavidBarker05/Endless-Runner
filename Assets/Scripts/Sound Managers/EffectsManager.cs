using System.Collections.Generic;
using UnityEngine;

public class EffectsManager : MonoBehaviour
{
    public static EffectsManager Instance { get; private set; }

    [SerializeField]
    AudioSource soundEffect;

    readonly Dictionary<string, int> nextSoundId = new Dictionary<string, int>();
    readonly Dictionary<string, Dictionary<int, AudioSource>> loopingSounds = new Dictionary<string, Dictionary<int, AudioSource>>();

    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(gameObject);
        else Instance = this;
    }

    AudioSource CreateAudioSource(AudioClip audioClip, Transform spawn, float volume = 1f, bool looping = false)
    {
        AudioSource _soundEffect = Instantiate(soundEffect, spawn.position, Quaternion.identity);
        _soundEffect.clip = audioClip;
        _soundEffect.volume = Mathf.Clamp01(volume);
        _soundEffect.loop = looping;
        _soundEffect.Play();
        StartCoroutine(ListenForPause(_soundEffect));
        return _soundEffect;
    }

    public void PlaySound(AudioClip audioClip, Transform spawn, float volume = 1f)
    {
        AudioSource _soundEffect = CreateAudioSource(audioClip, spawn, volume);
        StartCoroutine(DestroySound(_soundEffect.gameObject, _soundEffect.clip.length));
    }

    public void PlaySound(AudioClip audioClip, GameObject followSource, float volume = 1f) {
        AudioSource _soundEffect = CreateAudioSource(audioClip, followSource.transform, volume);
        StartCoroutine(FollowSource(_soundEffect.gameObject, followSource));
        StartCoroutine(DestroySound(_soundEffect.gameObject, _soundEffect.clip.length));
    }

    public int PlayLoopingSound(AudioClip audioClip, Transform spawn, float volume = 1f)
    {
        AudioSource _soundEffect = CreateAudioSource(audioClip, spawn, volume, looping: true);
        if (nextSoundId.ContainsKey(audioClip.name)) nextSoundId[audioClip.name]++;
        else
        {
            nextSoundId.Add(audioClip.name, 0);
            loopingSounds.Add(audioClip.name, new Dictionary<int, AudioSource>());
        }
        loopingSounds[audioClip.name].Add(nextSoundId[audioClip.name], _soundEffect);
        return nextSoundId[audioClip.name];
    }

    public int PlayLoopingSound(AudioClip audioClip, GameObject followSource, float volume = 1f)
    {
        AudioSource _soundEffect = CreateAudioSource(audioClip, followSource.transform, volume, looping: true);
        StartCoroutine(FollowSource(_soundEffect.gameObject, followSource));
        if (nextSoundId.ContainsKey(audioClip.name)) nextSoundId[audioClip.name]++;
        else
        {
            nextSoundId.Add(audioClip.name, 0);
            loopingSounds.Add(audioClip.name, new Dictionary<int, AudioSource>());
        }
        loopingSounds[audioClip.name].Add(nextSoundId[audioClip.name], _soundEffect);
        return nextSoundId[audioClip.name];
    }

    public void StopLoopingSound(AudioClip audioClip, int id)
    {
        if (!loopingSounds.ContainsKey(audioClip.name)) return;
        if (!loopingSounds[audioClip.name].ContainsKey(id)) return;
        AudioSource _soundEffect = loopingSounds[audioClip.name][id];
        loopingSounds[audioClip.name].Remove(id);
        if (loopingSounds[audioClip.name].Count == 0)
        {
            nextSoundId.Remove(audioClip.name);
            loopingSounds.Remove(audioClip.name);
        }
        Destroy(_soundEffect.gameObject);
    }

    System.Collections.IEnumerator ListenForPause(AudioSource audioSource)
    {
        while (audioSource != null)
        {
            while (GameManager.Instance.State == GameManager.GameState.Paused)
            {
                if (audioSource.isPlaying) audioSource.Pause();
                yield return null;
            }
            if (!audioSource.isPlaying) audioSource.UnPause();
            yield return new WaitForFixedUpdate();
        }
    }

    System.Collections.IEnumerator DestroySound(GameObject soundObject, float clipLength)
    {
        float _timer = 0f;
        while (_timer < clipLength)
        {
            while (GameManager.Instance.State == GameManager.GameState.Paused) yield return null;
            _timer += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }
        Destroy(soundObject);
    }

    System.Collections.IEnumerator FollowSource(GameObject soundObject, GameObject followObject)
    {
        while (soundObject != null && followObject != null)
        {
            while (GameManager.Instance.State == GameManager.GameState.Paused) yield return null;
            soundObject.transform.position = followObject.transform.position;
            yield return new WaitForFixedUpdate();
        }
    }
}
