using UnityEngine;

public class PlacementZone : MonoBehaviour
{
    public string zoneID;           // Inspector里填ID，如 "slot_book"
    public string acceptObjectID;   // 只接受哪个物体，填物体的tag或name

    void Start()
    {
        InteractableRegistry.Register(zoneID, gameObject);
        Debug.Log($"[PlacementZone] 注册区域ID: {zoneID}");
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log($"[PlacementZone] 有物体进入: {other.gameObject.name}");

        // 判断是否是指定物体
        if (other.gameObject.name != acceptObjectID) return;

        Debug.Log($"[PlacementZone] 正确物体放入: {zoneID}");
        GameEvents.TriggerInteractionComplete(zoneID);
    }
}
