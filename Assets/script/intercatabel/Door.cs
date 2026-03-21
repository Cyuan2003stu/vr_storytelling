using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class Door : XRBaseInteractable
{
    [Header("旋转设置")]
    public Transform pivotPoint;
    public float minAngle = -90f;
    public float maxAngle = 0f;

    [Header("关联抽屉")]
    public FridgeDrawer drawer;

    [Header("初始状态")]
    public bool isLocked = false;

    private bool isGrabbed = false;
    private IXRSelectInteractor currentInteractor;
    private float currentAngle = 0f;
    private float grabAngleOffset = 0f;

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
        if (isLocked) return;
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

            // Y 轴旋转
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
        // Y 轴旋转用 X 和 Z 计算角度
        return Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
    }
}