using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem;

public class CalendarManager : MonoBehaviour
{
    public static CalendarManager Instance;

    [Header("所有槽位ID")]
    public List<string> allSlotIDs;

    [Header("完成后触发的ID")]
    public string completeTriggerID = "calendar_complete";

    [Header("跳过设置")]
    public bool enableSkip = true;
    public string skipTriggerID = "";

    [Header("OpenXR 输入")]
    public InputActionReference primaryButtonAction;
    public InputActionReference gripAction;

    private List<string> completedSlots = new List<string>();
    private bool skipCooldown = false;

    void Awake() => Instance = this;

    void OnEnable()
    {
        GameEvents.OnInteractionComplete += OnSlotFilled;

        if (primaryButtonAction != null)
        {
            primaryButtonAction.action.actionMap?.Enable();
            primaryButtonAction.action.Enable();
            primaryButtonAction.action.performed += OnPrimaryButton;
            Debug.Log("[CalendarManager] A键监听已注册");
        }
        else
        {
            Debug.LogWarning("[CalendarManager] primaryButtonAction 未赋值");
        }

        if (gripAction != null)
        {
            gripAction.action.actionMap?.Enable();
            gripAction.action.Enable();
            gripAction.action.performed += OnGrip;
            Debug.Log("[CalendarManager] 握拳监听已注册");
        }
        else
        {
            Debug.LogWarning("[CalendarManager] gripAction 未赋值");
        }
    }

    void OnDisable()
    {
        GameEvents.OnInteractionComplete -= OnSlotFilled;

        if (primaryButtonAction != null)
        {
            primaryButtonAction.action.performed -= OnPrimaryButton;
            primaryButtonAction.action.Disable();
        }

        if (gripAction != null)
        {
            gripAction.action.performed -= OnGrip;
            gripAction.action.Disable();
        }
    }

    void OnPrimaryButton(InputAction.CallbackContext ctx)
    {
        Debug.Log($"[CalendarManager] 收到A键输入，enableSkip: {enableSkip}");
        if (!enableSkip || skipCooldown) return;
        Debug.Log("[CalendarManager] A键跳过");
        Skip();
    }

    void OnGrip(InputAction.CallbackContext ctx)
    {
        float value = ctx.ReadValue<float>();
        Debug.Log($"[CalendarManager] 收到握拳输入，值: {value}，enableSkip: {enableSkip}");
        if (!enableSkip || skipCooldown) return;
        if (value >= 0.9f)
        {
            Debug.Log("[CalendarManager] 握拳跳过");
            Skip();
        }
    }

    void Skip()
    {
        enableSkip = false;
        skipCooldown = true;
        string triggerID = string.IsNullOrEmpty(skipTriggerID) ? completeTriggerID : skipTriggerID;
        Debug.Log($"[CalendarManager] 跳过，触发ID: {triggerID}");
        CustomTrigger.FireID(triggerID);
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
        enableSkip = true;
        skipCooldown = false;
    }
}