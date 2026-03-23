using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class BoxFromNPC : XRGrabInteractable
{
    [Header("叙事设置")]
    public string interactableID;

    [Header("NPC设置")]
    public Transform npcHand;

    [Header("初始状态")]
    public bool canGrab = false;

    private bool taken = false;

    protected override void Awake()
    {
        base.Awake();
        if (npcHand != null)
            transform.SetParent(npcHand);
    }

    void Start()
    {
        InteractableRegistry.Register(interactableID, gameObject);
        Debug.Log($"[BoxFromNPC] 注册ID: {interactableID}");
    }

    // ← 关键：在这里控制是否允许抓取
    public override bool IsSelectableBy(IXRSelectInteractor interactor)
    {
        if (!canGrab) return false;
        return base.IsSelectableBy(interactor);
    }

    public void EnableGrab()
    {
        canGrab = true;
        Debug.Log("[BoxFromNPC] 可以抓取了");
    }

    public void DisableGrab()
    {
        canGrab = false;
        Debug.Log("[BoxFromNPC] 禁止抓取");
    }

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        if (taken) return;
        taken = true;

        base.OnSelectEntered(args);

        transform.SetParent(null);

        var rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = false;
            rb.useGravity = false;
        }

        Debug.Log($"[BoxFromNPC] 箱子被接过，触发ID: {interactableID}");
        GameEvents.TriggerInteractionComplete(interactableID);
    }

    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        base.OnSelectExited(args);

        var rb = GetComponent<Rigidbody>();
        if (rb != null)
            rb.useGravity = true;

        Debug.Log("[BoxFromNPC] 箱子被放下");
    }

    void Update()
    {
        if (taken && transform.parent != null)
        {
            transform.SetParent(null);

            var rb = GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = false;
                rb.useGravity = true;
            }
        }
    }
}