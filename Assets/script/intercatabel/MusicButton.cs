using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class MusicButton : XRBaseInteractable
{
    [Header("按键设置")]
    public float pressDepth = 0.02f;
    public float pressSpeed = 10f;

    [Header("音频设置")]
    public AudioSource targetAudioSource;

    [Header("是否只允许按钮关闭一次")]
    public bool stopOnlyOnce = true;

    private Vector3 originalPosition;
    private Vector3 pressedPosition;

    private bool isPressed = false;
    private bool isPressing = false;
    private bool hasStopped = false;
    private bool isPlaying = false;

    private void Start()
    {
        originalPosition = transform.localPosition;
        pressedPosition = originalPosition - new Vector3(0, pressDepth, 0);

        isPlaying = targetAudioSource != null && targetAudioSource.isPlaying;
    }

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);

        Debug.Log("[MusicButton] OnSelectEntered 被触发");

        if (isPressing) return;
        isPressing = true;

        isPressed = !isPressed;

        if (targetAudioSource == null)
        {
            Debug.LogWarning("[MusicButton] targetAudioSource 未赋值。");
            return;
        }

        if (stopOnlyOnce && hasStopped)
        {
            Debug.Log("[MusicButton] 已经停止过一次，忽略重复触发。");
            return;
        }

        StopMusic();
        hasStopped = true;
    }

    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        base.OnSelectExited(args);
        isPressing = false;
    }

    private void Update()
    {
        Vector3 targetPos = isPressed ? pressedPosition : originalPosition;
        transform.localPosition = Vector3.Lerp(
            transform.localPosition,
            targetPos,
            Time.deltaTime * pressSpeed
        );
    }

    public void StartMusic()
    {
        if (targetAudioSource == null)
        {
            Debug.LogWarning("[MusicButton] StartMusic 失败，targetAudioSource 未赋值。");
            return;
        }

        targetAudioSource.Play();
        isPlaying = true;
        hasStopped = false;
        Debug.Log("[MusicButton] StartMusic()");
    }

    public void StopMusic()
    {
        if (targetAudioSource == null)
        {
            Debug.LogWarning("[MusicButton] StopMusic 失败，targetAudioSource 未赋值。");
            return;
        }

        targetAudioSource.Stop();
        isPlaying = false;
        Debug.Log("[MusicButton] StopMusic()");
    }
}