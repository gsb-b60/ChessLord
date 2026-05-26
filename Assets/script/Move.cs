using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;

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

    public bool isEngineMove;
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
        int row = y + 1;
        return $"{column}{row}";
    }
    public static string GetCharChessPiece(PieceView piece)
    {
        if (piece == null || piece.pieceData == null)
            return "";

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
        if (!isAttack) return "";

        if (pieceView != null && pieceView.pieceData is Pawn)
        {
            return Convert(fromX, fromY)[0].ToString() + "x";
        }
        return "x";
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

        if (pieceView != null)
        {
            return $"{GetCharChessPiece(pieceView)}{getAttackChar()}{Convert(toX, toY)}{ConvertPromotion()}{isCheckChar()}";
        }
        else
        {
            // For engine moves before they are enriched with piece information
            string uci = $"{Convert(fromX, fromY)}{Convert(toX, toY)}";
            if (promoteTo != PieceType.None)
            {
                uci += promoteTo switch
                {
                    PieceType.Queen => "q",
                    PieceType.Rook => "r",
                    PieceType.Bishop => "b",
                    PieceType.Knight => "n",
                    _ => ""
                };
            }
            return uci + isCheckChar();
        }
    }
    public string enPassantFen()
    {
        return $"{Convert(toX, toY)}";
    }

    public static int[] ConvertNotationToCoords(string notation)
    {
        if (string.IsNullOrWhiteSpace(notation) || notation.Length < 2)
            return null;

        // Convert file (char) to index: 'a' -> 0, 'b' -> 1, etc.
        // We use lowercase to ensure 'D4' and 'd4' both work.
        int file = char.ToLower(notation[0]) - 'a';

        // Convert rank (char) to index: '1' -> 0, '2' -> 1, etc.
        // Subtract '1' from the numeric value.
        int rank = (int)char.GetNumericValue(notation[1]) - 1;

        // Validation to ensure it's on a standard 8x8 board
        if (file < 0 || file > 7 || rank < 0 || rank > 7)
        {
            Debug.Log("Invalid move.");
            return null;
        }

        return new int[] { file, rank };
    }

    public static Move convertUCIToMove(string move)
    {
        // 1. Extract the two parts of the UCI string
        // "e2e4" -> fromPart = "e2", toPart = "e4"

        string fromPart = move.Substring(0, 2);
        string toPart = move.Substring(2, 2);

        // 2. Convert both to coordinate arrays [file, rank]
        int[] fromCoords = ConvertNotationToCoords(fromPart);
        int[] toCoords = ConvertNotationToCoords(toPart);
        Move engineMove = new Move(fromCoords[0], fromCoords[1], toCoords[0], toCoords[1]);

        // 3. Safety check: if either conversion failed, return a default/null Move

        if (move.Length == 5)
        {
            char promotionPiece = move[4]; // 'q', 'r', 'b', or 'n'
            engineMove.promoteTo = promotionPiece switch
            {
                'q' => PieceType.Queen,
                'r' => PieceType.Rook,
                'b' => PieceType.Bishop,
                'n' => PieceType.Knight,
                _ => PieceType.None
            };
        }
        if (fromCoords == null || toCoords == null)
        {
            Debug.LogError($"Failed to parse UCI move: {move}");
            return null;
        }

        // 4. Create the Move object using the extracted indices
        // Order: fromFile, fromRank, toFile, toRank


        engineMove.isEngineMove = true;

        return engineMove;
    }
}