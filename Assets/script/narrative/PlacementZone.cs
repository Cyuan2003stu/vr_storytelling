using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class PlacementZone : MonoBehaviour
{
    [Header("区域设置")]
    public string zoneID;
    public string acceptObjectID;
    public float snapDistance = 0.2f;

    [Header("替换设置")]
    public GameObject zoneVisual;

    [Header("对齐设置")]
    public bool matchPosition = true;   // 对齐位置
    public bool matchRotation = true;   // 对齐旋转
    public bool matchScale = true;      // 对齐大小

    private bool isOccupied = false;
    public bool IsOccupied => isOccupied;

    void Start()
    {
        InteractableRegistry.Register(zoneID, gameObject);
        Debug.Log($"[PlacementZone] 注册区域ID: {zoneID}");
    }

    void Update()
    {
        if (isOccupied) return;

        Collider[] colliders = Physics.OverlapSphere(transform.position, snapDistance);
        foreach (var col in colliders)
        {
            if (col.gameObject.name != acceptObjectID) continue;

            var interactable = col.GetComponent<XRBaseInteractable>();
            if (interactable != null && interactable.isSelected) continue;

            SnapObject(col.gameObject);
            break;
        }
    }

    void SnapObject(GameObject obj)
    {
        isOccupied = true;

        // 对齐位置旋转大小
        if (matchPosition) obj.transform.position = transform.position;
        if (matchRotation) obj.transform.rotation = transform.rotation;
        if (matchScale) obj.transform.localScale = transform.localScale;

        // 冻结物理
        var rb = obj.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.isKinematic = true;
        }

        // 不 disable 物体，只隐藏 Zone 视觉
        if (zoneVisual != null)
            zoneVisual.SetActive(false);
        else
        {
            var zoneRenderer = GetComponent<Renderer>();
            if (zoneRenderer != null)
                zoneRenderer.enabled = false;
        }

        // 关掉 Collider 防止重复检测
        var col = GetComponent<Collider>();
        if (col != null) col.enabled = false;

        Debug.Log($"[PlacementZone] {acceptObjectID} 吸附成功");
        GameEvents.TriggerInteractionComplete(zoneID);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = isOccupied ? Color.green : Color.yellow;
        Gizmos.DrawWireSphere(transform.position, snapDistance);
    }
}