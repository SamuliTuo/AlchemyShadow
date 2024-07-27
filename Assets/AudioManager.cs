
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AudioClips
{
    NONE,
}

public class AudioManager : MonoBehaviour
{
    public List<AudioSource> sources = new List<AudioSource>();
    public List<AudioLibraryClip> audioLibrary = new List<AudioLibraryClip>();

    public void PlayClip(string name)
    {
        foreach (AudioLibraryClip audio in audioLibrary)
        {
            if (audio.name == name)
            {
                var _source = GetAFreeAudioSource();
                _source.clip = audio.clips[Random.Range(0, audio.clips.Count)];
                _source.pitch = audio.pitch + Random.Range(-audio.pitchRandomizationAmount, audio.pitchRandomizationAmount);
                _source.volume = audio.volume;
                _source.Play();
                break;
            }
        }
    }

    AudioSource GetAFreeAudioSource()
    {
        print("tee audio sourcejen pooling, nyt kaikki t‰‰ll‰ k‰ytt‰‰ yht‰ ja samaa");
        return sources[0];
    }
}

[System.Serializable]
public class AudioLibraryClip
{
    public string name;
    public List<AudioClip> clips;
    public float pitch;
    public float pitchRandomizationAmount;
    public float volume;
    
    public AudioLibraryClip (
            string name,
            List<AudioClip> clips, 
            float pitch,
            float pitchRandomizationAmount, 
            float volume)
    {
        this.name = name;
        this.clips = clips;
        this.pitch = pitch;
        this.pitchRandomizationAmount = pitchRandomizationAmount;
        this.volume = volume;
    }
}
