using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class MaterialSwapper : XRGrabInteractable
{
    [Header("材质设置")]
    public Renderer targetRenderer;     // 拖入物体的 Renderer
    public Material placedMaterial;     // 放置时的材质（A）
    public Material grabbedMaterial;    // 拿起时的材质（B）

    protected override void Awake()
    {
        base.Awake();

        // 默认显示放置时的材质
        if (targetRenderer != null && placedMaterial != null)
            targetRenderer.material = placedMaterial;
    }

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);

        // 拿起时换成 B 材质
        if (targetRenderer != null && grabbedMaterial != null)
        {
            targetRenderer.material = grabbedMaterial;
            Debug.Log("[MaterialSwapper] 切换到 Grabbed 材质");
        }
    }

    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        base.OnSelectExited(args);

        // 放下时换回 A 材质
        if (targetRenderer != null && placedMaterial != null)
        {
            targetRenderer.material = placedMaterial;
            Debug.Log("[MaterialSwapper] 切换回 Placed 材质");
        }
    }
}