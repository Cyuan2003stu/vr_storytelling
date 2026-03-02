using UnityEngine;
using UnityEngine.Timeline;

[CreateAssetMenu(menuName = "Narrative/Event")]
public class NarrativeEvent : ScriptableObject
{
    [Header("播放内容")]
    public TimelineAsset timeline;          // 动画+语音 都放这里

    [Header("完成条件")]
    public CompletionType completionType;   // 选哪种触发方式

    [Header("交互物体（抓取类）")]
    public string interactableID;           // 填物体的ID，如 "letter"

    [Header("区域触发类")]
    public string triggerZoneID;            // 填区域的ID，如 "zone_door"

    private System.Action onComplete;

    public void Begin(System.Action callback)
    {
        onComplete = callback;

        // 1. 先播放Timeline（动画+语音）
        if (timeline != null)
            TimelineManager.Instance.Play(timeline, OnTimelineFinished);
        else
            OnTimelineFinished(); // 没有timeline直接跳到等待
    }

    void OnTimelineFinished()
    {
        // 2. Timeline播完后，根据完成条件决定是否等待玩家
        switch (completionType)
        {
            case CompletionType.Auto:
                // 自动完成，不等玩家
                onComplete?.Invoke();
                break;

            case CompletionType.WaitForGrab:
                // 激活对应物体，等待抓取
                InteractableRegistry.SetActive(interactableID, true);
                GameEvents.OnInteractionComplete += WaitForGrab;
                break;

            case CompletionType.WaitForZone:
                // 激活区域，等待玩家走进去
                InteractableRegistry.SetActive(triggerZoneID, true);
                GameEvents.OnInteractionComplete += WaitForZone;
                break;
        }
    }

    void WaitForGrab(string id)
    {
        if (id != interactableID) return;
        GameEvents.OnInteractionComplete -= WaitForGrab;
        InteractableRegistry.SetActive(interactableID, false);
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
    Auto,           // 播完自动进入下一个
    WaitForGrab,    // 等玩家抓取某物体
    WaitForZone     // 等玩家走进某区域
}