using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class FridgeDrawer : XRBaseInteractable
{
    [Header("位移设置")]
    public Transform drawerTransform;   // 抽屉本体
    public float minDistance = 0f;      // 最小位移（关闭）
    public float maxDistance = 0.5f;    // 最大位移（完全拉出）

    [Header("移动轴")]
    public Vector3 slideAxis = Vector3.forward; // 默认沿Z轴滑动

    private bool isGrabbed = false;
    private IXRSelectInteractor currentInteractor;
    private float currentDistance = 0f;
    private float grabOffset = 0f;
    private Vector3 initialPosition;

    void Start()
    {
        initialPosition = drawerTransform.localPosition;
    }

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);
        isGrabbed = true;
        currentInteractor = args.interactorObject;

        // 记录抓取时手和抽屉的位移差
        Vector3 handPos = currentInteractor.GetAttachTransform(this).position;
        float handDistance = GetDistanceAlongAxis(handPos);
        grabOffset = currentDistance - handDistance;

        Debug.Log($"[Drawer] 抓住抽屉，当前位移: {currentDistance}");
    }

    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        base.OnSelectExited(args);
        isGrabbed = false;
        currentInteractor = null;
        Debug.Log("[Drawer] 放开抽屉");
    }

    void Update()
    {
        if (!isGrabbed || currentInteractor == null) return;

        Vector3 handPos = currentInteractor.GetAttachTransform(this).position;
        float handDistance = GetDistanceAlongAxis(handPos);

        // 目标位移 = 手的位移 + 抓取时的偏移
        float targetDistance = handDistance + grabOffset;

        // 限制位移范围
        targetDistance = Mathf.Clamp(targetDistance, minDistance, maxDistance);

        currentDistance = targetDistance;

        // 应用位移，只沿指定轴移动
        drawerTransform.localPosition = initialPosition + slideAxis.normalized * currentDistance;
    }

    float GetDistanceAlongAxis(Vector3 position)
    {
        // 计算位置在滑动轴上的投影距离
        Vector3 worldAxis = drawerTransform.parent != null
            ? drawerTransform.parent.TransformDirection(slideAxis.normalized)
            : slideAxis.normalized;

        return Vector3.Dot(position - drawerTransform.parent.position, worldAxis);
    }
}