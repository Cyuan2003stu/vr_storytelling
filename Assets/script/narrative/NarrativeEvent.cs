using UnityEngine;
using UnityEngine.Timeline;

[CreateAssetMenu(menuName = "Narrative/Event")]
public class NarrativeEvent : ScriptableObject
{
    [Header("播放内容")]
    public TimelineAsset timeline;

    [Header("完成条件")]
    public CompletionType completionType;

    [Header("自动播放下一个")]
    public bool autoPlayNext = false;  // ← 勾选后上一个播完自动播下一个

    [Header("抓取类")]
    public string interactableID;

    [Header("区域触发类")]
    public string triggerZoneID;

    [Header("放置类")]
    public string placementZoneID;

    [Header("自定义触发")]
    public string customTriggerID;

    private System.Action onComplete;

    public void Begin(System.Action callback)
    {
        onComplete = callback;
        Debug.Log($"[Event] Begin 被调用，完成条件: {completionType}");

        if (completionType == CompletionType.WaitForPlace ||
            completionType == CompletionType.WaitForGrab ||
            completionType == CompletionType.WaitForCustomTrigger ||
            completionType == CompletionType.WaitForZone)
        {
            OnTimelineFinished();
            return;
        }

        if (timeline != null)
            TimelineManager.Instance.Play(timeline, OnTimelineFinished);
        else
            OnTimelineFinished();
    }

    void OnTimelineFinished()
    {
        Debug.Log($"[Event] OnTimelineFinished，准备进入等待: {completionType}");

        switch (completionType)
        {
            case CompletionType.Auto:
                Debug.Log("[Event] Auto完成");
                onComplete?.Invoke();
                break;

            case CompletionType.WaitForGrab:
                Debug.Log($"[Event] 开始等待抓取ID: {interactableID}");
                InteractableRegistry.SetActive(interactableID, true);
                GameEvents.OnInteractionComplete += WaitForGrab;
                break;

            case CompletionType.WaitForZone:
                Debug.Log($"[Event] 开始等待区域ID: {triggerZoneID}");
                InteractableRegistry.SetActive(triggerZoneID, true);
                GameEvents.OnInteractionComplete += WaitForZone;
                break;

            case CompletionType.WaitForPlace:
                Debug.Log($"[Event] 开始等待放置到区域ID: {placementZoneID}");
                InteractableRegistry.SetActive(placementZoneID, true);
                GameEvents.OnInteractionComplete += WaitForPlace;
                break;

            case CompletionType.WaitForCustomTrigger:
                Debug.Log($"[Event] 开始等待自定义触发ID: {customTriggerID}");
                GameEvents.OnInteractionComplete += WaitForCustomTrigger;
                break;
        }
    }

    // 统一的 Timeline 播放完成处理
    void OnActionComplete()
    {
        if (autoPlayNext)
        {
            // 勾选了自动播放，直接完成进入下一个
            Debug.Log("[Event] 自动播放下一个Event");
            onComplete?.Invoke();
        }
        else
        {
            // 没勾选，正常完成
            onComplete?.Invoke();
        }
    }

    void WaitForGrab(string id)
    {
        Debug.Log($"[Event] 收到交互ID: {id}，等待的ID: {interactableID}");
        if (id != interactableID) return;
        GameEvents.OnInteractionComplete -= WaitForGrab;
        Debug.Log("[Event] 抓取完成，开始播放Timeline");

        if (timeline != null)
            TimelineManager.Instance.Play(timeline, () =>
            {
                Debug.Log("[Event] Timeline播放完成，进入下一个Event");
                OnActionComplete();
            });
        else
            OnActionComplete();
    }

    void WaitForZone(string id)
    {
        if (id != triggerZoneID) return;
        GameEvents.OnInteractionComplete -= WaitForZone;
        Debug.Log("[Event] 区域触发完成，开始播放Timeline");

        if (timeline != null)
            TimelineManager.Instance.Play(timeline, () =>
            {
                Debug.Log("[Event] Timeline播放完成，进入下一个Event");
                OnActionComplete();
            });
        else
            OnActionComplete();
    }

    void WaitForPlace(string id)
    {
        Debug.Log($"[Event] 收到放置ID: {id}，等待的ID: {placementZoneID}");
        if (id != placementZoneID) return;
        GameEvents.OnInteractionComplete -= WaitForPlace;
        Debug.Log("[Event] 放置完成，开始播放Timeline");

        if (timeline != null)
            TimelineManager.Instance.Play(timeline, () =>
            {
                Debug.Log("[Event] Timeline播放完成，进入下一个Event");
                OnActionComplete();
            });
        else
            OnActionComplete();
    }

    void WaitForCustomTrigger(string id)
    {
        if (id != customTriggerID) return;
        GameEvents.OnInteractionComplete -= WaitForCustomTrigger;
        Debug.Log("[Event] 自定义触发完成，开始播放Timeline");

        if (timeline != null)
            TimelineManager.Instance.Play(timeline, () =>
            {
                Debug.Log("[Event] Timeline播放完成，进入下一个Event");
                OnActionComplete();
            });
        else
            OnActionComplete();
    }
}

public enum CompletionType
{
    Auto,
    WaitForGrab,
    WaitForZone,
    WaitForPlace,
    WaitForCustomTrigger
}