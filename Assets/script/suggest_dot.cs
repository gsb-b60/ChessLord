using UnityEngine;

public class SuggestDot : MonoBehaviour
{
    public Vector2Int position;

    public bool isAttackMove;
    public bool isCastleMove=false;
    public bool isEnPassant = false;

    public Move move;
    public void onClick()
    {
        GameManage.instance.onDotClicked(this);
    }
}