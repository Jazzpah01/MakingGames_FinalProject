using UnityEngine.Audio;
using System;
using UnityEngine;

public class AudioManager : MonoBehaviour {

    public Sound[] sounds;
    public AudioMixer audioMixer;

    public static AudioManager instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.outputAudioMixerGroup = s.audioMixerGroup;
            s.source.mute = s.mute;
            s.source.loop = false;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
        }
    }

    private void Update()
    {
        //foreach (Sound sound in sounds)
        //{
        //    if (!sound.source.isPlaying && sound.loop)
        //    {
        //        sound.source.clip = sound.loopClip;
        //        sound.source.Play();
        //    }
        //}
    }

    public void ScheduleLoop(Sound s)
    {
        if (s.loop)
        {
            s.source.PlayScheduled(AudioSettings.dspTime + s.loopClip.length);
            s.source.loop = true;
        }
        else
        {
            s.source.Stop();
        }
    }

    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found");
            return;
        }

        s.source.loop = false;
        s.source.Stop();
        s.source.PlayOneShot(s.source.clip);

        if (s.loopClip != null)
        {
            s.loop = true;
            s.source.clip = s.loopClip;
            ScheduleLoop(s);
        } else
        {
            s.loop = false;
        }
    }
    public void Stop(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found");
            return;
        }

        s.loop = false;
        s.source.loop = false;
        s.source.Stop();
    }

    public void SetMusicVolume(float volume)
    {
        audioMixer.SetFloat("MusicVolume", volume);
    }

    public void SetSFXVolume(float volume)
    {
        audioMixer.SetFloat("SFXVolume", volume);
    }

    public void SetMasterVolume(float volume)
    {
        audioMixer.SetFloat("MasterVolume", volume);
    }

    public float GetMusicVolume()
    {
        return GetMixerValue("MusicVolume");
    }

    public float GetSFXVolume()
    {
        return GetMixerValue("SFXVolume");
    }

    public float GetMasterVolume()
    {
        return GetMixerValue("MasterVolume");
    }

    public float GetMixerValue(string name)
    {
        float value;
        bool result = audioMixer.GetFloat(name, out value);
        if (result)
        {
            return value;
        }
        else
        {
            return 0f;
        }
    }
}
