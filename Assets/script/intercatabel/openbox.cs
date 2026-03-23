using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class openbox: XRBaseInteractable
{
    [Header("旋转设置")]
    public Transform pivotPoint;
    public float minAngle = -90f;
    public float maxAngle = 0f;

    [Header("触发设置")]
    public float triggerAngle = 45f;
    public string triggerID = "door_opened";
    private bool triggered = false;

    [Header("初始状态")]
    public bool isLocked = false;

    private bool isGrabbed = false;
    private IXRSelectInteractor currentInteractor;
    private float currentAngle = 0f;
    private float grabAngleOffset = 0f;

    public void UnlockDoor()
    {
        isLocked = false;
        Debug.Log("[Door] 解锁，可以开门");
    }

    public void LockDoor()
    {
        isLocked = true;
        Debug.Log("[Door] 锁定，不能开门");
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
            pivotPoint.localRotation = Quaternion.Euler(0, currentAngle, 0);

            // 实时显示当前角度
            Debug.Log($"[Door] 当前角度: {currentAngle:F1}");
        }

        // 检查触发条件
        if (!triggered && currentAngle <= triggerAngle)
        {
            triggered = true;
            Debug.Log($"[Door] 触发！角度: {currentAngle:F1}，触发ID: {triggerID}");
            CustomTrigger.FireID(triggerID);
        }
    }

    float GetAngleFromPivot(Vector3 position)
    {
        Vector3 direction = position - pivotPoint.position;
        return Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
    }
}