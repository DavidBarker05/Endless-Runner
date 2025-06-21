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

    public void PlaySound(AudioClip audioClip, Transform spawn, float volume = 1f)
    {
        AudioSource _soundEffect = Instantiate(soundEffect, spawn.position, Quaternion.identity);
        _soundEffect.clip = audioClip;
        _soundEffect.volume = Mathf.Clamp01(volume);
        _soundEffect.Play();
        StartCoroutine(DestroySound(_soundEffect.gameObject, _soundEffect.clip.length));
    }

    public void PlaySound(AudioClip audioClip, GameObject followSource, float volume = 1f) {
        AudioSource _soundEffect = Instantiate(soundEffect, followSource.transform.position, Quaternion.identity);
        _soundEffect.clip = audioClip;
        _soundEffect.volume = Mathf.Clamp01(volume);
        _soundEffect.Play();
        StartCoroutine(FollowSource(_soundEffect.gameObject, followSource));
        StartCoroutine(DestroySound(_soundEffect.gameObject, _soundEffect.clip.length));
    }

    public int PlayLoopingSound(AudioClip audioClip, Transform spawn, float volume = 1f)
    {
        AudioSource _audioSource = Instantiate(soundEffect, spawn.position, Quaternion.identity);
        _audioSource.clip = audioClip;
        _audioSource.volume = Mathf.Clamp01(volume);
        _audioSource.loop = true;
        _audioSource.Play();
        if (nextSoundId.ContainsKey(audioClip.name)) nextSoundId[audioClip.name]++;
        else nextSoundId.Add(audioClip.name, 0);
        loopingSounds[audioClip.name].Add(nextSoundId[audioClip.name], _audioSource);
        return nextSoundId[audioClip.name];
    }

    public int PlayLoopingSound(AudioClip audioClip, GameObject followSource, float volume = 1f)
    {
        AudioSource _audioSource = Instantiate(soundEffect, followSource.transform.position, Quaternion.identity);
        _audioSource.clip = audioClip;
        _audioSource.volume = Mathf.Clamp01(volume);
        _audioSource.loop = true;
        _audioSource.Play();
        StartCoroutine(FollowSource(_audioSource.gameObject, followSource));
        if (nextSoundId.ContainsKey(audioClip.name)) nextSoundId[audioClip.name]++;
        else nextSoundId.Add(audioClip.name, 0);
        loopingSounds[audioClip.name].Add(nextSoundId[audioClip.name], _audioSource);
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
        while (true)
        {
            while (GameManager.Instance.State == GameManager.GameState.Paused) yield return null;
            soundObject.transform.position = followObject.transform.position;
            yield return new WaitForFixedUpdate();
        }
    }
}
