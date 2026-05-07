public class Move
{
    public int fromX, fromY;
    public int toX, toY;


    public bool isPawnLongMove = false;
    public bool isAttack = false;
    public bool isWhite;
    public bool isCastle = false;
    public PieceType promoteTo = PieceType.None;

    public CheckType checkType = CheckType.None;

    public PieceView pieceView;
    public Move(int fx, int fy, int tx, int ty)
    {
        fromX = fx;
        fromY = fy;
        toX = tx;
        toY = ty;
    }

    public static string Convert(int x, int y)
    {
        char column = (char)('a' + x);
        int row = y+1;
        return $"{column}{row}";
    }
    public static string GetCharChessPiece(PieceView piece)
    {
        return piece.pieceData switch
        {
            Pawn => "",
            Knight => "N",
            Bishop => "B",
            Rook => "R",
            Queen => "Q",
            King => "K",
            _ => ""
        };
    }
    public string getAttackChar()
    {
        if(pieceView.pieceData is Pawn && isAttack)
        {
            return Convert(fromX, fromY)[0].ToString()+"x"; 
        }
        return isAttack ? "x" : "";
    }
    public string isCheckChar()
    {
        return checkType switch
        {
            CheckType.Check => "+",
            CheckType.Checkmate => "#",
            CheckType.Stalemate => "½-½",
            _ => ""
        };
    }
    public string ConvertPromotion()
    {
        return promoteTo switch
        {
            PieceType.Queen => "=Q",
            PieceType.Rook => "=R",
            PieceType.Bishop => "=B",
            PieceType.Knight => "=N",
            _ => ""
        };
    }
    public override string ToString()
    {
        if (isCastle)
        {
            if (toX == 6) 
                return "O-O";
            else 
                return "O-O-O";
        }
        return $"{GetCharChessPiece(pieceView)}{getAttackChar()}{Convert(toX, toY)}{ConvertPromotion()}{isCheckChar()}";
    }
    public string enPassantFen()
    {
        return $"{Convert(toX, toY)}";
    }
}