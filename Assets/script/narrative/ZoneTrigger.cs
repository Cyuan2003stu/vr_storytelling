using UnityEngine;

public class ZoneTrigger : MonoBehaviour
{
    public string zoneID; // InspectorÀïÌî "zone_door"

    void Start()
        => InteractableRegistry.Register(zoneID, gameObject);

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            GameEvents.TriggerInteractionComplete(zoneID);
    }
}