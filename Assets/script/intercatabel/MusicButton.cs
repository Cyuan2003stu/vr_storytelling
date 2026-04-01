using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class MusicButton : XRBaseInteractable
{
    [Header("按键设置")]
    public float pressDepth = 0.02f;        // 按下的距离
    public float pressSpeed = 10f;          // 按下和弹起的速度

    [Header("音频设置")]
    public AudioSource audioSource;         // 拖入 AudioSource

    private Vector3 originalPosition;
    private Vector3 pressedPosition;
    private bool isPressed = false;
    private bool isPlaying = true;          // 默认音乐在播放
    private bool isPressing = false;

    void Start()
    {
        originalPosition = transform.localPosition;
        pressedPosition = originalPosition - new Vector3(0, pressDepth, 0);

        // 默认播放音乐
        if (audioSource != null && !audioSource.isPlaying)
            audioSource.Play();
    }

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);
        if (!isPressing)
        {
            isPressing = true;
            isPressed = !isPressed;

            // 切换音乐播放状态
            if (audioSource != null)
            {
                if (isPlaying)
                {
                    audioSource.Pause();
                    isPlaying = false;
                    Debug.Log("[MusicButton] 音乐暂停");
                }
                else
                {
                    audioSource.Play();
                    isPlaying = true;
                    Debug.Log("[MusicButton] 音乐播放");
                }
            }
        }
    }

    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        base.OnSelectExited(args);
        isPressing = false;
    }

    void Update()
    {
        // 按键平滑移动
        Vector3 targetPos = isPressed ? pressedPosition : originalPosition;
        transform.localPosition = Vector3.Lerp(
            transform.localPosition,
            targetPos,
            Time.deltaTime * pressSpeed
        );
    }
}