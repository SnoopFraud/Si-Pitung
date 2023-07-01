using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip Audio_clip;
    
    [Range(0f, 1f)]
    public float Volume;
    
    [Range(.1f, 3f)]
    public float Pitch;

    [HideInInspector] public AudioSource source;

    public bool Loop;
}