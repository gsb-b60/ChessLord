using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class Board
{
    public int boardSize = 8;
    public float squareSize = 1.0f;
    public Piece[,] board;
    public List<Piece> whitePieces;
    public List<Piece> blackPieces;

    public List<Piece> pieces;


    public static Piece[,] DeepCopyBoard(Piece[,] original)
    {
        int rows = original.GetLength(0);
        int cols = original.GetLength(1);
        Piece[,] newBoard = new Piece[rows, cols];

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                if (original[i, j] != null)
                {
                    // Option A: Using a Copy Constructor
                    newBoard[i, j] = original[i, j].Copy();

                    // Option B: Using a Clone method (if you implemented one)
                    // newBoard[i, j] = (Piece)original[i, j].Clone();
                }
            }
        }
        return newBoard;
    }



    public void PrintBoard()
{
    if (board == null)
    {
        Debug.LogError("Board is null! Cannot print.");
        return;
    }

    // Initialize as an empty string
    string boardString = "Board State:\n";
    boardString += "  a b c d e f g h\n";

    int rows = board.GetLength(0);
    int cols = board.GetLength(1);

    for (int i = 0; i < rows; i++)
    {
        // Adding the row number (8 down to 1)
        boardString += (rows - i) + " ";

        for (int j = 0; j < cols; j++)
        {
            Piece p = board[j,i];
            if (p == null)
            {
                boardString += ". ";
            }
            else
            {
                boardString += GetPieceChar(p) + " ";
            }
        }
        boardString += "\n"; // Move to next row
    }

    // Print the final result to Unity Console
    Debug.Log(boardString);
}
    private char GetPieceChar(Piece p)
    {
        // Assuming your Piece class has a Type or Name property
        char c = p.getType().ToLower()[0]; // Default to first letter (p, r, n, b, q, k)

        // Convention: Uppercase for White, Lowercase for Black
        return p.isWhite ? char.ToUpper(c) : char.ToLower(c);
    }
}