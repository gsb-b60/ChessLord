using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class playManager : MonoBehaviour
{
    public ChooseLevelState level;
    public chooseSideState side;
    
    [Header("UI Cài đặt")]
    public GameObject modeSelectionGroup; // Nhóm 2 nút PvP/Bot
    public GameObject levelSelectionPanel; // Cái cụm chooseLevel
    public GameObject playButton; // Nút Play Now
    
    // --- BIẾN MỚI: ĐỂ ĐIỀU KHIỂN NÚT QUAY LẠI ---
    public GameObject backButton; 
    
    public AudioSource myAudio; 
    public static bool isPvPMode = false; 

    void Start() 
    {
        // Mới vào game: Chỉ hiện 2 nút chọn chế độ
        if (modeSelectionGroup != null) modeSelectionGroup.SetActive(true);
        
        // Ẩn sạch mấy thứ còn lại cho gọn màn hình
        if (playButton != null) playButton.SetActive(false);
        if (levelSelectionPanel != null) levelSelectionPanel.SetActive(false);
        if (backButton != null) backButton.SetActive(false); // Mới vào thì ẩn nút quay lại đi
    }

    public void ChoosePvPMode()
    {
        isPvPMode = true; 
        if (modeSelectionGroup != null) modeSelectionGroup.SetActive(false);
        if (levelSelectionPanel != null) levelSelectionPanel.SetActive(false); 
        if (playButton != null) playButton.SetActive(true); 
        
        if (backButton != null) backButton.SetActive(true); // Hiện nút quay lại để người ta đổi ý
        Debug.Log("Đã chọn PvP");
    }

    public void ChooseBotMode()
    {
        isPvPMode = false; 
        if (modeSelectionGroup != null) modeSelectionGroup.SetActive(false);
        if (levelSelectionPanel != null) levelSelectionPanel.SetActive(true); 
        if (playButton != null) playButton.SetActive(true); 
        
        if (backButton != null) backButton.SetActive(true); // Hiện nút quay lại để người ta đổi ý
        Debug.Log("Đã chọn Bot");
    }

    // ========================================================
    // --- HÀM MỚI: QUAY LẠI MÀN HÌNH CHỌN CHẾ ĐỘ ---
    // ========================================================
    public void BackToModeSelection()
    {
        if (modeSelectionGroup != null) modeSelectionGroup.SetActive(true); // Hiện lại 2 nút PvP/Bot
        
        // Ẩn hết mấy cái giao diện chọn sau đi
        if (levelSelectionPanel != null) levelSelectionPanel.SetActive(false);
        if (playButton != null) playButton.SetActive(false);
        if (backButton != null) backButton.SetActive(false); // Ẩn chính nó luôn
        
        Debug.Log("Đã quay xe về màn hình chọn chế độ chơi gốc!");
    }

    public void playEvent()
    {
        GameData.selectedLevel = level.selectedLevel;
        GameData.selectedSide = side.selectedSide;
        if (isPvPMode)
        {
            // Tìm cổ thằng trùm cuối lúc nó đang trốn ở Menu
            GameObject trumCuoi = GameObject.Find("ChessEngineManager");
            if (trumCuoi != null)
            {
                Destroy(trumCuoi); // Xé vé, đuổi cổ nó khỏi game
                Debug.Log(">>> PVP MODE: Đã tiêu diệt ChessEngineManager từ trứng nước! <<<");
            }
        }
        StartCoroutine(PlaySoundAndLoadScene());
    }

    IEnumerator PlaySoundAndLoadScene()
    {
        if (myAudio != null)
        {
            myAudio.Play();
            yield return new WaitForSeconds(myAudio.clip.length);
        }
        else yield return new WaitForSeconds(0.5f); 

        SceneManager.LoadScene(1);
    }
}