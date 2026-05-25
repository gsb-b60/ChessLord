using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;


public class Pawn : Piece
{
    public override string getType()
    {
        return "p";
    }
    public bool isEnPassantVulnerable = false;
    public override List<Move> getAttackMove(Board board, int x, int y)
    {
        List<Move> attackMoves = new List<Move>();
        if (this.isWhite)
        {
            // bool isInBoard = x + 1 < board.boardSize && y + 1 < board.boardSize;

            // bool isBlackPiece = false;
            // if (isInBoard)
            // {
            //     isBlackPiece = board.board[x + 1, y + 1] != null && (!board.board[x + 1, y + 1].isWhite);
            // }

            // bool isEnpassant = false;
            // if (isInBoard && board.board[x + 1, y] is Pawn)
            // {
            //     if(board.board[x + 1, y] is Pawn && !board.board[x+1,y].isWhite)
            //     {
            //         attackMoves.Add(new Move(x, y, x + 1, y +1));
            //     }
            //     Pawn thisPawn = board.board[x + 1, y] as Pawn;
                



            // }


            // if (isInBoard && (isBlackPiece || isEnpassant))
            // {
            //     //attackMoves.Add(new Move(x, y, x + 1, y + 1));
            // }

            // isInBoard = x - 1 >=0 && y + 1 < board.boardSize;
            // isBlackPiece = false;
            // if (isInBoard)
            // {
            //     isBlackPiece = board.board[x - 1, y + 1] != null && !board.board[x - 1, y + 1].isWhite;
            // }

            // isEnpassant = false;
            // if (isBlackPiece && board.board[x - 1, y] is Pawn)
            // {
            //     Pawn thisPawn = board.board[x - 1, y] as Pawn;
            //     isEnpassant = thisPawn.isEnPassantVulnerable;
            // }
            // if (isInBoard && (isBlackPiece || isEnpassant))
            // {
            //     //attackMoves.Add(new Move(x, y, x - 1, y + 1));
            // }

            if (x + 1 < board.boardSize && y + 1 >= 0 && board.board[x + 1, y + 1] != null && !board.board[x + 1, y + 1].isWhite)
            {
                attackMoves.Add(new Move(x, y, x + 1, y + 1));
            }
            if (x - 1 >= 0 && y + 1 >= 0 && board.board[x - 1, y + 1] != null && !board.board[x - 1, y + 1].isWhite)
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
                        Move move= new Move(x, y, x, y + 2);
                        move.isPawnLongMove=true;
                        validMoves.Add(move);
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
                        Move move= new Move(x, y, x, y -2);
                        move.isPawnLongMove=true;
                        validMoves.Add(move);
                    }
                }
            }
        }

        return validMoves;
    }
}