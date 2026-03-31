using UnityEngine;

public class SpeakerController : MonoBehaviour
{
	public AudioSource audioSource;

	private bool isOn = false;

	public void ToggleSpeaker()
	{
		isOn = !isOn;

		if (isOn)
		{
			audioSource.Play();
			Debug.Log("音响开启");
		}
		else
		{
			audioSource.Stop();
			Debug.Log("音响关闭");
		}
	}
}