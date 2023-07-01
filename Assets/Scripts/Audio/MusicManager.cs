using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class MusicManager : MonoBehaviour
{
    public Sound[] _sound;
    public static MusicManager instance;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);

        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        foreach (Sound s in _sound)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.Audio_clip;

            s.source.volume = s.Volume;
            s.source.pitch = s.Pitch;
            s.source.loop = s.Loop;
        }
    }

    public void PlaySound(string name)
    {
        Sound s = Array.Find(_sound, sound => sound.name == name);

        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " tidak ditemukan");
            return;
        }

        s.source.Play();
    }
    public void StopSound(string name)
    {
        Sound s = Array.Find(_sound, sound => sound.name == name);

        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " tidak ditemukan");
            return;
        }

        s.source.Stop();
    }
}