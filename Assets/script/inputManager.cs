using UnityEngine;
using UnityEngine.InputSystem;

public class inputManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());

            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

            if (hit.collider != null)
            {
                

                PieceView piece = hit.collider.GetComponent<PieceView>();
                if (piece != null)
                {
                    
                    piece.onClick();
                    return;
                }
                SuggestDot dot = hit.collider.GetComponent<SuggestDot>();
                if (dot != null)
                {
                    dot.onClick();
                    Debug.Log("Clicked on dot at position: " + dot.position);
                    return;
                }
            }
            

        }
    }
}
