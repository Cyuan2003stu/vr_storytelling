using UnityEngine;

public class BoxDetach : MonoBehaviour
{
    [Header("设置")]
    public Transform tablePosition;  // 桌子上的目标位置

    // Signal Receiver 里绑定这个函数
    public void DetachFromParent()
    {
        // 脱离父子关系
        transform.SetParent(null);

        // 移动到桌子位置
        if (tablePosition != null)
        {
            transform.position = tablePosition.position;
            transform.rotation = tablePosition.rotation;
        }

        // 确保有重力
        var rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;  // 放在桌上不需要物理，保持静止
            rb.useGravity = false;
        }

        Debug.Log("[BoxDetach] 箱子已脱离父子关系，放置在桌上");
    }
}