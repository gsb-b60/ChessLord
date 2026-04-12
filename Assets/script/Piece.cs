using System.Collections.Generic;

public class Piece 
{
    public bool isWhite;
    public bool hasMoved;
    public virtual List<Move> getAllValidMoves(Board board, int x, int y)
    {
        return new List<Move>();
    }
    public virtual List<Move> getAttackMove(Board board, int x, int y)
    {
        return new List<Move>();
    }

    public Piece(Piece other)
    {
        this.isWhite = other.isWhite;
        this.hasMoved = other.hasMoved;
        // Copy all other necessary properties here
    }
    public Piece()
    {
        // Default constructor
    }

    public virtual string getType()
    {
        return "p";
    }

    public Piece Copy()
    {
        // This creates a new object of the EXACT same derived type
        return (Piece)this.MemberwiseClone();
    }
}