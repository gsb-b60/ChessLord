using System.Collections.Generic;

public class Rook : Piece
{
     public override string getType()
    {
        return "r";
    }
    public override List<Move> getAllValidMoves(Board board, int x, int y)
    {
        List<Move> validMoves = new List<Move>();
        int[,] directions = { { 1, 0 }, { -1, 0 }, { 0, 1 }, { 0, -1 } };

        for (int d = 0; d < directions.GetLength(0); d++)
        {
            int dx = directions[d, 0];
            int dy = directions[d, 1];

            for (int i = 1; i < board.boardSize; i++)
            {
                int nx = x + i * dx;
                int ny = y + i * dy;

                if (nx < 0 || nx >= board.boardSize || ny < 0 || ny >= board.boardSize)
                    break;

                if (board.board[nx, ny] == null)
                {
                    validMoves.Add(new Move(x, y, nx, ny));
                }
                else
                {
                    break;
                }
            }
        }
        return validMoves;
    }

    public override List<Move> getAttackMove(Board board, int x, int y)
    {
        List<Move> attackMoves = new List<Move>();
        int[,] directions = { { 1, 0 }, { -1, 0 }, { 0, 1 }, { 0, -1 } };

        for (int d = 0; d < directions.GetLength(0); d++)
        {
            int dx = directions[d, 0];
            int dy = directions[d, 1];

            for (int i = 1; i < board.boardSize; i++)
            {
                int nx = x + i * dx;
                int ny = y + i * dy;

                if (nx < 0 || nx >= board.boardSize || ny < 0 || ny >= board.boardSize)
                    break;

                if (board.board[nx, ny] != null)
                {
                    if (board.board[nx, ny].isWhite != this.isWhite)
                    {
                        attackMoves.Add(new Move(x, y, nx, ny));
                    }
                    break;
                }
            }
        }
        return attackMoves;
    }
}