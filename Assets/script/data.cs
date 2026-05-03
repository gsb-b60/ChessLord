using System;
using System.Collections.Generic;
using math = System.Math;
public static class GameData
{
    public static int selectedLevel;
    public static int selectedSide;

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
}