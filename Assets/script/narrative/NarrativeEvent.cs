using UnityEngine;
using UnityEngine.Timeline;

[CreateAssetMenu(menuName = "Narrative/Event")]
public class NarrativeEvent : ScriptableObject
{
    [Header("播放内容")]
    public TimelineAsset timeline;

    [Header("完成条件")]
    public CompletionType completionType;

    [Header("抓取类")]
    public string interactableID;

    [Header("区域触发类")]
    public string triggerZoneID;

    [Header("放置类")]
    public string placementZoneID;

    private System.Action onComplete;

    public void Begin(System.Action callback)
    {
        onComplete = callback;
        Debug.Log($"[Event] Begin 被调用，完成条件: {completionType}");

        if (completionType == CompletionType.WaitForPlace ||
            completionType == CompletionType.WaitForGrab)
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
                InteractableRegistry.SetActive(placementZoneID, true); // ← 这时才激活
                GameEvents.OnInteractionComplete += WaitForPlace;
                break;
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
                onComplete?.Invoke();
            });
        else
            onComplete?.Invoke();
    }

    void WaitForZone(string id)
    {
        if (id != triggerZoneID) return;
        GameEvents.OnInteractionComplete -= WaitForZone;
        onComplete?.Invoke();
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
                onComplete?.Invoke();
            });
        else
            onComplete?.Invoke();
    }
}

public enum CompletionType
{
    Auto,
    WaitForGrab,
    WaitForZone,
    WaitForPlace
}