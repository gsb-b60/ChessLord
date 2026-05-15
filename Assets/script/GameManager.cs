using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using NUnit.Framework;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public enum PieceType
{
    None,
    Queen,
    Rook,
    Bishop,
    Knight,
    Pawn
}
public enum CheckType
{
    None,
    Check,
    Checkmate,
    Stalemate
}
public class GameManage : MonoBehaviour
{
    //ui state variables
    private bool isPieceSelected = false;
    private PieceView selectedPiece;

    private List<GameObject> activeDots = new List<GameObject>();


    private bool gameTurnWhite = true;


    public static GameManage instance;
    public Board board;
    private PieceView[,] pieceViews;

    public GameObject pawn_white;
    public GameObject pawn_black;
    public GameObject rook_white;
    public GameObject rook_black;
    public GameObject knight_white;
    public GameObject knight_black;
    public GameObject bishop_white;
    public GameObject bishop_black;
    public GameObject queen_white;
    public GameObject queen_black;
    public GameObject king_white;
    public GameObject king_black;


    public GameObject dot;
    public GameObject attack_high_light;

    public GameObject piecesParent;


    public Sprite blackBishop;
    public Sprite whiteBishop;
    public Sprite blackKnight;
    public Sprite whiteKnight;
    public Sprite blackRook;
    public Sprite whiteRook;
    public Sprite blackQueen;
    public Sprite whiteQueen;
    public Sprite blackPawn;
    public Sprite whitePawn;

    public GameObject userCaptureContainer;
    public GameObject computerCaptureContainer;

    public GameObject listMovePanel;
    public GameObject moveOddPrefab;
    public GameObject moveEvenPrefab;

    public GameObject lastMovePrefab;

    public GameObject gameMatchPanel;

    List<Piece> capturedWhitePieces = new List<Piece>();
    List<Piece> capturedBlackPieces = new List<Piece>();

    List<Move> moveHistory = new List<Move>();
    List<Piece[,]> boardHistory = new List<Piece[,]>();
    List<bool> turnHistory = new List<bool>();
    List<Piece> piecesCurrentlyChecking = new List<Piece>();

    List<PieceView> pieceOnBoard = new List<PieceView>();

    public PromoteMenu promoteMenu;

    public Piece pawnPromte;
    public Move promoteMove;
    public ScrollRect moveListScroll;
    private GameObject currentListingMove;




    List<GameObject> activeGameObjects = new List<GameObject>();
    List<GameObject> activeMoveHighlight = new List<GameObject>();

    bool isPlayerWhite;
    public void EndingGame(CheckType result = CheckType.None, bool userWon = false)
    {
        Debug.Log("Ending game with result: " + result + ", userWon: " + userWon);
        gameMatchPanel.SetActive(true);
        gameMatchPanel.GetComponent<GameMatchScript>().SetResultText(result, userWon);
    }
    public void QuitGame()
    {
        Application.Quit();
    }
    public void PlayAgain()
    {
        GameData.changeSide();
        Debug.Log("Selected side after change: " + GameData.selectedSide);
        reStartBoard();
    }
    public void getEngineMove(string fen)
    {
        // You don't need a reference, just call the Instance
        StockfishManager.Instance.SendCommand($"position fen {fen}");
        StockfishManager.Instance.SendCommand("go depth 12");
    }
    public void reStartBoard()
    {

        isPlayerWhite = GameData.getPlayerSide();
        Debug.Log("Player is white: " + isPlayerWhite);


        gameMatchPanel.SetActive(false);
        activeGameObjects.ForEach(dot => Destroy(dot));
        activeGameObjects.Clear();
        activeMoveHighlight.ForEach(dot => Destroy(dot));
        activeMoveHighlight.Clear();
        board = new Board();
        pieceViews = new PieceView[board.boardSize, board.boardSize];
        Debug.Log(pieceViews != null ? "pieceViews initialized successfully" : "Failed to initialize pieceViews");
        moveHistory.Clear();
        activeGameObjects.ForEach(dot => Destroy(dot));
        activeGameObjects.Clear();
        gameTurnWhite = true;


        board.board = new Piece[board.boardSize, board.boardSize];
        board.whitePieces = new System.Collections.Generic.List<Piece> {
                new Rook(){isWhite=true},
                new Knight(){isWhite=true},
                new Bishop(){isWhite=true},
                new Queen(){isWhite=true},
                new King(){isWhite=true},
                new Bishop(){isWhite=true},
                new Knight(){isWhite=true},
                new Rook(){isWhite=true}
            };
        board.blackPieces = new System.Collections.Generic.List<Piece> {
            new Rook(){isWhite=false},
            new Knight(){isWhite=false},
            new Bishop(){isWhite=false},
            new Queen(){isWhite=false},
            new King(){isWhite=false},
            new Bishop(){isWhite=false},
            new Knight(){isWhite=false},
            new Rook(){isWhite=false}
        };
        board.pieces = new System.Collections.Generic.List<Piece>();
        board.pieces.AddRange(board.whitePieces);
        board.pieces.AddRange(board.blackPieces);

        for (int i = 0; i < board.boardSize; i++)
        {
            board.board[i, 1] = new Pawn() { isWhite = true };
            board.board[i, 6] = new Pawn() { isWhite = false };
        }
        for (int i = 0; i < board.whitePieces.Count; i++)
        {
            board.board[i, 0] = board.whitePieces[i];
            board.board[i, 7] = board.blackPieces[i];
        }
        displayBoard(board);
    }
    private void Awake()
    {
        instance = this;
        reStartBoard();

    }
    private void clearDots()
    {
        activeDots.ForEach(dot => Destroy(dot));
        activeDots.Clear();
    }
    public void onPieceClicked(PieceView piece)
    {
        //promoteMenu.OpenPromoteCanvas(true);
        if (isPieceSelected)
        {
            clearDots();
        }
        if (selectedPiece != null && selectedPiece.id == piece.id)
        {
            isPieceSelected = false;
            selectedPiece = null;
            clearDots();
            return;
        }

        if (gameTurnWhite != piece.pieceData.isWhite)
        {
            return;
        }
        isPieceSelected = true;
        selectedPiece = piece;

        Vector2Int pos = piece.position;
        List<Move> validMoves = piece.pieceData.getAllValidMoves(board, pos.x, pos.y);
        //Debug.Log("Found " + validMoves.Count + " valid moves for piece at (" + pos.x + "," + pos.y + ")");     
        foreach (Move move in validMoves)
        {
            //Debug.Log("Valid move: (" + move.toX + "," + move.toY + ")");
            if (simulateMoveAndCheckSafety(piece.pieceData, move))
            {
                AddDot(move, false, false, false);
            }

        }

        List<Move> attackMoves = piece.pieceData.getAttackMove(board, pos.x, pos.y);
        //Debug.Log("Found " + attackMoves.Count + " attack moves for piece at (" + pos.x + "," + pos.y + ")");        
        foreach (Move move in attackMoves)
        {
            if (simulateMoveAndCheckSafety(piece.pieceData, move))
            {
                AddDot(move, false, false, true);
            }

        }

        List<Move> enpassantMoves = getEnpassantMoves(piece);
        //Debug.Log("Found " + attackMoves.Count + " attack moves for piece at (" + pos.x + "," + pos.y + ")");        
        foreach (Move move in enpassantMoves)
        {
            if (simulateMoveAndCheckSafety(piece.pieceData, move))
            {
                AddDot(move, false, true, true);
            }

        }
        addCastleMove(piece);
        //addEnpassantMove(piece);
    }

    public void makeEngineMove(Move move)
    {
        Debug.Log(move.ToString());

        movePiecesByEngineLogic(move);

        moveHistory.Add(move);


        isPieceSelected = false;
        clearDots();

        gameTurnWhite = !gameTurnWhite;
        moveHistory.Last().checkType = !checkKingSafety(gameTurnWhite, board) ? CheckType.Check : CheckType.None;

        displayKingInCheck();
        checkEndGame();
        displayMovedPiece(moveHistory.Last());
        displayListMove();
        ExportFEN();
    }
    private void movePiecesByEngineLogic(Move move)
    {
        PieceView piece = pieceViews[move.fromX, move.fromY];

        if (piece == null)
        {
            Debug.LogError($"No piece found at {move.fromX}, {move.fromY}");
            return;
        }

        // 2. Update Physical/Visual Position
        Vector2 spawnPos = new Vector2(changeXVector(move.toX), changeYVector(move.toY));
        piece.transform.position = spawnPos;

        // 3. Update Backend Logic (Data Board)
        board.board[move.fromX, move.fromY].hasMoved = true;
        board.board[move.toX, move.toY] = piece.pieceData;
        board.board[move.fromX, move.fromY] = null; // Cleaned up to use move.from

        // 4. Update View Tracking (UI/Reference Board)
        pieceViews[move.toX, move.toY] = piece;
        pieceViews[move.fromX, move.fromY] = null;

        // 5. Update Piece Internal State
        piece.position = new Vector2Int(move.toX, move.toY);
        piece.pieceData.hasMoved = true;

        // 6. Handle Special Rules (Promotion)
        // if (piece.pieceData is Pawn)
        // {
        //     bool isWhitePromotion = piece.pieceData.isWhite && move.toY == 7;
        //     bool isBlackPromotion = !piece.pieceData.isWhite && move.toY == 0;

        //     if (isWhitePromotion || isBlackPromotion)
        //     {
        //         // Note: I used 'piece' here instead of 'selectedPiece' 
        //         // to ensure the promotion happens to the correct object.
        //         piece.position = new Vector2Int(move.toX, move.toY);
        //         promoteMenu.OpenPromoteCanvas(piece.pieceData.isWhite);

        //         pawnPromte = piece.pieceData;
        //         promoteMove = move;
        //     }
        // }
    }

    private List<Move> getAllPossibleMoves(PieceView piece)
    {
        List<Move> allMoves = new List<Move>();
        Vector2Int pos = piece.position;
        List<Move> validMoves = piece.pieceData.getAllValidMoves(board, pos.x, pos.y);
        //Debug.Log("Found " + validMoves.Count + " valid moves for piece at (" + pos.x + "," + pos.y + ")");     
        foreach (Move move in validMoves)
        {
            //Debug.Log("Valid move: (" + move.toX + "," + move.toY + ")");
            if (simulateMoveAndCheckSafety(piece.pieceData, move))
            {
                // AddDot(move, false, false, false);
                allMoves.Add(move);
            }

        }

        List<Move> attackMoves = piece.pieceData.getAttackMove(board, pos.x, pos.y);
        //Debug.Log("Found " + attackMoves.Count + " attack moves for piece at (" + pos.x + "," + pos.y + ")");        
        foreach (Move move in attackMoves)
        {
            if (simulateMoveAndCheckSafety(piece.pieceData, move))
            {
                //AddDot(move, false, false, true);
                allMoves.Add(move);


            }

        }

        List<Move> enpassantMoves = getEnpassantMoves(piece);
        //Debug.Log("Found " + attackMoves.Count + " attack moves for piece at (" + pos.x + "," + pos.y + ")");        
        foreach (Move move in enpassantMoves)
        {
            if (simulateMoveAndCheckSafety(piece.pieceData, move))
            {
                //AddDot(move, false, true, false);
                allMoves.Add(move);

            }

        }
        return allMoves;

    }
    private void checkEndGame()
    {
        int possibleMoveCount = 0;
        int thisSidePieceCount = 0;
        int opponentPieceCount = 0;

        foreach (PieceView piece in pieceOnBoard)
        {
            if (piece != null && piece.pieceData.isWhite == gameTurnWhite)
            {
                List<Move> moves = getAllPossibleMoves(piece);
                if (moves.Count > 0)
                {
                    possibleMoveCount += moves.Count;
                }
                thisSidePieceCount++;
            }

        }

        if (possibleMoveCount == 0)
        {

            if (!checkKingSafety(gameTurnWhite, board))
            {
                Debug.Log("Checkmate! " + (gameTurnWhite ? "Black" : "White") + " wins!");

                moveHistory.Last().checkType = CheckType.Checkmate;
                Debug.Log("is player white: " + isPlayerWhite);
                Debug.Log("game turn white: " + gameTurnWhite);
                EndingGame(CheckType.Checkmate, isPlayerWhite != gameTurnWhite);
            }
            else
            {
                Debug.Log("Stalemate! It's a draw!");
                moveHistory.Last().checkType = CheckType.Stalemate;
                EndingGame(CheckType.Stalemate);
            }
        }
        else
        {
            if (moveHistory.Count >= 50)
            {
                bool isDraw = moveHistory
                .TakeLast(50)
                .All(m => !m.isAttack && !(m.pieceView.pieceData is Pawn));
                if (isDraw)
                {
                    Debug.Log("Draw by 50-move rule!");

                    moveHistory.Last().checkType = CheckType.Stalemate;
                    EndingGame(CheckType.Stalemate);
                }
            }
            if (thisSidePieceCount == 1 || opponentPieceCount == 1)
            {
                opponentPieceCount = pieceOnBoard.Count - thisSidePieceCount;
                if (opponentPieceCount <= 3)
                {
                    Debug.Log("Draw by insufficient material!");

                    moveHistory.Last().checkType = CheckType.Stalemate;
                    EndingGame(CheckType.Stalemate);
                }
            }



        }
    }


    private void addEnpassantMove(PieceView piece)
    {
        List<Move> enpassantMoves = getEnpassantMoves(piece);



        foreach (Move move in enpassantMoves)
        {
            if (simulateMoveAndCheckSafety(piece.pieceData, move))
            {
                AddDot(move, false, true, true);
            }
        }


    }
    private List<Move> getEnpassantMoves(PieceView piece)
    {
        List<Move> enpassantMoves = new List<Move>();
        if (piece.pieceData is Pawn)
        {
            Debug.Log("get in 1");
            Move lastMove;

            if (moveHistory.Count == 0)
                return new List<Move>();

            Debug.Log("get in 2");

            lastMove = moveHistory[^1];

            if (!(lastMove.isPawnLongMove &&
                  lastMove.pieceView.pieceData is Pawn pawn &&
                  pawn.isWhite != piece.pieceData.isWhite))
                return new List<Move>();

            Debug.Log("yes to all");

            int x = piece.position.x;
            int y = piece.position.y;

            int passantY = lastMove.toY;
            int passantX = lastMove.toX;

            if (piece.pieceData.isWhite)
            {
                if (y == passantY)
                {
                    if (x + 1 == passantX)
                    {
                        // Handle en passant move
                        //AddDot(new Move(x, y, x + 1, y + 1), false, true, false);
                        enpassantMoves.Add(new Move(x, y, x + 1, y + 1));

                    }
                    else if (x - 1 == passantX)
                    {
                        // Handle en passant move
                        //AddDot(new Move(x, y, x - 1, y + 1), false, true, false);
                        enpassantMoves.Add(new Move(x, y, x - 1, y + 1));
                    }
                }
                else
                {
                    return new List<Move>();
                }
            }
            else
            {
                if (y == passantY)
                {
                    if (x + 1 == passantX)
                    {
                        // Handle en passant move
                        //AddDot(new Move(x, y, x + 1, y - 1), false, true, false);
                        enpassantMoves.Add(new Move(x, y, x + 1, y - 1));
                    }
                    else if (x - 1 == passantX)
                    {
                        // Handle en passant move
                        enpassantMoves.Add(new Move(x, y, x - 1, y - 1));
                        //AddDot(new Move(x, y, x - 1, y - 1), false, true, false);
                    }
                }
                else
                {
                    return new List<Move>();
                }
            }
        }
        return enpassantMoves;
    }

    private void addCastleMove(PieceView piece)
    {
        if (piece.pieceData is King && !piece.pieceData.hasMoved)
        {
            //Debug.Log("is king");
            if (piece.pieceData.isWhite)
            {
                //Debug.Log("is white");
                if (board.board[0, 0] != null && board.board[0, 0] is Rook && !board.board[0, 0].hasMoved)
                {
                    bool haveBlock = false;
                    for (int i = 1; i < 3; i++)
                    {
                        if (board.board[i, 0] != null)
                        {
                            haveBlock = true;
                            break;
                        }

                    }
                    if (!haveBlock)
                    {
                        if (simulateMoveAndCheckSafety(piece.pieceData, new Move(piece.position.x, piece.position.y, 2, 0) { isCastle = true }))
                        {
                            AddDot(new Move(piece.position.x, piece.position.y, 2, 0) { isCastle = true }, true, false, false);
                        }
                    }
                }
                if (board.board[7, 0] != null && board.board[7, 0] is Rook && !board.board[7, 0].hasMoved)
                {
                    bool haveBlock = false;
                    for (int i = 5; i <= 6; i++)
                    {
                        if (board.board[i, 0] != null)
                        {
                            haveBlock = true;
                            break;
                        }
                    }
                    if (!haveBlock)
                    {
                        if (simulateMoveAndCheckSafety(piece.pieceData, new Move(piece.position.x, piece.position.y, 6, 0) { isCastle = true }))
                        {
                            AddDot(new Move(piece.position.x, piece.position.y, 6, 0) { isCastle = true }, true, false, false);
                        }
                    }
                }
            }
            else
            {
                //Debug.Log("is black");
                if (board.board[0, 7] != null && board.board[0, 7] is Rook && !board.board[0, 7].hasMoved)
                {
                    bool haveBlock = false;
                    for (int i = 1; i < 3; i++)
                    {
                        if (board.board[i, 7] != null)
                        {
                            haveBlock = true;
                            break;
                        }

                    }
                    if (!haveBlock)
                    {
                        if (simulateMoveAndCheckSafety(piece.pieceData, new Move(piece.position.x, piece.position.y, 2, 7) { isCastle = true }))
                        {
                            AddDot(new Move(piece.position.x, piece.position.y, 2, 7) { isCastle = true }, true, false, false);
                        }
                    }
                }
                if (board.board[7, 7] != null && board.board[7, 7] is Rook && !board.board[7, 7].hasMoved)
                {
                    bool haveBlock = false;
                    for (int i = 5; i <= 6; i++)
                    {
                        if (board.board[i, 7] != null)
                        {
                            haveBlock = true;
                            break;
                        }
                    }
                    if (!haveBlock)
                    {
                        if (simulateMoveAndCheckSafety(piece.pieceData, new Move(piece.position.x, piece.position.y, 6, 7) { isCastle = true }))
                        {
                            AddDot(new Move(piece.position.x, piece.position.y, 6, 7) { isCastle = true }, true, false, false);
                        }
                    }
                }
            }
        }
    }
    private void AddDot(Move move, bool isCastle, bool isEnpassant, bool isAttack)
    {
        Vector2 spawnPos = new Vector2(changeXVector(move.toX), changeYVector(move.toY));
        if (!isAttack)
        {

            GameObject dotObject = Instantiate(dot, spawnPos, Quaternion.identity);
            SuggestDot dotScript = dotObject.GetComponent<SuggestDot>();
            dotScript.position = new Vector2Int(move.toX, move.toY);
            dotScript.isAttackMove = isAttack;
            dotScript.isCastleMove = isCastle;
            dotScript.isEnPassant = isEnpassant;

            dotScript.move = move;

            activeDots.Add(dotObject);
        }
        else
        {
            GameObject dotObject = Instantiate(attack_high_light, spawnPos, Quaternion.identity);
            SuggestDot dotScript = dotObject.GetComponent<SuggestDot>();
            dotScript.position = new Vector2Int(move.toX, move.toY);
            dotScript.isAttackMove = isAttack;
            dotScript.isCastleMove = isCastle;
            dotScript.isEnPassant = isEnpassant;
            dotScript.move = move;
            activeDots.Add(dotObject);
        }
    }




    // private void ListingMove(PieceView piece)
    // {
    //     Vector2Int pos = piece.position;
    //     List<Move> validMoves = piece.pieceData.getAllValidMoves(board, pos.x, pos.y);
    //     //Debug.Log("Found " + validMoves.Count + " valid moves for piece at (" + pos.x + "," + pos.y + ")");     
    //     foreach (Move move in validMoves)
    //     {
    //         AddDot(move, false, false, false);
    //     }

    //     List<Move> attackMoves = piece.pieceData.getAttackMove(board, pos.x, pos.y);
    //     Debug.Log("Found " + attackMoves.Count + " attack moves for piece at (" + pos.x + "," + pos.y + ")");
    //     foreach (Move move in attackMoves)
    //     {
    //         AddDot(move, false, false, true);
    //     }

    // }

    private bool checkKingSafety(bool isWhite, Board checkBoard)
    {
        //piecesCurrentlyChecking= new List<Piece>();
        // Debug.Log("check safety");
        //  checkBoard.PrintBoard();
        for (int i = 0; i < checkBoard.boardSize; i++)
        {
            for (int j = 0; j < checkBoard.boardSize; j++)
            {
                Piece currentPiece = checkBoard.board[i, j];
                // Debug.Log("the piece want to move is "+currentPiece.getType());
                if (currentPiece != null && currentPiece.isWhite != isWhite)
                {
                    List<Move> attackMoves = currentPiece.getAttackMove(checkBoard, i, j);
                    foreach (Move attackMove in attackMoves)
                    {
                        if (checkBoard.board[attackMove.toX, attackMove.toY] is King)
                        {
                            return false;
                        }
                    }
                }
            }
        }

        return true;
    }

    private bool simulateMoveAndCheckSafety(Piece piece, Move move)
    {

        Piece[,] virtualBoard = Board.DeepCopyBoard(board.board);

        virtualBoard[move.toX, move.toY] = piece;
        virtualBoard[move.fromX, move.fromY] = null;
        // Debug.Log("now check with simulator");
        if (checkKingSafety(gameTurnWhite, new Board { board = virtualBoard }))
        {
            return true;
        }
        // Debug.Log("Move from (" + move.fromX + "," + move.fromY + ") to (" + move.toX + "," + move.toY + ") would put king in check. Move is not safe.");


        return false;
    }
    private void appendCapturedPiece(Piece piece)
    {
        if (piece.isWhite)
        {
            capturedWhitePieces.Add(piece);
        }
        else
        {
            capturedBlackPieces.Add(piece);
        }
        Transform targetContainer = GetTargetContainer(piece);

        // 3. Tạo UI
        CreateCapturedUI(piece, targetContainer);

    }
    private Transform GetTargetContainer(Piece piece)
    {
        bool isPlayerPiece = (piece.isWhite == isPlayerWhite);
        return isPlayerPiece
            ? computerCaptureContainer.transform
            : userCaptureContainer.transform;
    }
    private void CreateCapturedUI(Piece piece, Transform container)
    {
        GameObject obj = new GameObject("CapturedPiece");
        obj.transform.SetParent(container, false);

        Image img = obj.AddComponent<Image>();
        img.sprite = getSprite(piece);
        img.preserveAspect = true;

        RectTransform rt = obj.GetComponent<RectTransform>();
        rt.sizeDelta = new Vector2(50, 50);
    }
    private Sprite getSprite(Piece piece)
    {
        if (piece is Pawn)
        {
            return piece.isWhite ? whitePawn : blackPawn;
        }
        else if (piece is Rook)
        {
            return piece.isWhite ? whiteRook : blackRook;
        }
        else if (piece is Knight)
        {
            return piece.isWhite ? whiteKnight : blackKnight;
        }
        else if (piece is Bishop)
        {
            return piece.isWhite ? whiteBishop : blackBishop;
        }
        else if (piece is Queen)
        {
            return piece.isWhite ? whiteQueen : blackQueen;
        }
        return null;
    }
    public void displayListMove()
    {
        if (!gameTurnWhite)
        {

            currentListingMove = null;
            int orderOfMove = moveHistory.Count / 2 + 1;
            string textMove = moveHistory.Last().ToString();
            if ((moveHistory.Count / 2) % 2 == 0)
            {
                currentListingMove = Instantiate(moveEvenPrefab, listMovePanel.transform);
            }
            else
            {
                currentListingMove = Instantiate(moveOddPrefab, listMovePanel.transform);

            }
            currentListingMove.SetActive(true);
            currentListingMove.GetComponent<ListMoveScript>().displayListMove(orderOfMove, textMove);

            Canvas.ForceUpdateCanvases();
            moveListScroll.verticalNormalizedPosition = 0f;
        }
        else
        {
            currentListingMove.GetComponent<ListMoveScript>().displayBlackMove(moveHistory.Last().ToString());
        }

    }



    public void onDotClicked(SuggestDot dot)
    {
        if (!isPieceSelected) return;

        Vector2Int from = selectedPiece.position;
        Vector2Int to = dot.position;

        Move move = new Move(from.x, from.y, to.x, to.y);


        Move originalMove = dot.move;

        originalMove.pieceView = pieceViews[from.x, from.y];

        // displayListMove();

        if (dot.isAttackMove)
        {

            // Debug.Log($"Performing attack move... {from.x},{from.y} to {to.x},{to.y}");
            // Debug.Log("Attack move from (" + from.x + "," + from.y + ") to (" + to.x + "," + to.y + ")");
            PieceView targetPiece = pieceViews[to.x, to.y];
            // Debug.Log("Target piece at (" + to.x + "," + to.y + "): " + targetPiece.pieceData.GetType().Name);
            if (targetPiece != null)
            {
                //Debug.Log("Attacking piece at (" + to.x + "," + to.y + "): " + targetPiece.pieceData.GetType().Name);
                appendCapturedPiece(targetPiece.pieceData);
                Destroy(targetPiece.gameObject);
                board.board[to.x, to.y] = null;
                pieceOnBoard.Remove(targetPiece);

            }
            originalMove.isAttack = true;
        }
        if (dot.isCastleMove)
        {
            if (selectedPiece.pieceData.isWhite)
            {
                MovePiece(selectedPiece, move);
                if (move.toX == 2)
                {
                    Debug.Log("Castle move: Moving rook from (0," + move.toY + ") to (3," + move.toY + ")");
                    MovePiece(pieceViews[0, move.toY], new Move(0, 0, move.toX + 1, move.toY));
                }
                if (move.toX == 6)
                {
                    Debug.Log("Castle move: Moving rook from (7," + move.toY + ") to (5," + move.toY + ")");
                    MovePiece(pieceViews[7, move.toY], new Move(0, 0, move.toX - 1, move.toY));
                }

            }
            else
            {
                MovePiece(selectedPiece, move);
                if (move.toX == 2)
                {
                    MovePiece(pieceViews[0, move.toY], new Move(0, 0, move.toX + 1, move.toY));
                }
                if (move.toX == 6)
                {
                    MovePiece(pieceViews[7, move.toY], new Move(0, 0, move.toX - 1, move.toY));
                }
            }

        }
        if (dot.isEnPassant)
        {
            MovePiece(selectedPiece, move);
            if (move.toX < move.fromX)
            {
                PieceView targetPiece = pieceViews[from.x - 1, from.y];
                if (targetPiece != null)
                {
                    Destroy(targetPiece.gameObject);
                    board.board[from.x - 1, from.y] = null;
                }
            }
            else
            {
                PieceView targetPiece = pieceViews[from.x + 1, from.y];
                if (targetPiece != null)
                {
                    Destroy(targetPiece.gameObject);
                    board.board[from.x + 1, from.y] = null;
                }
            }
        }
        else
        {
            MovePiece(selectedPiece, move);
        }


        moveHistory.Add(originalMove);


        isPieceSelected = false;
        clearDots();

        gameTurnWhite = !gameTurnWhite;
        moveHistory.Last().checkType = !checkKingSafety(gameTurnWhite, board) ? CheckType.Check : CheckType.None;

        displayKingInCheck();
        checkEndGame();
        displayMovedPiece(moveHistory.Last());
        displayListMove();
        ExportFEN();

    }



    private void ExportFEN()
    {

        string boardPosition = "";

        for (int i = 0; i < board.boardSize; i++)
        {
            int emptyCount = 0;
            for (int j = 0; j < board.boardSize; j++)
            {
                Piece piece = board.board[j, board.boardSize - 1 - i];
                if (piece == null)
                {
                    emptyCount++;
                }
                else
                {
                    if (emptyCount > 0)
                    {
                        boardPosition += emptyCount.ToString();
                        emptyCount = 0;
                    }
                    boardPosition += GameData.getFenChar(piece);
                }
            }
            if (emptyCount > 0)
            {
                boardPosition += emptyCount.ToString();
            }
            if (i < board.boardSize - 1)
            {
                boardPosition += "/";
            }
        }
        // Debug.Log("FEN: " + boardPosition);
        // Debug.Log("Active color: " + (gameTurnWhite ? "w" : "b"));




        string castlelingRights = "";
        if (board.board[4, 0] != null && board.board[4, 0] is King whiteKing && !whiteKing.hasMoved)
        {
            Debug.Log("white king havent moved yet");
            if (board.board[7, 0] != null && board.board[7, 0] is Rook rook && !rook.hasMoved)
            {
                castlelingRights += "K";
                Debug.Log("white can castle kingside");
            }
            if (board.board[0, 0] != null && board.board[0, 0] is Rook rookWhite1 && !rookWhite1.hasMoved)
            {
                castlelingRights += "Q";
                Debug.Log("white can castle queenside");
            }
        }


        if (board.board[4, 7] != null && board.board[4, 7] is King king && !king.hasMoved)
        {
            if (board.board[7, 7] != null && board.board[7, 7] is Rook rook && !rook.hasMoved)
            {
                castlelingRights += "k";
            }
            if (board.board[0, 7] != null && board.board[0, 7] is Rook rookblack1 && !rookblack1.hasMoved)
            {
                castlelingRights += "q";
            }
        }
        if (castlelingRights == "")
        {
            castlelingRights = "-";
        }
        Debug.Log("Castling rights: " + castlelingRights);
        string enpassantRight = "-";
        if (moveHistory.Last().isPawnLongMove)
        {
            foreach (PieceView piece in pieceOnBoard)
            {
                List<Move> enMove = getEnpassantMoves(piece);
                if (enMove != null && enMove.Count > 0)
                {
                    enpassantRight = enMove.Last().enPassantFen();
                }

            }
        }

        Debug.Log("enpassant right " + enpassantRight);
        Debug.Log("complete FEN: " + boardPosition + " " + (gameTurnWhite ? "w" : "b") + " " + castlelingRights + " " + enpassantRight + " 0 1");

        string fen =
            boardPosition + " " +
            (gameTurnWhite ? "w" : "b") + " " + castlelingRights + " " +
            enpassantRight + " " +
            "0 1";

        if (gameTurnWhite != isPlayerWhite)
        {
            getEngineMove(fen);
        }

    }
    private void displayKingInCheck()
    {
        foreach (PieceView piece in pieceOnBoard)
        {
            if (piece == null || piece.pieceData is not King)
                continue;

            SpriteRenderer sr = piece.GetComponent<SpriteRenderer>();

            bool isInCheck = !checkKingSafety(piece.pieceData.isWhite, board);
            if (isInCheck)
            {
                Debug.Log("King at (" + piece.position.x + "," + piece.position.y + ") is in check!");
            }

            sr.color = isInCheck ? Color.red : Color.white;
        }
    }

    private void displayMovedPiece(Move move)
    {
        activeMoveHighlight.ForEach(dot => Destroy(dot));
        activeMoveHighlight.Clear();

        Vector2 spawnPos = new Vector2(changeXVector(move.fromX), changeYVector(move.fromY));

        GameObject fromMove = Instantiate(lastMovePrefab, spawnPos, Quaternion.identity);
        activeMoveHighlight.Add(fromMove);

        spawnPos = new Vector2(changeXVector(move.toX), changeYVector(move.toY));
        GameObject toMove = Instantiate(lastMovePrefab, spawnPos, Quaternion.identity);
        activeMoveHighlight.Add(toMove);
    }

    private void MovePiece(PieceView piece, Move move)
    {
        Vector2 spawnPos = new Vector2(changeXVector(move.toX), changeYVector(move.toY));
        piece.transform.position = spawnPos;
        board.board[move.fromX, move.fromY].hasMoved = true;
        board.board[move.toX, move.toY] = piece.pieceData;
        board.board[piece.position.x, piece.position.y] = null;

        pieceViews[move.toX, move.toY] = piece;
        pieceViews[piece.position.x, piece.position.y] = null;
        piece.position = new Vector2Int(move.toX, move.toY);
        piece.pieceData.hasMoved = true;


        if (piece.pieceData is Pawn)
        {
            if ((piece.pieceData.isWhite && move.toY == 7) || (!piece.pieceData.isWhite && move.toY == 0))
            {
                selectedPiece.position = new Vector2Int(move.toX, move.toY);
                promoteMenu.OpenPromoteCanvas(piece.pieceData.isWhite);

                pawnPromte = piece.pieceData;
                promoteMove = move;
            }
        }
    }

    public void callPromote(PieceType promoteTo)
    {
        promotePiece(pawnPromte, promoteTo, promoteMove);
    }

    public void promotePiece(Piece piece, PieceType promoteTo, Move move)
    {
        if (selectedPiece == null || piece.getType() != "p")
        {
            Debug.LogError("Promotion error: No pawn selected for promotion.");
            return;
        }


        int x = move.toX;
        int y = move.toY;
        Destroy(pieceViews[move.toX, move.toY].gameObject);
        Piece newPiece = null;
        switch (promoteTo)
        {
            case PieceType.Queen:
                newPiece = new Queen() { isWhite = piece.isWhite };
                break;
            case PieceType.Rook:
                newPiece = new Rook() { isWhite = piece.isWhite };
                break;
            case PieceType.Bishop:
                newPiece = new Bishop() { isWhite = piece.isWhite };
                break;
            case PieceType.Knight:
                newPiece = new Knight() { isWhite = piece.isWhite };
                break;
        }
        board.board[x, y] = newPiece;
        GameObject obj = Instantiate(getPrefab(newPiece), new Vector2(changeXVector(x), changeYVector(y)), Quaternion.identity);
        PieceView view = obj.GetComponent<PieceView>();
        pieceViews[x, y] = view;
        view.Init(newPiece, x, y);
        moveHistory.Last().promoteTo = promoteTo;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }
    public void displayBoard(Board board)
    {
        for (int i = 0; i < board.boardSize; i++)
        {
            for (int j = 0; j < board.boardSize; j++)
            {
                if (board.board[i, j] != null)
                {
                    //Debug.Log("Piece at position (" + i + "," + j + "): " + board.board[i, j].GetType().Name);

                    Vector2 spawnPos = new Vector2(changeXVector(i), changeYVector(j));
                    //Instantiate(getPrefab(board.board[i, j]), spawnPos, Quaternion.identity);



                    GameObject obj = Instantiate(getPrefab(board.board[i, j]), spawnPos, Quaternion.identity, piecesParent.transform);
                    activeGameObjects.Add(obj);

                    PieceView view = obj.GetComponent<PieceView>();

                    pieceViews[i, j] = view;
                    pieceOnBoard.Add(view);
                    view.id = i * board.boardSize + j;

                    if (view != null)
                    {
                        view.Init(board.board[i, j], i, j);
                    }

                }
            }
        }




    }
    public GameObject getPrefab(Piece piece)
    {
        if (piece is Pawn)
        {
            return piece.isWhite ? pawn_white : pawn_black;
        }
        else if (piece is Rook)
        {
            return piece.isWhite ? rook_white : rook_black;
        }
        else if (piece is Knight)
        {
            return piece.isWhite ? knight_white : knight_black;
        }
        else if (piece is Bishop)
        {
            return piece.isWhite ? bishop_white : bishop_black;
        }
        else if (piece is Queen)
        {
            return piece.isWhite ? queen_white : queen_black;
        }
        else if (piece is King)
        {
            return piece.isWhite ? king_white : king_black;
        }
        return null;
    }
    private int changeXVector(int x)
    {
        if (isPlayerWhite)
        {
            return x - 4;
        }
        else
        {
            return 7 - x - 4;
        }

    }
    private int changeYVector(int y)
    {
        if (isPlayerWhite)
        {
            return y - 3;
        }
        else
        {
            return 7 - y - 3;
        }

    }

    // Update is called once per frame
    void Update()
    {

    }
}
