using UnityEngine;
using UnityEngine.Events;

public class SignalEnabler : MonoBehaviour
{
    [Header("触发设置")]
    public UnityEvent onEnable;  // 启用时触发的事件

    void OnEnable()
    {
        onEnable?.Invoke();
    }
}