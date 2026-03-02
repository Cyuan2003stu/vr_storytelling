using UnityEngine;
using UnityEngine.Timeline;

[CreateAssetMenu(menuName = "Narrative/Event")]
public class NarrativeEvent : ScriptableObject
{
    [Header("播放内容")]
    public TimelineAsset timeline;

    [Header("完成条件")]
    public CompletionType completionType;

    [Header("交互物体（抓取类）")]
    public string interactableID;

    [Header("区域触发类")]
    public string triggerZoneID;

    private System.Action onComplete;

    public void Begin(System.Action callback)
    {
        onComplete = callback;
        Debug.Log($"[Event] Begin 被调用，完成条件: {completionType}");

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
        }
    }

    void WaitForGrab(string id)
    {
        Debug.Log($"[Event] 收到交互ID: {id}，等待的ID: {interactableID}");
        if (id != interactableID) return;
        GameEvents.OnInteractionComplete -= WaitForGrab;
        InteractableRegistry.SetActive(interactableID, false);
        Debug.Log("[Event] 抓取完成，进入下一个Event");
        onComplete?.Invoke();
    }

    void WaitForZone(string id)
    {
        if (id != triggerZoneID) return;
        GameEvents.OnInteractionComplete -= WaitForZone;
        onComplete?.Invoke();
    }
}

public enum CompletionType
{
    Auto,
    WaitForGrab,
    WaitForZone
}