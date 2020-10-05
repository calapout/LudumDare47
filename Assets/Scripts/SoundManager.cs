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
    private void Start()
    {
        EventManager.AddEventListener("playSound", PlaySound);
    }
    private void PlaySound(Bytes.Data data)
    {
        PlaySoundData soundData = (PlaySoundData)data;
        float vol = source.volume / 2f;
        //if (soundData.Volume == -1) { vol = source.volume; }

        AudioClip clip = Resources.Load<AudioClip>("Sounds/" + soundData.Name);

        if (clip != null)
            source.PlayOneShot(clip, vol);
        else
            Debug.LogError("Sounds/" + soundData.Name + " doesnt exist!");
    }
}
