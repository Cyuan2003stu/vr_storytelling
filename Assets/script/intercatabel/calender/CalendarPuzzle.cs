using UnityEngine;
using System.Collections.Generic;

public class CalendarPuzzle : MonoBehaviour
{
    [Header("碎片总数")]
    public int totalPieces = 6;       // 填你实际的碎片数量

    [Header("完成后显示的完整日历（可选）")]
    public GameObject completeCalendar;

    private int snappedCount = 0;

    public void OnPieceSnapped(string pieceID)
    {
        snappedCount++;
        Debug.Log($"[Puzzle] 已拼合 {snappedCount}/{totalPieces}");

        if (snappedCount >= totalPieces)
            OnPuzzleComplete();
    }

    void OnPuzzleComplete()
    {
        Debug.Log("[Puzzle] 日历拼图完成！");

        // 显示完整日历（可选）
        if (completeCalendar != null)
            completeCalendar.SetActive(true);

        // 触发叙事事件
        GameEvents.TriggerInteractionComplete("calendar_puzzle");
    }
}