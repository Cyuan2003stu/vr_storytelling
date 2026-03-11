using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class FridgeDoor : XRBaseInteractable
{
    [Header("旋转设置")]
    public Transform pivotPoint;
    public float minAngle = 0f;
    public float maxAngle = 120f;

    private bool isGrabbed = false;
    private IXRSelectInteractor currentInteractor;
    private float currentAngle = 0f;
    private float grabAngleOffset = 0f; // 抓取时手和门的角度差

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);
        isGrabbed = true;
        currentInteractor = args.interactorObject;

        // 记录抓取时手的角度和门当前角度的差值
        Vector3 handPos = currentInteractor.GetAttachTransform(this).position;
        float handAngle = GetAngleFromPivot(handPos);
        grabAngleOffset = currentAngle - handAngle;

        Debug.Log($"[FridgeDoor] 抓住门，当前角度: {currentAngle}, 手角度: {handAngle}");
    }

    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        base.OnSelectExited(args);
        isGrabbed = false;
        currentInteractor = null;
        Debug.Log("[FridgeDoor] 放开门");
    }

    void Update()
    {
        if (!isGrabbed || currentInteractor == null) return;

        Vector3 handPos = currentInteractor.GetAttachTransform(this).position;
        float handAngle = GetAngleFromPivot(handPos);

        // 目标角度 = 手的角度 + 抓取时的偏移
        float targetAngle = handAngle + grabAngleOffset;

        // 限制角度范围
        targetAngle = Mathf.Clamp(targetAngle, minAngle, maxAngle);

        // 直接跟随，不做平滑（让门完全跟手）
        currentAngle = targetAngle;

        // 应用旋转
        pivotPoint.localRotation = Quaternion.Euler(0, currentAngle, 0);
    }

    float GetAngleFromPivot(Vector3 position)
    {
        // 计算位置相对于铰链的角度
        Vector3 direction = position - pivotPoint.position;
        return Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
    }
}