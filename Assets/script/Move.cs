using Unity.VisualScripting;

public class Move {
    public int fromX, fromY;
    public int toX, toY;


    public bool isPawnLongMove =false;
    public bool isAttack = false;
    public bool isWhite;
    public bool isCastle = false;

    public PieceView pieceView;
    public Move(int fx, int fy, int tx, int ty) {
        fromX = fx;
        fromY = fy;
        toX = tx;
        toY = ty;
    }
}