using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class TimelineManager : MonoBehaviour
{
    public static TimelineManager Instance;
    private PlayableDirector director;

    void Awake()
    {
        Instance = this;
        director = GetComponent<PlayableDirector>();

        if (director == null)
            Debug.LogError("TimelineManager: 找不到 PlayableDirector 组件！");
        else
            Debug.Log("TimelineManager: 初始化成功");
    }

    public void Play(TimelineAsset timeline, Action onFinish)
    {
        Debug.Log($"[TimelineManager] 开始播放: {timeline.name}");
        director.playableAsset = timeline;
        director.Play();
        StartCoroutine(WaitForEnd(onFinish));
    }

    IEnumerator WaitForEnd(Action onFinish)
    {
        yield return null;
        Debug.Log($"[Timeline] 开始等待，当前state: {director.state}, duration: {director.duration}");

        float timer = 0f;
        while (true)
        {
            timer += Time.deltaTime;
            Debug.Log($"[Timeline] 等待中... state: {director.state}, time: {director.time:F2}, duration: {director.duration:F2}");

            if (director.state == PlayState.Paused || director.time >= director.duration)
                break;

            // 防止无限等待，超过30秒强制结束
            if (timer > 30f)
            {
                Debug.LogWarning("[Timeline] 超时强制结束");
                break;
            }

            yield return new WaitForSeconds(0.5f); // 每0.5秒打一次日志
        }

        Debug.Log("[Timeline] 播放结束，调用回调");
        onFinish?.Invoke();
    }
}

