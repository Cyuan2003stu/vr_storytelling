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

        // 切换资产前先保存旧的绑定
        var oldAsset = director.playableAsset as TimelineAsset;

        director.playableAsset = timeline;

        // 把旧绑定复制到新 Timeline 的同名轨道上
        if (oldAsset != null)
            CopyBindings(oldAsset, timeline);

        director.Play();
        StartCoroutine(WaitForEnd(onFinish));
    }

    void CopyBindings(TimelineAsset oldAsset, TimelineAsset newAsset)
    {
        // 遍历旧 Timeline 的所有轨道
        foreach (var oldTrack in oldAsset.GetOutputTracks())
        {
            // 在新 Timeline 里找同名轨道
            foreach (var newTrack in newAsset.GetOutputTracks())
            {
                if (oldTrack.name == newTrack.name)
                {
                    // 把旧轨道绑定的物体复制给新轨道
                    var binding = director.GetGenericBinding(oldTrack);
                    if (binding != null)
                    {
                        director.SetGenericBinding(newTrack, binding);
                        Debug.Log($"[Binding] 复制绑定: {newTrack.name} → {binding}");
                    }
                }
            }
        }
    }

    IEnumerator WaitForEnd(Action onFinish)
    {
        yield return null;

        float lastTime = -1f;
        float stuckTimer = 0f;

        while (true)
        {
            if (director.state == PlayState.Paused ||
                director.time >= director.duration)
                break;

            if (Mathf.Approximately((float)director.time, lastTime))
            {
                stuckTimer += 0.5f;
                if (stuckTimer >= 2f)
                {
                    Debug.LogWarning("[Timeline] 播放卡住，强制结束");
                    break;
                }
            }
            else
            {
                stuckTimer = 0f;
                lastTime = (float)director.time;
            }

            yield return new WaitForSeconds(0.5f);
        }

        Debug.Log("[Timeline] 播放结束");
        onFinish?.Invoke();
    }
}