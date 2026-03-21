using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class BoxFromNPC : XRGrabInteractable
{
    [Header("叙事设置")]
    public string interactableID;

    [Header("NPC设置")]
    public Transform npcHand;

    private bool taken = false;

    protected override void Awake()
    {
        base.Awake();
        // 开始时跟随 NPC 手
        if (npcHand != null)
            transform.SetParent(npcHand);
    }

    void Update()
    {
        // 确保被抓取后不再跟随 NPC
        if (taken && transform.parent != null)
        {
            transform.SetParent(null);

            var rb = GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = false;
                rb.useGravity = true;
            }

            Debug.Log("[BoxFromNPC] 强制断开父子关系，开启重力");
        }
    }
    void Start()
    {
        InteractableRegistry.Register(interactableID, gameObject);
        Debug.Log($"[BoxFromNPC] 注册ID: {interactableID}");
    }

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        if (taken) return;
        taken = true;

        // 先调用 base 让 XR 系统注册抓取
        base.OnSelectEntered(args);

        // 强制脱离 NPC 手
        transform.SetParent(null);

        var rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = false;
            rb.useGravity = false; // 抓着时关闭重力，放手后开启
        }

        Debug.Log($"[BoxFromNPC] 箱子被接过，触发ID: {interactableID}");
        GameEvents.TriggerInteractionComplete(interactableID);
    }

    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        base.OnSelectExited(args);

        // 放手后开启重力
        var rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.useGravity = true;
        }

        Debug.Log("[BoxFromNPC] 箱子被放下");
    }
}