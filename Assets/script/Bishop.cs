using System.Collections.Generic;

public class Bishop: Piece
{
    public List<Move> getValidMoves(Board board, int x, int y)
    {
        List<Move> validMoves = new List<Move>();
        for(int i=1; i<board.boardSize; i++)
        {
            if(x+i<board.boardSize && y+i<board.boardSize)
            {
                if(board.board[x+i,y+i]==null)
                {
                    validMoves.Add(new Move(x, y, x + i, y + i));
                }
                else
                {
                    if(board.board[x+i,y+i].isWhite!=this.isWhite)
                    {
                        validMoves.Add(new Move(x, y, x + i, y + i));
                    }
                    break;
                }
            }
        }
        for(int i=1; i<board.boardSize; i++)
        {
            if(x-i>=0 && y+i<board.boardSize)
            {
                if(board.board[x-i,y+i]==null)
                {
                    validMoves.Add(new Move(x, y, x - i, y + i));
                }
                else
                {
                    if(board.board[x-i,y+i].isWhite!=this.isWhite)
                    {
                        validMoves.Add(new Move(x, y, x - i, y + i));
                    }
                    break;
                }
            }
        }
        for(int i=1; i<board.boardSize; i++)
        {
            if(x+i<board.boardSize && y-i>=0)
            {
                if(board.board[x+i,y-i]==null)
                {
                    validMoves.Add(new Move(x, y, x + i, y - i));
                }
                else
                {
                    if(board.board[x+i,y-i].isWhite!=this.isWhite)
                    {
                        validMoves.Add(new Move(x, y, x + i, y - i));
                    }
                    break;
                }
            }
        }
        for(int i=1; i<board.boardSize; i++)
        {
            if(x-i>=0 && y-i>=0)
            {
                if(board.board[x-i,y-i]==null)
                {
                    validMoves.Add(new Move(x, y, x - i, y - i));
                }
                else
                {
                    if(board.board[x-i,y-i].isWhite!=this.isWhite)
                    {
                        validMoves.Add(new Move(x, y, x - i, y - i));
                    }
                    break;
                }
            }
        }
        return validMoves;
    }


}