using System.Collections.Generic;
using UnityEngine;


public class Queen : Piece
{
     public override string getType()
    {
        return "q";
    }
    public override List<Move> getAllValidMoves(Board board, int x, int y)
    {
        List<Move> validMoves = new List<Move>();
        Rook rook = new Rook();
        rook.isWhite = this.isWhite;
        Bishop bishop = new Bishop();
        bishop.isWhite = this.isWhite;
        validMoves.AddRange(rook.getAllValidMoves(board, x, y));
        validMoves.AddRange(bishop.getAllValidMoves(board, x, y));
        return validMoves;
    }

    public override List<Move> getAttackMove(Board board, int x, int y)
    {
        List<Move> validMoves = new List<Move>();
        Rook rook = new Rook();
        rook.isWhite = this.isWhite;
        Bishop bishop = new Bishop();
        bishop.isWhite = this.isWhite;
        validMoves.AddRange(rook.getAttackMove(board, x, y));
        validMoves.AddRange(bishop.getAttackMove(board, x, y));
        return validMoves;
    }
}