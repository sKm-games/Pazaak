using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager_script : MonoBehaviour
{
    private AudioSource _musicSource, _sfxSource;
    
    void Start()
    {
        _musicSource = GetComponents<AudioSource>()[0];
        _sfxSource = GetComponents<AudioSource>()[1];
    }

    public void SetMusicVolume(float v)
    {
        _musicSource.volume = v;
    }

    public void SetSFXVolume(float v)
    {
        _sfxSource.volume = v;
    }

    public void PlayEffet(AudioClip ac, float v = 0.5f)
    {
        float r = Random.Range(0.8f, 1.2f);
        _sfxSource.pitch = r;
        _sfxSource.PlayOneShot(ac,v);
    }

    public void PlayEffetDelay(AudioClip ac, float delay, float v = 0.5f)
    {
        float r = Random.Range(0.8f, 1.2f);
        _sfxSource.pitch = r;
        _sfxSource.clip = ac;
        _sfxSource.volume = v;
        _sfxSource.PlayDelayed(delay);

    }
}
