using UnityEngine.Audio;
using UnityEngine;
[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;
    public AudioMixerGroup audioMixerGroup;
    public bool mute;
    public bool loop;
    [Range(0f,1f)]
    public float volume;
    [Range(-3f, 3f)]
    public float pitch;
    [HideInInspector]
    public AudioSource source;
}
