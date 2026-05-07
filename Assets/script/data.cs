using System;
using System.Collections.Generic;
public static class GameData
{
    public static int selectedLevel;
    public static int selectedSide=1;

    public static List<int> sideListTurn = new List<int>();
    private static readonly Random _rand = new Random();


    public static void reset()
    {
        selectedLevel = 0;
        selectedSide = 0;
        sideListTurn.Clear();
    }


    public static void addSideListTurn(int side)
    {
        if(side ==0)
        if(sideListTurn.Count >0)
        {
            if(sideListTurn[sideListTurn.Count - 1] == side)
            {
                return;
            }
        }
        sideListTurn.Add(side);
    }
    public static void changeSide()
    {
        selectedSide *= -1;
    }
    public static bool getPlayerSide()
    {
        if (selectedSide == 1)
        {
            return true;
        }
        else if (selectedSide == -1)
        {
            return false;
        }
        else
        {
            return _rand.Next(2) == 0;
        }
    }



    public static string getFenChar(Piece piece)
    {
        if (piece == null) return "1";
        string fenChar = piece switch 
        {
            Pawn => fenChar = "p",
            Knight => fenChar = "n",
            Rook => fenChar = "r",
            Bishop => fenChar = "b",
            Queen => fenChar = "q",
            King => fenChar = "k",
            _ => fenChar = "1"
        };
        return piece.isWhite ? fenChar.ToUpper() : fenChar.ToLower();
    }
}