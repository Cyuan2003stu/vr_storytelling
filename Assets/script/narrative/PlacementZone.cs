using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class PlacementZone : MonoBehaviour
{
    [Header("区域设置")]
    public string zoneID;
    public string acceptObjectID;
    public float snapDistance = 0.2f;

    [Header("吸附设置")]
    public bool lockAfterSnap = true;

    private bool isOccupied = false;
    public bool IsOccupied => isOccupied; // ← 外部可以查询

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

        obj.transform.position = transform.position;
        obj.transform.rotation = transform.rotation;
        obj.transform.localScale = transform.localScale;

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

    void OnDrawGizmos()
    {
        Gizmos.color = isOccupied ? Color.green : Color.yellow;
        Gizmos.DrawWireSphere(transform.position, snapDistance);
    }
}