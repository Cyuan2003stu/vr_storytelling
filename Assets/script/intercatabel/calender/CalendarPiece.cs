using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class CalendarPiece : XRGrabInteractable
{
    [Header("碎片设置")]
    public string pieceID;            // 如 "piece_01"

    [HideInInspector] public bool isSnapped = false;

    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        base.OnSelectExited(args);
        // 放手逻辑由 CalendarSlot 负责
    }
}