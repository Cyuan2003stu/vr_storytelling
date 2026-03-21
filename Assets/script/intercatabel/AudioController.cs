using UnityEngine;

public class AudioController : MonoBehaviour
{
    [Header("“Ù∆µ…Ë÷√")]
    public AudioSource audioSource;

    public void Play()
    {
        if (audioSource != null)
        {
            audioSource.Play();
            Debug.Log($"[AudioController] ≤•∑≈“Ù∆µ: {audioSource.clip?.name}");
        }
    }

    public void Stop()
    {
        if (audioSource != null)
        {
            audioSource.Stop();
            Debug.Log("[AudioController] Õ£÷π“Ù∆µ");
        }
    }

    public void Pause()
    {
        if (audioSource != null)
        {
            audioSource.Pause();
            Debug.Log("[AudioController] ‘›Õ£“Ù∆µ");
        }
    }

    public void SetClipAndPlay(AudioClip clip)
    {
        if (audioSource != null)
        {
            audioSource.clip = clip;
            audioSource.Play();
            Debug.Log($"[AudioController] ≤•∑≈–¬“Ù∆µ: {clip?.name}");
        }
    }
}