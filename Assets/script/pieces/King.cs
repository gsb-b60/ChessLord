using System.Collections.Generic;

public class King:Piece


{

    public override string getType()
    {
        return "k";
    }
    public override List<Move> getAllValidMoves(Board board, int x, int y)
    {
        List<Move> validMoves = new List<Move>();
        for(int i=-1; i<=1; i++)
        {
            for(int j=-1; j<=1; j++)
            {
                if(i==0 && j==0) continue;
                if(x+i>=0 && x+i<board.boardSize && y+j>=0 && y+j<board.boardSize)
                {
                    if(board.board[x+i,y+j]==null )
                    {
                        validMoves.Add(new Move(x, y, x + i, y + j));   
                    }
                }
            }
        }

        
        return validMoves;
    }

    public override List<Move> getAttackMove(Board board, int x, int y)
    {
        List<Move> validMoves = new List<Move>();
        for(int i=-1; i<=1; i++)
        {
            for(int j=-1; j<=1; j++)
            {
                if(i==0 && j==0) continue;
                if(x+i>=0 && x+i<board.boardSize && y+j>=0 && y+j<board.boardSize)
                {
                    if(board.board[x+i,y+j]!=null && board.board[x+i,y+j].isWhite != this.isWhite)
                    {
                        validMoves.Add(new Move(x, y, x + i, y + j));   
                    }
                }
            }
        }
        return validMoves;
    }
}