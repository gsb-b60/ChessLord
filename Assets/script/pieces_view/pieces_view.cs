using UnityEngine;

public class PieceView : MonoBehaviour
{
    public int id;
    

    public Piece pieceData;

    public Vector2Int position;

    public void Init(Piece piece,int x, int y)
    {
        pieceData = piece;
        position = new Vector2Int(x, y);

    }


    public void onClick()
    {
        GameManage.instance.onPieceClicked(this);
    }


    public void MoveTo(int x, int y)
    {
        
    }

}