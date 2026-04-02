using UnityEngine;
using UnityEngine.Video;

public class VideoToTimeline : MonoBehaviour
{
    [Header("片头视频播放器")]
    public VideoPlayer videoPlayer;

    [Header("片头视频结束后隐藏的 Canvas（可选）")]
    public GameObject videoCanvas;

    [Header("片头期间锁定移动（可选）")]
    public LocomotionController locomotionController;

    private bool hasTriggered = false;

    private void OnEnable()
    {
        if (videoPlayer != null)
            videoPlayer.loopPointReached += OnVideoFinished;
    }

    private void OnDisable()
    {
        if (videoPlayer != null)
            videoPlayer.loopPointReached -= OnVideoFinished;
    }

    private void Start()
    {
        // ✅ 片头开始：锁移动
        if (locomotionController != null)
            locomotionController.DisableLocomotion();

        // 播放视频
        if (videoPlayer != null)
            videoPlayer.Play();
        else
            Debug.LogWarning("[VideoToTimeline] videoPlayer is not assigned.");
    }

    private void OnVideoFinished(VideoPlayer vp)
    {
        if (hasTriggered) return;
        hasTriggered = true;

        // 隐藏片头 Canvas
        if (videoCanvas != null)
            videoCanvas.SetActive(false);

        // ✅ 片头结束：恢复移动
        if (locomotionController != null)
            locomotionController.EnableLocomotion();

        // 开始主流程
        if (GameManager.Instance != null)
            GameManager.Instance.BeginGame();
        else
            Debug.LogWarning("[VideoToTimeline] GameManager.Instance is null.");
    }
}