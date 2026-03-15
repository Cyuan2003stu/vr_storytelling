using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class FridgeDoor : XRBaseInteractable
{
    [Header("旋转设置")]
    public Transform pivotPoint;
    public float minAngle = -90f;
    public float maxAngle = 0f;

    [Header("关联抽屉")]
    public FridgeDrawer drawer;

    [Header("初始状态")]
    public bool isLocked = true;     // ← 默认锁定，不能开门

    private bool isGrabbed = false;
    private IXRSelectInteractor currentInteractor;
    private float currentAngle = 0f;
    private float grabAngleOffset = 0f;

    // Signal Receiver 里绑定这两个函数
    public void UnlockDoor()
    {
        isLocked = false;
        Debug.Log("[FridgeDoor] 解锁，可以开门");
    }

    public void LockDoor()
    {
        isLocked = true;
        Debug.Log("[FridgeDoor] 锁定，不能开门");
    }

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        if (isLocked) return;       // ← 锁定时不响应抓取
        base.OnSelectEntered(args);
        isGrabbed = true;
        currentInteractor = args.interactorObject;

        Vector3 handPos = currentInteractor.GetAttachTransform(this).position;
        float handAngle = GetAngleFromPivot(handPos);
        grabAngleOffset = currentAngle - handAngle;
    }

    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        base.OnSelectExited(args);
        isGrabbed = false;
        currentInteractor = null;
    }

    void Update()
    {
        if (isGrabbed && currentInteractor != null)
        {
            Vector3 handPos = currentInteractor.GetAttachTransform(this).position;
            float handAngle = GetAngleFromPivot(handPos);
            float targetAngle = handAngle + grabAngleOffset;
            currentAngle = Mathf.Clamp(targetAngle, minAngle, maxAngle);
            pivotPoint.localRotation = Quaternion.Euler(0, currentAngle, 0);
        }

        if (drawer != null)
        {
            float t = Mathf.InverseLerp(maxAngle, minAngle, currentAngle);
            drawer.currentMaxDistance = Mathf.Lerp(-0.15f, 0.3f, t);
        }
    }

    float GetAngleFromPivot(Vector3 position)
    {
        Vector3 direction = position - pivotPoint.position;
        return Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
    }
}
