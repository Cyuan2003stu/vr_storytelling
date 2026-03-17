using UnityEngine;
using System.Collections.Generic;

public class CalendarManager : MonoBehaviour
{
    public static CalendarManager Instance;

    [Header("所有槽位ID")]
    public List<string> allSlotIDs;

    [Header("完成后触发的ID")]
    public string completeTriggerID = "calendar_complete";

    private List<string> completedSlots = new List<string>();

    void Awake() => Instance = this;

    void OnEnable()
    {
        GameEvents.OnInteractionComplete += OnSlotFilled;
    }

    void OnDisable()
    {
        GameEvents.OnInteractionComplete -= OnSlotFilled;
    }

    void OnSlotFilled(string id)
    {
        if (!allSlotIDs.Contains(id)) return;
        if (completedSlots.Contains(id)) return;

        completedSlots.Add(id);
        Debug.Log($"[Calendar] 完成 {completedSlots.Count}/{allSlotIDs.Count}");

        if (completedSlots.Count >= allSlotIDs.Count)
        {
            Debug.Log("[Calendar] 所有碎片拼完，触发CustomTrigger");
            CustomTrigger.FireID(completeTriggerID);
        }
    }

    public void Reset()
    {
        completedSlots.Clear();
    }
}