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

    private void Awake()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            sr.sortingOrder = 2; // Đảm bảo quân cờ luôn luôn nổi lên trên highlight (layer 0 hoặc -1)
        }
    }


    public void MoveTo(int x, int y)
    {
        
    }

}