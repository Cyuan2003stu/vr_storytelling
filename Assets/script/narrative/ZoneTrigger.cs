using UnityEngine;

public class ZoneTrigger : MonoBehaviour
{
    public string zoneID;
    private bool triggered = false;

    void Start()
        => InteractableRegistry.Register(zoneID, gameObject);

    void OnTriggerEnter(Collider other)
    {
        if (triggered) return;
        if (!other.CompareTag("Player")) return;

        triggered = true;
        GameEvents.TriggerInteractionComplete(zoneID);
        Debug.Log($"[ZoneTrigger] {zoneID} 触发");

        // 重新开启所有 Renderer
        foreach (var r in GetComponentsInChildren<Renderer>())
            r.enabled = true;
    }
}