using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(UIDocument))]
public class PromoteMenu : MonoBehaviour
{
    public VisualTreeAsset uiTemplate;
    private VisualElement promoteContainer;

    public Sprite blackBishop;
    public Sprite whiteBishop;
    public Sprite blackKnight;
    public Sprite whiteKnight;
    public Sprite blackRook;
    public Sprite whiteRook;
    public Sprite blackQueen;
    public Sprite whiteQueen;
    public GameManage gameManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    void OnEnable()
    {

    }
    public void OpenPromoteCanvas(bool isWhite)
    {
        var uiDocument = GetComponent<UIDocument>();

        // Safety check: Does the file exist in the Inspector slot?
        if (uiTemplate == null)
        {
            Debug.LogError("Assign your UXML file to the 'Ui Template' slot in the Inspector!");
            return;
        }

        // Assign the file to the document
        uiDocument.visualTreeAsset = uiTemplate;

        // Give Unity a tiny moment to process the change
        VisualElement root = uiDocument.rootVisualElement;

        root.style.display = DisplayStyle.Flex; 


        promoteContainer = root.Q<VisualElement>("promoteContrainer");

        if (promoteContainer != null)
        {
            createChoices(isWhite);
        }

    }
    private void createChoices(bool isWhite)
    {
        promoteContainer.Clear();
        VisualElement queenImg = new VisualElement();
        queenImg.style.width = 40;
        queenImg.style.height = 40;
        queenImg.style.borderBottomLeftRadius = 10;
        queenImg.style.borderBottomRightRadius = 10;
        queenImg.RegisterCallback<ClickEvent>(evt =>
        {
            Debug.Log("user chose queen!");
            queenImg.style.unityBackgroundImageTintColor = Color.yellow; // Visual feedback
            ClosePromoteCanvas();
            gameManager.callPromote(PieceType.Queen);
        });
        promoteContainer.Add(queenImg);

        VisualElement knightImg = new VisualElement();
        knightImg.style.width = 40;
        knightImg.style.height = 40;
        knightImg.style.borderBottomLeftRadius = 10;
        knightImg.style.borderBottomRightRadius = 10;
        knightImg.RegisterCallback<ClickEvent>(evt =>
        {
            Debug.Log("user chose knight!");
            knightImg.style.unityBackgroundImageTintColor = Color.yellow;
            ClosePromoteCanvas();
            gameManager.callPromote(PieceType.Knight);
        });
        promoteContainer.Add(knightImg);

        VisualElement rookImg = new VisualElement();
        rookImg.style.width = 40;
        rookImg.style.height = 40;
        rookImg.style.borderBottomLeftRadius = 10;
        rookImg.style.borderBottomRightRadius = 10;
        rookImg.RegisterCallback<ClickEvent>(evt =>
        {
            Debug.Log("user chose rook!");
            rookImg.style.unityBackgroundImageTintColor = Color.yellow; 
            ClosePromoteCanvas();
            gameManager.callPromote(PieceType.Rook);
        });
        promoteContainer.Add(rookImg);


        VisualElement bishopImg = new VisualElement();
        bishopImg.style.width = 40;
        bishopImg.style.height = 40;
        bishopImg.style.borderBottomLeftRadius = 10;
        bishopImg.style.borderBottomRightRadius = 10;
        bishopImg.RegisterCallback<ClickEvent>(evt =>
        {
            Debug.Log("user chose bishop!");
            bishopImg.style.unityBackgroundImageTintColor = Color.yellow; 
            ClosePromoteCanvas();
            gameManager.callPromote(PieceType.Bishop);
        });
        promoteContainer.Add(bishopImg);
        if (isWhite)
        {
            knightImg.style.backgroundImage = new StyleBackground(whiteKnight);
            queenImg.style.backgroundImage = new StyleBackground(whiteQueen);
            bishopImg.style.backgroundImage = new StyleBackground(whiteBishop);
            rookImg.style.backgroundImage = new StyleBackground(whiteRook);
        }
        else
        {
            knightImg.style.backgroundImage = new StyleBackground(blackKnight);
            queenImg.style.backgroundImage = new StyleBackground(blackQueen);
            bishopImg.style.backgroundImage = new StyleBackground(blackBishop);
            rookImg.style.backgroundImage = new StyleBackground(blackRook);
        }



    }
    
    public void ClosePromoteCanvas()
    {
        var uiDocument = GetComponent<UIDocument>();

        // Option 1: clear everything
        uiDocument.rootVisualElement.Clear();

        // Option 2 (cleaner): just hide it
        uiDocument.rootVisualElement.style.display = DisplayStyle.None;

        promoteContainer = null;
    }

}
