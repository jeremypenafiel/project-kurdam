using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioSource musicPlayer;
    [SerializeField] AudioSource sfxPlayer;

    public static AudioManager i { get; private set; }


    private void Awake()
    {
        i = this; 
    }

    public void PlayMusic(AudioClip clip, bool loop = true)
    {
        if (clip == null) return;

        musicPlayer.clip = clip;
        sfxPlayer.loop = loop;
        musicPlayer.Play();
    }

}


