using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Radio : MonoBehaviour, IInteractable
{
    [SerializeField] private AudioClip toggleSound;
    [SerializeField] private AudioClip playSound;
    [SerializeField] private bool loop;
    private AudioSource radioSource;
    private bool radioState;
    
    void Start()
    {
        radioSource = GetComponent<AudioSource>();
    }

    private void ToggleRadio()
    {
        radioState = !radioState;
        if (radioState)
        {
            StartCoroutine(PlayRadio());
        }
        else if (!radioState)
        {
            StopCoroutine(PlayRadio());
            PlaySound(toggleSound);
        }
    }
    public void Interact()
    {
        ToggleRadio();
    }

    private void PlaySound(AudioClip clip)
    {
        if (clip != null)
        {
            radioSource.clip = clip;
            radioSource.Play();
        }
    }
    IEnumerator PlayRadio()
    {
        PlaySound(toggleSound);
        yield return new WaitUntil(() => !radioSource.isPlaying);
        PlaySound(playSound);
    }
}
