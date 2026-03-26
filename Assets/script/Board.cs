using System.Collections.Generic;

public class Board
{
    public int boardSize = 8;
    public float squareSize = 1.0f;
    public Piece[,] board;
    public List<Piece> whitePieces;
    public List<Piece> blackPieces;

    public List<Piece> pieces;
}