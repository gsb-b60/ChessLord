using System.Collections.Generic;

public class Pawn : Piece
{
    public bool isEnPassantVulnerable;
    public List<Move> getValidMoves(Board board, int x, int y)
    {
        List<Move> validMoves = new List<Move>();
        if(this.hasMoved)
        {
            if(this.isWhite)
            {
                if(board.board[x,y+1]==null)
                {
                    validMoves.Add(new Move(x, y, x, y + 1));
                }
            }
            else
            {
                if(board.board[x,y-1]==null)
                {
                    validMoves.Add(new Move(x, y, x, y - 1));
                }
            }
        }
        else
        {
            if(this.isWhite)
            {
                if(board.board[x,y+1]==null)
                {
                    validMoves.Add(new Move(x, y, x, y + 1));
                    if(board.board[x,y+2]==null)
                    {
                        validMoves.Add(new Move(x, y, x, y + 2));
                    }
                }
            }
            else
            {
                if(board.board[x,y-1]==null)
                {
                    validMoves.Add(new Move(x, y, x, y - 1));
                    if(board.board[x,y-2]==null)
                    {
                        validMoves.Add(new Move(x, y, x, y - 2));
                    }
                }
            }
        }
        
        return validMoves;
    }
}