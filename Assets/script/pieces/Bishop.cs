using System.Collections.Generic;

public class Bishop : Piece
{

     public override string getType()
    {
        return "b";
    }
    public override List<Move> getAllValidMoves(Board board, int x, int y)
    {
        List<Move> validMoves = new List<Move>();
        for (int i = 1; i < board.boardSize; i++)
        {
            if (x + i < board.boardSize && y + i < board.boardSize)
            {
                if (board.board[x + i, y + i] == null)
                {
                    validMoves.Add(new Move(x, y, x + i, y + i));
                }
                else
                {
                    break;
                }
            }
        }
        for (int i = 1; i < board.boardSize; i++)
        {
            if (x - i >= 0 && y + i < board.boardSize)
            {
                if (board.board[x - i, y + i] == null)
                {
                    validMoves.Add(new Move(x, y, x - i, y + i));
                }
                else
                {
                    break;
                }
            }
        }
        for (int i = 1; i < board.boardSize; i++)
        {
            if (x + i < board.boardSize && y - i >= 0)
            {
                if (board.board[x + i, y - i] == null)
                {
                    validMoves.Add(new Move(x, y, x + i, y - i));
                }
                else
                {
                    break;
                }
            }
        }
        for (int i = 1; i < board.boardSize; i++)
        {
            if (x - i >= 0 && y - i >= 0)
            {
                if (board.board[x - i, y - i] == null)
                {
                    validMoves.Add(new Move(x, y, x - i, y - i));
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
        List<Move> validMoves = new List<Move>();
        for (int i = 1; i < board.boardSize; i++)
        {
            if (x + i < board.boardSize && y + i < board.boardSize)
            {
                if (board.board[x + i, y + i] != null && board.board[x + i, y + i].isWhite != this.isWhite)
                {
                    if (board.board[x + i, y + i].isWhite != this.isWhite)
                    {
                        validMoves.Add(new Move(x, y, x + i, y + i));
                        break;
                    }
                    else
                    {
                        break;
                    }
                }
                
            }
        }
        for (int i = 1; i < board.boardSize; i++)
        {
            if (x - i >= 0 && y + i < board.boardSize)
            {
                if (board.board[x - i, y + i] != null)
                {
                    if (board.board[x - i, y + i].isWhite != this.isWhite)
                    {
                        validMoves.Add(new Move(x, y, x - i, y + i));
                        break;
                    }
                    else
                    {
                        break;
                    }
                    
                }
            
            }
        }
        for (int i = 1; i < board.boardSize; i++)
        {
            if (x + i < board.boardSize && y - i >= 0)
            {
                if (board.board[x + i, y - i] != null )
                {
                    if (board.board[x + i, y - i].isWhite != this.isWhite)
                    {
                        validMoves.Add(new Move(x, y, x + i, y - i));
                        break;
                    }
                    else
                    {
                        break;
                    }
                }
                
                
            }
        }
        for (int i = 1; i < board.boardSize; i++)
        {
            if (x - i >= 0 && y - i >= 0)
            {
                if (board.board[x - i, y - i] != null )
                {
                    if (board.board[x - i, y - i].isWhite != this.isWhite)
                    {
                        validMoves.Add(new Move(x, y, x - i, y - i));
                        break;
                    }
                    else
                    {
                        break;
                    }
                }
                
            }
        }
        return validMoves;
    }


}