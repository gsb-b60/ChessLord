using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayManager : MonoBehaviour
{
    public ChooseLevelState level;
    public chooseSideState side;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void playEvent()
    {
        Debug.Log("play with level " + level.selectedLevel + " and side " + side.selectedSide);
        GameData.selectedLevel = level.selectedLevel;
        GameData.selectedSide = side.selectedSide;

        SceneManager.LoadScene(1);

    }
}