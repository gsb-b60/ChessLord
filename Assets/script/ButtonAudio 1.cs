using UnityEngine;
using UnityEngine.UI;

public class ButtonAudio : MonoBehaviour
{
  
    public AudioClip clickSound; 
    
    private AudioSource audioSource;
    private Button button;

    void Start()
    {
        // Tự động tìm thành phần AudioSource và Button trên nút bấm
        audioSource = GetComponent<AudioSource>();
        button = GetComponent<Button>();

        // Nếu nút bấm chưa có AudioSource, tự động thêm mới luôn
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // Tắt tính năng tự động phát lúc mới mở game
        audioSource.playOnAwake = false; 

        // Gắn sự kiện: Khi click vào nút thì chạy hàm PlaySound
        if (button != null)
        {
            button.onClick.AddListener(PlaySound);
        }
    }

    void PlaySound()
    {
        if (clickSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(clickSound);
        }
    }
}