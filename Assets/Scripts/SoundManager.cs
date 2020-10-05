using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Bytes;

public class PlaySoundData : Bytes.Data
{
    public PlaySoundData(string name, float vol = -1f) { Name = name; Volume = vol; }
    public string Name { get; private set; }
    public float Volume { get; private set; }
}

public class SoundManager : MonoBehaviour
{
    public AudioSource source;
    public AudioSource musicSource;
    public AudioSource ambient;

    public AudioClip normalMusic;
    public AudioClip normalAmbient;

    public AudioClip bossMusic;
    public AudioClip bossAmbient;

    private void Start()
    {
        EventManager.AddEventListener("playSound", PlaySound);
        EventManager.AddEventListener("musicNormal", MusicNormal);
        EventManager.AddEventListener("musicBossAppear", MusicBossAppear);
    }
    private void MusicNormal(Bytes.Data data)
    {
        musicSource.Stop();
        ambient.Stop();

        musicSource.clip = normalMusic;
        ambient.clip = normalAmbient;

        musicSource.Play();
        ambient.Play();
    }
    private void MusicBossAppear(Bytes.Data data)
    {
        musicSource.Stop();
        ambient.Stop();

        musicSource.clip = bossMusic;
        ambient.clip = bossAmbient;

        musicSource.Play();
        ambient.Play();
    }
    private void PlaySound(Bytes.Data data)
    {
        PlaySoundData soundData = (PlaySoundData)data;
        float vol = source.volume / 2f;
        //if (soundData.Volume == -1) { vol = source.volume; }

        AudioClip clip = Resources.Load<AudioClip>("Sounds/" + soundData.Name);

        if (clip != null)
            source.PlayOneShot(clip, vol);
        //else
            //Debug.LogError("Sounds/" + soundData.Name + " doesnt exist!");
    }
}
