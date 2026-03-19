using UnityEngine;

public class TelephoneController : MonoBehaviour
{
    [Header("µÁª∞¡Â…˘“Ù‘¥")]
    public AudioSource ringAudio;

    public void StartRinging()
    {
        if (ringAudio == null) return;

        ringAudio.loop = true;

        if (!ringAudio.isPlaying)
        {
            ringAudio.Play();
        }
    }

    public void StopRinging()
    {
        if (ringAudio == null) return;

        if (ringAudio.isPlaying)
        {
            ringAudio.Stop();
        }
    }
}