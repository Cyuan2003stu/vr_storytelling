using System;
using System.Collections;
using UnityEditor.VersionControl;
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
    }

    public void Play(TimelineAsset timeline, Action onFinish)
    {
        director.playableAsset = timeline;
        director.Play();
        StartCoroutine(WaitForEnd(onFinish));
    }

    IEnumerator WaitForEnd(Action onFinish)
    {
        yield return new WaitUntil(() => director.state != PlayState.Playing);
        onFinish?.Invoke();
    }
}
