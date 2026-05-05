using TMPro;
using UnityEngine;

public class ListMoveScript : MonoBehaviour
{
    public TMP_Text moveNumber;
    public TMP_Text whiteMoveDisplay;
    public TMP_Text blackMoveDisplay;


    public void displayListMove(int moveNum, string whiteMove)
    {
        moveNumber.text = moveNum.ToString();
        whiteMoveDisplay.text = whiteMove;
        
        
    }
    public void displayBlackMove(string blackMove)
    {
        blackMoveDisplay.text = blackMove;
    }
}