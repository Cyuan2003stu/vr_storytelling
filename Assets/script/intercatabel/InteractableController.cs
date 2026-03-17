using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class InteractableController : MonoBehaviour
{
    public XRBaseInteractable targetInteractable;

    public void DisableGrab()
    {
        if (targetInteractable != null)
        {
            targetInteractable.enabled = false;
            Debug.Log($"[InteractableController] 禁用抓取: {targetInteractable.name}");
        }
    }

    public void EnableGrab()
    {
        if (targetInteractable != null)
        {
            targetInteractable.enabled = true;
            Debug.Log($"[InteractableController] 启用抓取: {targetInteractable.name}");
        }
    }
}