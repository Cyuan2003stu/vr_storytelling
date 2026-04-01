using UnityEngine;
using UnityEngine.Video;

public class VideoToTimeline : MonoBehaviour
{
    [Header("片头视频播放器")]
    public VideoPlayer videoPlayer;

    [Header("片头视频结束后隐藏的 Canvas（可选）")]
    public GameObject videoCanvas;

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
        if (videoPlayer != null)
            videoPlayer.Play();
    }

    private void OnVideoFinished(VideoPlayer vp)
    {
        if (hasTriggered) return;
        hasTriggered = true;

        if (videoCanvas != null)
            videoCanvas.SetActive(false);

        if (GameManager.Instance != null)
            GameManager.Instance.BeginGame();
        else
            Debug.LogWarning("[VideoToTimeline] GameManager.Instance is null.");
    }
}