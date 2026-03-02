using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class GrabInteractable : XRGrabInteractable
{
    public string interactableID;

    void Awake()
        => InteractableRegistry.Register(interactableID, gameObject);

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args); // 保留原有抓取物理行为
        GameEvents.TriggerInteractionComplete(interactableID);
    }
}