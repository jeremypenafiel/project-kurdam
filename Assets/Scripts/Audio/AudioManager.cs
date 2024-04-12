using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] List<AudioData> sfxList;
    [SerializeField] AudioSource musicPlayer;
    [SerializeField] AudioSource sfxPlayer;
    [SerializeField] AudioSource ambiencePlayer;

    [SerializeField] float fadeDuration;

    float originalMusicVolume;
    private const float ambienceVolumeMax = 1f;
    Dictionary<AudioId, AudioData> sfxLookup;
    [SerializeField] private AudioClip creepyMusic;

    public static AudioManager i { get; private set; }


    private void Awake()
    {
        i = this; 
    }

    private void Start()
    {
        i = this;
        originalMusicVolume = musicPlayer.volume;
        ambiencePlayer.volume = ambienceVolumeMax;
        sfxLookup = sfxList.ToDictionary(x => x.id);
    }

    public void PlaySFX(AudioClip clip)
    {
        if (clip == null) return;
        sfxPlayer.PlayOneShot(clip);
    }

    public void PlaySFX(AudioId audioId)
    {
        if (!sfxLookup.ContainsKey(audioId)) return;

        var audioData = sfxLookup[audioId];

        PlaySFX(audioData.clip);
    }

    public void PlayMusic(AudioClip clip, bool loop = true, bool fade=false)
    {
        if (clip == null) return;

        StartCoroutine(PlayMusicAsync(clip, loop, fade));
        
    }

    IEnumerator PlayMusicAsync(AudioClip clip, bool loop, bool fade)
    {
        if (fade)
            yield return musicPlayer.DOFade(0, fadeDuration).WaitForCompletion();

        musicPlayer.clip = clip;
        sfxPlayer.loop = loop;
        musicPlayer.Play();

        if (fade)
            yield return musicPlayer.DOFade(originalMusicVolume, fadeDuration).WaitForCompletion();
    }

    public void PlayAmbientSound(AudioClip clip, bool fade)
    {
        if (clip == null) 
            clip = creepyMusic;

        StartCoroutine(PlayAmbientSoundAsync(clip, fade));

    }

    public void StopPlayAmbientSound(bool fade = true)
    {
        StartCoroutine(StopPlayAmbientSoundAsync(fade));
    }

    IEnumerator PlayAmbientSoundAsync(AudioClip clip, bool fade)
    {
        if (fade)
            yield return ambiencePlayer.DOFade(0, fadeDuration).WaitForCompletion();

        ambiencePlayer.clip = clip;
        ambiencePlayer.Play();

        if (fade)
            yield return ambiencePlayer.DOFade(ambienceVolumeMax, fadeDuration).WaitForCompletion();
    }
    
    IEnumerator StopPlayAmbientSoundAsync(bool fade = true)
    {
        if (fade)
            yield return ambiencePlayer.DOFade(0, fadeDuration).WaitForCompletion();
        ambiencePlayer.Stop();
    }
    
    public void SetAmbientVolume(float volume)
    {
        /*StartCoroutine(SetAmbientVolumeAsync(volume));*/
        ambiencePlayer.volume = volume*ambienceVolumeMax;
    }
    
    IEnumerator SetAmbientVolumeAsync(float volume)
    {
        Debug.Log(volume*ambienceVolumeMax);
        yield return ambiencePlayer.DOFade(volume*ambienceVolumeMax, fadeDuration).WaitForCompletion();
    }
    

}

public enum AudioId { UISelect, Hit, Faint, ExpGain }


[System.Serializable]
public class AudioData
{
    public AudioId id;
    public AudioClip clip;
}

