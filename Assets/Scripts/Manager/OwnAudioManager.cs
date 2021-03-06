using UnityEngine;
using System;

public class OwnAudioManager : MonoBehaviour
{
    public Sound[] sounds;


    private void Start()
    {      
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            if (s.isMusic)
            {
                s.source.volume = GameManager.musicLevel;
            }
            else
            {
                s.source.volume = GameManager.soundEffectLevel;

            }
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
        Play("BackGround");
    }

    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            return;
        }
        s.source.Play();
    }

    public void Stop(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            return;
        }
        s.source.Stop();
    }

    public bool SeeIfPlaying(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            return false;
        }
        return s.source.isPlaying;
    }

    public void ChangeValues()
    {
        foreach (Sound s in sounds)
        {
            if (s.isMusic)
            {
                s.source.volume = GameManager.musicLevel;
            }
            else
            {
                s.source.volume = GameManager.soundEffectLevel;
            }

        }
    }

}
