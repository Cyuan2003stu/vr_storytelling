using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class TimelineManager : MonoBehaviour
{
    public static TimelineManager Instance;

    void Awake()
    {
        Instance = this;
    }

    public void Play(TimelineAsset timeline, Action onFinish)
    {
        // 在场景里找所有 PlayableDirector
        PlayableDirector target = null;
        var allDirectors = FindObjectsOfType<PlayableDirector>();

        foreach (var d in allDirectors)
        {
            if (d.playableAsset == timeline)
            {
                target = d;
                break;
            }
        }

        if (target == null)
        {
            Debug.LogError($"[TimelineManager] 找不到对应的Director: {timeline.name}");
            onFinish?.Invoke();
            return;
        }

        Debug.Log($"[TimelineManager] 开始播放: {timeline.name}, duration: {target.duration}");
        target.playOnAwake = false;
        target.Play();
        StartCoroutine(WaitForEnd(target, onFinish));
    }

    IEnumerator WaitForEnd(PlayableDirector director, Action onFinish)
    {
        float duration = Mathf.Max((float)director.duration, 0.1f);
        Debug.Log($"[TimelineManager] 等待 {duration} 秒");
        yield return new WaitForSeconds(duration);
        Debug.Log("[TimelineManager] 播放结束");
        director.Stop();
        onFinish?.Invoke();
    }
}
