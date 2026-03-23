using UnityEngine;

public class Slipperdetach : MonoBehaviour
{
    [Header("设置")]
    public Transform tablePosition;

    [Header("抛出设置")]
    public bool throwOnDetach = false;   // 是否抛出
    public Vector3 throwForce = new Vector3(0, 2f, 5f);  // 抛出力方向和大小
    public ForceMode forceMode = ForceMode.Impulse;       // 力的模式

    public void DetachFromParent()
    {
        transform.SetParent(null);

        if (tablePosition != null)
        {
            transform.position = tablePosition.position;
            transform.rotation = tablePosition.rotation;
        }

        var rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            if (throwOnDetach)
            {
                // 抛出模式，开启物理
                rb.isKinematic = false;
                rb.useGravity = true;
                // 沿物体自身方向施加力
                rb.AddForce(transform.TransformDirection(throwForce), forceMode);
                Debug.Log($"[BoxDetach] 抛出力: {throwForce}");
            }
            else
            {
                // 放置模式，保持静止
                rb.isKinematic = true;
                rb.useGravity = false;
            }
        }

        Debug.Log("[BoxDetach] 脱离父子关系完成");
    }
}