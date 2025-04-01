using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Audio Source")]
    [SerializeField] private AudioSource _sfxSource;
    [SerializeField] private AudioSource _musicSource;

    [Header("SFX Clips")]
    [SerializeField] private AudioClip _hitClip;
    [SerializeField] private AudioClip[] _damageClip;
    [SerializeField] private AudioClip _deathClip;

    [Header("Music")]
    [SerializeField] private AudioClip[] _musics;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        PlayMusic(_musics[0], 0.3f);
    }

    public void PlayPunchSound(float volume = 1f)
    {
        _sfxSource.PlayOneShot(_hitClip, volume);
    }

    public void PlayKickSound(float volume = 1f)
    {
        _sfxSource.PlayOneShot(_hitClip, volume);
    }

    public void PlayDamageSound(float volume = 1f)
    {
        AudioClip randomClip = _damageClip[Random.Range(0, _damageClip.Length)];
        _sfxSource.PlayOneShot(randomClip, volume);
    }

    public void PlayDeathSound(float volume = 1f)
    {
        _sfxSource.PlayOneShot(_deathClip, volume);
    }

    public void PlaySound(AudioClip clip, float volume = 1f)
    {
        _sfxSource.PlayOneShot(clip, volume);
    }

    public void PlayMusic(AudioClip clip, float volume = 0.5f, bool loop = true)
    {
        _musicSource.clip = clip;
        _musicSource.volume = volume;
        _musicSource.loop = loop;
        _musicSource.Play();
    }

    public void StopMusic()
    {
        _musicSource.Stop();
    }
}
