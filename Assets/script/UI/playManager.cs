using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class playManager : MonoBehaviour
{
    public ChooseLevelState level;
    public chooseSideState side;
    
    // Tạo một ô để ông nhét cái Loa vào
    public AudioSource myAudio; 
   
    void Start() { }
    void Update() { }

    public void playEvent()
    {
        Debug.Log("play with level " + level.selectedLevel + " and side " + side.selectedSide);
        GameData.selectedLevel = level.selectedLevel;
        GameData.selectedSide = side.selectedSide;

        // Bắt đầu quy trình: Kêu beep -> Đợi -> Chuyển Scene
        StartCoroutine(PlaySoundAndLoadScene());
    }

    IEnumerator PlaySoundAndLoadScene()
    {
        // 1. Nếu có gắn Loa thì bật Loa lên
        if (myAudio != null)
        {
            myAudio.Play();
            
            // 2. Tự động lấy độ dài của file âm thanh (ví dụ 0.3s) và bắt Unity đứng đợi đúng chừng đó thời gian
            yield return new WaitForSeconds(myAudio.clip.length);
        }
        else
        {
            // Dự phòng lỡ ông quên gắn loa thì nó vẫn đợi nửa giây rồi chuyển
            yield return new WaitForSeconds(0.5f); 
        }

        // 3. Đợi tiếng kêu xong xuôi hết rồi mới load Scene mới
        SceneManager.LoadScene(1);
    }
}