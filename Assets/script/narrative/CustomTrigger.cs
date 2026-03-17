using UnityEngine;

public class CustomTrigger : MonoBehaviour
{
    public string triggerID;

    // 任何脚本或 Signal Receiver 都可以调用这个
    public void Fire()
    {
        Debug.Log($"[CustomTrigger] 触发: {triggerID}");
        GameEvents.TriggerInteractionComplete(triggerID);
    }

    // 也可以指定ID触发，不用挂脚本的ID
    public static void FireID(string id)
    {
        Debug.Log($"[CustomTrigger] 静态触发: {id}");
        GameEvents.TriggerInteractionComplete(id);
    }
}