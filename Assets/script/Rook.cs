using System.Collections.Generic;

public class Rook : Piece
{
    public List<Move> getAllValidMoves(Board board, int x, int y)
    {
        List<Move> validMoves = new List<Move>();
        for(int i=1; i<board.boardSize; i++)
        {
            if(x+i<board.boardSize)
            {
                if(board.board[x+i,y]==null)
                {
                    validMoves.Add(new Move(x, y, x + i, y));
                }
                else
                {
                    if(board.board[x+i,y].isWhite!=this.isWhite)
                    {
                        validMoves.Add(new Move(x, y, x + i, y));
                    }
                    break;
                }
            }
        }
        for(int i=1; i<board.boardSize; i++)
        {
            if(x-i>=0)
            {
                if(board.board[x-i,y]==null)
                {
                    validMoves.Add(new Move(x, y, x - i, y));
                }
                else
                {
                    if(board.board[x-i,y].isWhite!=this.isWhite)
                    {
                        validMoves.Add(new Move(x, y, x - i, y));
                    }
                    break;
                }
            }
        }
        for(int i=1; i<board.boardSize; i++)
        {
            if(y+i<board.boardSize)
            {
                if(board.board[x,y+i]==null)
                {
                    validMoves.Add(new Move(x, y, x, y + i));
                }
                else
                {
                    if(board.board[x,y+i].isWhite!=this.isWhite)
                    {
                        validMoves.Add(new Move(x, y, x, y + i));
                    }
                    break;
                }
            }
        }
        for(int i=1; i<board.boardSize; i++)
        {
            if(y-i>=0)
            {
                if(board.board[x,y-i]==null)
                {
                    validMoves.Add(new Move(x, y, x, y - i));
                }
                else
                {
                    if(board.board[x,y-i].isWhite!=this.isWhite)
                    {
                        validMoves.Add(new Move(x, y, x, y - i));
                    }
                    break;
                }
            }
        }
        return validMoves;
    }
}