using System.Collections.Generic;

public class Knight: Piece
{
    public List<Move> getValidMoves(Board board, int x, int y)
    {
        List<Move> validMoves = new List<Move>();
        int[,] directions = new int[,] { { 1, 2 }, { 1, -2 }, { -1, 2 }, { -1, -2 }, { 2, 1 }, { 2, -1 }, { -2, 1 }, { -2, -1 } };
        for (int i = 0; i < directions.GetLength(0); i++)
        {
            int newX = x + directions[i, 0];
            int newY = y + directions[i, 1];
            if (newX >= 0 && newX < board.boardSize && newY >= 0 && newY < board.boardSize)
            {
                if (board.board[newX, newY] == null || board.board[newX, newY].isWhite != this.isWhite)
                {
                    validMoves.Add(new Move(x, y, newX, newY));
                }
            }
        }
        return validMoves;
    }
}