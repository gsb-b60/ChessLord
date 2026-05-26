using TMPro;
using UnityEngine;

public class GameMatchScript : MonoBehaviour
{
    public TMP_Text resultText;
    public void SetResultText(CheckType result, bool userWon, int botLevel = 0)
    {
        string resultString = "";
        string lostToText = botLevel > 0 ? $"Thua bởi Computer Level {botLevel}!" : "You Lose!";
        string wonText = "You Win!";

        Debug.Log("Setting result text with result: " + result + ", userWon: " + userWon + ", botLevel: " + botLevel);

        switch (result)
        {
            case CheckType.Checkmate:
                resultString = userWon ? "You Win by Checkmate!" : (botLevel > 0 ? $"Thua bởi Computer Level {botLevel}!" : "You Lose by Checkmate!");
                break;
            case CheckType.Stalemate:
                resultString = "It's a Draw by Stalemate!";
                break;
            case CheckType.Resign:
                resultString = "You Resigned!";
                break;
            case CheckType.None:
                resultString = userWon ? wonText : lostToText;
                break;
        }
        resultText.text = resultString;
    }
}