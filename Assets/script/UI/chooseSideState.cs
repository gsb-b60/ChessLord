using UnityEngine;
using UnityEngine.Rendering;

public class chooseSideState : MonoBehaviour
{
    public GameObject chooseWhite;
    public GameObject chooseBlack;
    public GameObject random;

    //public GameObject selectedBorder;

    public int selectedSide = 1;

    public RectTransform selectedBorder;

    void MoveBorder(RectTransform target)
    {
        selectedBorder.SetParent(target);
        selectedBorder.anchoredPosition = Vector2.zero;

        float scaleFactor = 1.1f;
        selectedBorder.sizeDelta = target.sizeDelta * scaleFactor;
    }
    public void chooseWhiteEvent()
    {
        // GameManager.instance.isWhite = true;
        // GameManager.instance.StartGame();
        Debug.Log("choose white");
        selectedSide = 1;
        MoveBorder(chooseWhite.GetComponent<RectTransform>());
    }
    public void chooseBlackEvent()
    {
        // GameManager.instance.isWhite = false;
        // GameManager.instance.StartGame();
        Debug.Log("choose black");
        selectedSide = -1;
        MoveBorder(chooseBlack.GetComponent<RectTransform>());
    }
    public void chooseRandomEvent()
    {
        // GameManager.instance.isWhite = Random.value > 0.5f;
        // GameManager.instance.StartGame();
        Debug.Log("choose random");
        selectedSide = 0;
        MoveBorder(random.GetComponent<RectTransform>());
    }
}