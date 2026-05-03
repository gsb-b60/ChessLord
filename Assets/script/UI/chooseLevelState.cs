using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChooseLevelState : MonoBehaviour
{
    public GameObject nextBtn;
    public GameObject backBtn;
    public Slider slider;
    public TMP_Text levelText;


    public int selectedLevel = 0;

    public void nextBtnEvent()
    {
        Debug.Log("next level");
        slider.value = Mathf.Min(slider.value + 1, slider.maxValue);
    }
    void Awake()
    {
        // Basic null protection
        if (slider == null) Debug.LogError("Slider not assigned");
        if (levelText == null) Debug.LogError("LevelText not assigned");
        UpdateText();
    }


    public void backBtnEvent()
    {
        Debug.Log("back to choose side");
        slider.value = Mathf.Max(slider.value - 1, slider.minValue);
    }
    public void silderEvent()
    {
        UpdateText();
        selectedLevel= (int)slider.value;
    }
    void UpdateText()
    {
        if (levelText == null || slider == null) return;

        levelText.text = "Level: " + slider.value.ToString("0");
    }


}