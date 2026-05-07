using TMPro;
using UnityEngine;

class GameMatchScript : MonoBehaviour
{
    TMP_Text resultText;
    public void SetResultText(CheckType result, bool userWon)
    {
        string resultString = "";
        switch (result)
        {
            case CheckType.Checkmate:
                resultString = userWon ? "You Win by Checkmate!" : "You Lose by Checkmate!";
                break;
            case CheckType.Stalemate:
                resultString = "It's a Draw by Stalemate!";
                break;
            case CheckType.None:
                resultString = userWon ? "You Win!" : "You Lose!";
                break;
        }
        resultText.text = resultString;
    }
}