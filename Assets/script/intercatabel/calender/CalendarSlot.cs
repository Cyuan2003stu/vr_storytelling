using UnityEngine;

public class CalendarSlot : MonoBehaviour
{
    [Header("槽位设置")]
    public string acceptPieceID;      // 接受哪个碎片，填碎片的 pieceID
    public float snapDistance = 0.15f;// 吸附距离

    private bool isFilled = false;
    private CalendarPuzzle puzzle;

    void Start()
    {
        puzzle = FindObjectOfType<CalendarPuzzle>();
    }

    void Update()
    {
        if (isFilled) return;

        // 在范围内寻找对应碎片
        Collider[] colliders = Physics.OverlapSphere(transform.position, snapDistance);
        foreach (var col in colliders)
        {
            CalendarPiece piece = col.GetComponent<CalendarPiece>();
            if (piece == null) continue;
            if (piece.pieceID != acceptPieceID) continue;
            if (piece.isSnapped) continue;

            // 碎片正在被抓着就不吸附
            if (piece.isSelected) continue;

            SnapPiece(piece);
            break;
        }
    }

    void SnapPiece(CalendarPiece piece)
    {
        isFilled = true;
        piece.isSnapped = true;

        // 对齐到槽位
        piece.transform.position = transform.position;
        piece.transform.rotation = transform.rotation;

        // 禁用抓取
        piece.enabled = false;

        Debug.Log($"[Slot] {acceptPieceID} 吸附成功");
        puzzle.OnPieceSnapped(acceptPieceID);
    }

    // 在 Scene 视图里显示吸附范围
    void OnDrawGizmos()
    {
        Gizmos.color = isFilled ? Color.green : Color.yellow;
        Gizmos.DrawWireSphere(transform.position, snapDistance);
    }
}