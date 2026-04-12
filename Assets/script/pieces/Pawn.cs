using System.Collections.Generic;
using UnityEngine;


public class Pawn : Piece
{
     public override string getType()
    {
        return "p";
    }
    public bool isEnPassantVulnerable;
    public override List<Move> getAttackMove(Board board, int x, int y)
    {
        List<Move> attackMoves = new List<Move>();
        if (this.isWhite)
        {
            if (x + 1 < board.boardSize && y + 1 < board.boardSize && board.board[x + 1, y + 1] != null && !board.board[x + 1, y + 1].isWhite)
            {
                attackMoves.Add(new Move(x, y, x + 1, y + 1));
            }
            if (x - 1 >= 0 && y + 1 < board.boardSize && board.board[x - 1, y + 1] != null && !board.board[x - 1, y + 1].isWhite)
            {
                attackMoves.Add(new Move(x, y, x - 1, y + 1));
            }
        }
        else
        {
            if (x + 1 < board.boardSize && y - 1 >= 0 && board.board[x + 1, y - 1] != null && board.board[x + 1, y - 1].isWhite)
            {
                attackMoves.Add(new Move(x, y, x + 1, y - 1));
            }
            if (x - 1 >= 0 && y - 1 >= 0 && board.board[x - 1, y - 1] != null && board.board[x - 1, y - 1].isWhite)
            {
                attackMoves.Add(new Move(x, y, x - 1, y - 1));
            }
        }
        return attackMoves;
    }
    public override List<Move> getAllValidMoves(Board board, int x, int y)
    {
        //Debug.Log("Getting valid moves for Pawn at position (" + x + "," + y + ")");
        List<Move> validMoves = new List<Move>();
        if (this.hasMoved)
        {
            if (this.isWhite)
            {
                if (board.board[x, y + 1] == null)
                {
                    validMoves.Add(new Move(x, y, x, y + 1));
                }
            }
            else
            {
                if (board.board[x, y - 1] == null)
                {
                    validMoves.Add(new Move(x, y, x, y - 1));
                }
            }
        }
        else
        {
            if (this.isWhite)
            {
                if (board.board[x, y + 1] == null)
                {
                    validMoves.Add(new Move(x, y, x, y + 1));
                    if (board.board[x, y + 2] == null)
                    {
                        validMoves.Add(new Move(x, y, x, y + 2));
                    }
                }
            }
            else
            {
                if (board.board[x, y - 1] == null)
                {
                    validMoves.Add(new Move(x, y, x, y - 1));
                    if (board.board[x, y - 2] == null)
                    {
                        validMoves.Add(new Move(x, y, x, y - 2));
                    }
                }
            }
        }

        return validMoves;
    }
}