using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class FridgeDrawer : XRBaseInteractable
{
    [Header("位移设置")]
    public Transform drawerTransform;
    public float minDistance = -0.15f;  // 锁定位置
    [HideInInspector] public float currentMaxDistance = -0.15f; // 由门控制

    public Vector3 slideAxis = Vector3.forward;

    private bool isGrabbed = false;
    private IXRSelectInteractor currentInteractor;
    private float currentDistance = -0.15f;
    private float grabOffset = 0f;
    private Vector3 initialPosition;

    void Start()
    {
        initialPosition = drawerTransform.localPosition;
        currentDistance = minDistance;
    }

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);
        isGrabbed = true;
        currentInteractor = args.interactorObject;

        Vector3 handPos = currentInteractor.GetAttachTransform(this).position;
        float handDistance = GetDistanceAlongAxis(handPos);
        grabOffset = currentDistance - handDistance;
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
            float handDistance = GetDistanceAlongAxis(handPos);
            float targetDistance = handDistance + grabOffset;

            // 用门传来的 currentMaxDistance 限制最大距离
            targetDistance = Mathf.Clamp(targetDistance, minDistance, currentMaxDistance);
            currentDistance = targetDistance;
        }
        else
        {
            // 没抓着时，如果门关上了就把抽屉推回去
            currentDistance = Mathf.Clamp(currentDistance, minDistance, currentMaxDistance);
        }

        drawerTransform.localPosition = initialPosition + slideAxis.normalized * currentDistance;
    }

    float GetDistanceAlongAxis(Vector3 position)
    {
        Vector3 worldAxis = drawerTransform.parent != null
            ? drawerTransform.parent.TransformDirection(slideAxis.normalized)
            : slideAxis.normalized;

        return Vector3.Dot(position - drawerTransform.parent.position, worldAxis);
    }
}