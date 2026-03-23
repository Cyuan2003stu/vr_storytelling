using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class OpenBox : XRBaseInteractable
{
    [Header("旋转设置")]
    public Transform pivotPoint;
    public float minAngle = -90f;
    public float maxAngle = 0f;

    [Header("触发设置")]
    public float triggerAngle = -60f;   // 开盖超过60度触发
    public string triggerID = "box_opened";
    private bool triggered = false;

    [Header("气球设置")]
    public GameObject balloon;           // 拖入气球物体
    public float floatSpeed = 0.5f;     // 上升速度
    public float floatHeight = 3f;      // 最终高度
    private bool balloonFloating = false;
    private Vector3 balloonStartPos;
    private Vector3 balloonTargetPos;

    [Header("初始状态")]
    public bool isLocked = false;

    private bool isGrabbed = false;
    private IXRSelectInteractor currentInteractor;
    private float currentAngle = 0f;
    private float grabAngleOffset = 0f;

    void Start()
    {
        // 气球默认隐藏
        if (balloon != null)
        {
            balloon.SetActive(false);
            balloonStartPos = balloon.transform.position;
            balloonTargetPos = balloonStartPos + Vector3.up * floatHeight;
        }
    }

    public void UnlockDoor()
    {
        isLocked = false;
        Debug.Log("[OpenBox] 解锁");
    }

    public void LockDoor()
    {
        isLocked = true;
        Debug.Log("[OpenBox] 锁定");
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
        }

        // 检查触发条件
        if (!triggered && currentAngle <= triggerAngle)
        {
            triggered = true;
            Debug.Log($"[OpenBox] 触发！角度: {currentAngle:F1}");

            // 触发气球飞出
            LaunchBalloon();

            // 触发叙事事件
            CustomTrigger.FireID(triggerID);
        }

        // 气球缓慢上升
        if (balloonFloating && balloon != null)
        {
            balloon.transform.position = Vector3.MoveTowards(
                balloon.transform.position,
                balloonTargetPos,
                floatSpeed * Time.deltaTime
            );

            // 到达目标高度停止
            if (Vector3.Distance(balloon.transform.position, balloonTargetPos) < 0.01f)
            {
                balloonFloating = false;
                Debug.Log("[OpenBox] 气球到达目标高度");
            }
        }
    }

    void LaunchBalloon()
    {
        if (balloon == null) return;

        // 显示气球
        balloon.SetActive(true);
        balloonFloating = true;

        // 脱离箱子父子关系
        balloon.transform.SetParent(null);

        // 重置起始和目标位置
        balloonStartPos = balloon.transform.position;
        balloonTargetPos = balloonStartPos + Vector3.up * floatHeight;

        Debug.Log("[OpenBox] 气球开始上升");
    }

    float GetAngleFromPivot(Vector3 position)
    {
        Vector3 direction = position - pivotPoint.position;
        return Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
    }
}