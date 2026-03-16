using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class PlacementZone : MonoBehaviour
{
    [Header("区域设置")]
    public string zoneID;
    public string acceptObjectID;       // 接受的物体名字
    public float snapDistance = 0.2f;   // 吸附距离

    [Header("吸附设置")]
    public bool lockAfterSnap = true;   // 吸附后锁定不能再拿走

    private bool isOccupied = false;

    void Start()
    {
        InteractableRegistry.Register(zoneID, gameObject);
    }

    void Update()
    {
        if (isOccupied) return;

        // 在范围内寻找目标物体
        Collider[] colliders = Physics.OverlapSphere(transform.position, snapDistance);
        foreach (var col in colliders)
        {
            // 检查名字是否匹配
            if (col.gameObject.name != acceptObjectID) continue;

            // 如果物体正在被抓着就不吸附
            var interactable = col.GetComponent<XRBaseInteractable>();
            if (interactable != null && interactable.isSelected) continue;

            SnapObject(col.gameObject);
            break;
        }
    }

    void SnapObject(GameObject obj)
    {
        isOccupied = true;

        // 完全对齐到 Slot 的位置、旋转、大小
        obj.transform.position = transform.position;
        obj.transform.rotation = transform.rotation;
        obj.transform.localScale = transform.localScale; // ← 用 Slot 的大小

        var rb = obj.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            if (lockAfterSnap) rb.isKinematic = true;
        }

        if (lockAfterSnap)
        {
            var interactable = obj.GetComponent<XRBaseInteractable>();
            if (interactable != null)
                interactable.enabled = false;
        }

        Debug.Log($"[PlacementZone] {acceptObjectID} 吸附成功");
        GameEvents.TriggerInteractionComplete(zoneID);

        foreach (var r in GetComponentsInChildren<Renderer>())
            r.enabled = false;
    }

}