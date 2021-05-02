using System.Collections;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource effectsSource;
    public AudioSource musicSource;

    public static AudioManager instance = null;
    void Awake()
    {
        if (instance == null)
            instance = this;
        
        else if (instance != this)
            Destroy(gameObject);
        
        DontDestroyOnLoad(gameObject);
    }

    public void Play(AudioClip clip)
    {
        if (clip == null) return;
        effectsSource.clip = clip;
        effectsSource.Play();
    }
    public void PlayMusic(AudioClip clip, bool fade)
    {
        if (clip == null) return;
        musicSource.clip = clip;
        if (fade)
        {
            float origVolume = musicSource.volume;
            musicSource.volume = 0;
            StartCoroutine(AudioFade(musicSource, origVolume, 0.05f, 0.5f));

        }
        musicSource.Play();
    }

    IEnumerator AudioFade(AudioSource source, float targetVolume, float step, float time)
    {
        for (float i = 0; i < targetVolume; i+=step)
        {
            source.volume = i;
            yield return new WaitForSeconds(time);
        }
        
    }
}
