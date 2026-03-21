using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class PlacementZone : MonoBehaviour
{
    [Header("区域设置")]
    public string zoneID;
    public string acceptObjectID;
    public float snapDistance = 0.2f;

    [Header("吸附设置")]
    public bool lockAfterSnap = true;   // ← 勾选锁定，不勾选可以再拿走

    [Header("替换设置")]
    public GameObject zoneVisual;

    [Header("对齐设置")]
    public bool matchPosition = true;
    public bool matchRotation = true;
    public bool matchScale = true;

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

        if (matchPosition) obj.transform.position = transform.position;
        if (matchRotation) obj.transform.rotation = transform.rotation;
        if (matchScale) obj.transform.localScale = transform.localScale;

        var rb = obj.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.isKinematic = true;
        }

        // 根据 lockAfterSnap 决定是否锁定抓取
        if (lockAfterSnap)
        {
            var interactable = obj.GetComponent<XRBaseInteractable>();
            if (interactable != null)
            {
                interactable.enabled = false;
                Debug.Log($"[PlacementZone] {acceptObjectID} 已锁定，不能再拿走");
            }
        }

        if (zoneVisual != null)
            zoneVisual.SetActive(false);
        else
        {
            var zoneRenderer = GetComponent<Renderer>();
            if (zoneRenderer != null)
                zoneRenderer.enabled = false;
        }

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