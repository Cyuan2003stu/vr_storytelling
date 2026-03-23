
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class GrabEnableObject : XRGrabInteractable
{
    [Header("叙事设置")]
    public string interactableID;

    [Header("激活设置")]
    public GameObject targetObject;  // 拿起时激活的物体

    void Start()
    {
        if (!string.IsNullOrEmpty(interactableID))
            InteractableRegistry.Register(interactableID, gameObject);
    }

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);

        // 拿起时激活目标物体
        if (targetObject != null)
        {
            targetObject.SetActive(true);
            Debug.Log($"[GrabEnableObject] 激活物体: {targetObject.name}");
        }

        if (!string.IsNullOrEmpty(interactableID))
            GameEvents.TriggerInteractionComplete(interactableID);
    }
}