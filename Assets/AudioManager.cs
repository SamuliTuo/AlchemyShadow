
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

    private void Start()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            sources.Add(transform.GetChild(i).GetComponent<AudioSource>());
            transform.GetChild(i).gameObject.SetActive(false);
        }
    }



    public void PlayClip(string name)
    {
        foreach (AudioLibraryClip audio in audioLibrary)
        {
            if (audio.name == name)
            {
                var _source = GetAFreeAudioSource();
                if (_source == null)
                {
                    return;
                }
                _source.clip = audio.clips[Random.Range(0, audio.clips.Count)];
                _source.pitch = audio.pitch + Random.Range(-audio.pitchRandomizationAmount, audio.pitchRandomizationAmount);
                _source.volume = audio.volume;
                _source.Play();
                StartCoroutine(PauseAudioSourceAfterPlaying(_source));
                break;
            }
        }
    }

    AudioSource GetAFreeAudioSource()
    {
        foreach (AudioSource source in sources)
        {
            if (source.isActiveAndEnabled == false)
            {
                source.gameObject.SetActive(true);
                return source;
            }
        }
        return null;
    }

    IEnumerator PauseAudioSourceAfterPlaying(AudioSource source)
    {
        while (source.isPlaying)
        {
            yield return null;
        }
        source.gameObject.SetActive(false);
    }
}

[System.Serializable]
public class AudioLibraryClip
{
    public string name;
    public List<AudioClip> clips;
    public float pitch = 1;
    public float pitchRandomizationAmount = 0;
    public float volume = 1;
    
    public AudioLibraryClip (
            string name,
            List<AudioClip> clips,
            float pitch = 1,
            float pitchRandomizationAmount = 0, 
            float volume = 1)
    {
        this.name = name;
        this.clips = clips;
        this.pitch = pitch;
        this.pitchRandomizationAmount = pitchRandomizationAmount;
        this.volume = volume;
    }
}
