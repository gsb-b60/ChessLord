using UnityEngine;

public class SuggestDot : MonoBehaviour
{
    public Vector2Int position;

    public bool isAttackMove;
    public void onClick()
    {
        GameManage.instance.onDotClicked(this);
    }
}