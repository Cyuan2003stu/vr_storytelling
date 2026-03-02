using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class GrabInteractable : XRGrabInteractable
{
    public string interactableID;

    void Start()
    {
        Debug.Log($"注册物体 ID: {interactableID}");
        InteractableRegistry.Register(interactableID, gameObject);
    }

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);
        Debug.Log($"物体被抓取，触发ID: {interactableID}");
        GameEvents.TriggerInteractionComplete(interactableID);
    }
}