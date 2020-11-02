using UnityEngine.Audio;
using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;


    public Sound[] sounds;
    public float volumeGlobal; 

    void Awake(){
        DontDestroyOnLoad(gameObject);

        if(instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }


        volumeGlobal = PlayerPrefs.GetFloat("Volume");

        foreach (Sound s in sounds){
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume * volumeGlobal;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
    }

    public void Play(string name){
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if(s == null)
            return;
        s.source.Play();
    }

    public void SetVolume(float _volume)
    {
        volumeGlobal = _volume;
        foreach (Sound s in sounds)
        {
            s.source.volume = s.volume * volumeGlobal;
        }
    }
}
