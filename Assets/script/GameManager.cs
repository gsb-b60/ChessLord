using System.Collections.Generic;
using System.Security.Principal;
using Unity.VisualScripting;
using UnityEngine;

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

    List<Piece> capturedWhitePieces = new List<Piece>();
    List<Piece> capturedBlackPieces = new List<Piece>();

    List<Move> moveHistory = new List<Move>();
    List<Piece[,]> boardHistory = new List<Piece[,]>();
    List<bool> turnHistory = new List<bool>();
    List<Piece> piecesCurrentlyChecking = new List<Piece>();


    private void Awake()
    {
        instance = this;

        board = new Board();
        pieceViews = new PieceView[board.boardSize, board.boardSize];
        //Debug.Log(pieceViews != null ? "pieceViews initialized successfully" : "Failed to initialize pieceViews");

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
    private void clearDots()
    {
        activeDots.ForEach(dot => Destroy(dot));
        activeDots.Clear();
    }
    public void onPieceClicked(PieceView piece)
    {
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
        if (!checkKingSafety(piece.pieceData, board))
        {
            
            Vector2Int pos = piece.position;
            List<Move> validMoves = piece.pieceData.getAllValidMoves(board, pos.x, pos.y);
            //Debug.Log("Found " + validMoves.Count + " valid moves for piece at (" + pos.x + "," + pos.y + ")");     
            foreach (Move move in validMoves)
            {
                //Debug.Log("Valid move: (" + move.toX + "," + move.toY + ")");
                if (simulateMoveAndCheckSafety(piece.pieceData, move))
                {
                    Vector2 spawnPos = new Vector2(changeXVector(move.toX), changeYVector(move.toY));
                    GameObject dotObject = Instantiate(dot, spawnPos, Quaternion.identity);
                    SuggestDot dotScript = dotObject.GetComponent<SuggestDot>();
                    dotScript.position = new Vector2Int(move.toX, move.toY);
                    dotScript.isAttackMove = false;

                    activeDots.Add(dotObject);
                }

            }

            List<Move> attackMoves = piece.pieceData.getAttackMove(board, pos.x, pos.y);
            //Debug.Log("Found " + attackMoves.Count + " attack moves for piece at (" + pos.x + "," + pos.y + ")");        
            foreach (Move move in attackMoves)
            {
                if (simulateMoveAndCheckSafety(piece.pieceData, move))
                {
                    Vector2 spawnPos = new Vector2(changeXVector(move.toX), changeYVector(move.toY));
                    GameObject dotObject = Instantiate(attack_high_light, spawnPos, Quaternion.identity);
                    SuggestDot dotScript = dotObject.GetComponent<SuggestDot>();
                    dotScript.position = new Vector2Int(move.toX, move.toY);
                    dotScript.isAttackMove = true;
                    activeDots.Add(dotObject);
                }

            }
            return;
        }




        ListingMove(piece);

    }




    private void ListingMove(PieceView piece)
    {
        Vector2Int pos = piece.position;
        List<Move> validMoves = piece.pieceData.getAllValidMoves(board, pos.x, pos.y);
        //Debug.Log("Found " + validMoves.Count + " valid moves for piece at (" + pos.x + "," + pos.y + ")");     
        foreach (Move move in validMoves)
        {
            //Debug.Log("Valid move: (" + move.toX + "," + move.toY + ")");
            Vector2 spawnPos = new Vector2(changeXVector(move.toX), changeYVector(move.toY));
            GameObject dotObject = Instantiate(dot, spawnPos, Quaternion.identity);
            SuggestDot dotScript = dotObject.GetComponent<SuggestDot>();
            dotScript.position = new Vector2Int(move.toX, move.toY);
            dotScript.isAttackMove = false;

            activeDots.Add(dotObject);
        }

        List<Move> attackMoves = piece.pieceData.getAttackMove(board, pos.x, pos.y);
        //Debug.Log("Found " + attackMoves.Count + " attack moves for piece at (" + pos.x + "," + pos.y + ")");        
        foreach (Move move in attackMoves)
        {
            Vector2 spawnPos = new Vector2(changeXVector(move.toX), changeYVector(move.toY));
            GameObject dotObject = Instantiate(attack_high_light, spawnPos, Quaternion.identity);
            SuggestDot dotScript = dotObject.GetComponent<SuggestDot>();
            dotScript.position = new Vector2Int(move.toX, move.toY);
            dotScript.isAttackMove = true;
            activeDots.Add(dotObject);
        }
    }

    private bool checkKingSafety(Piece piece,Board checkBoard)
    {
        //piecesCurrentlyChecking= new List<Piece>();
        Debug.Log("check safety");
         checkBoard.PrintBoard();
        for (int i = 0; i < checkBoard.boardSize; i++)
        {
            for (int j = 0; j < checkBoard.boardSize; j++)
            {
                Piece currentPiece = checkBoard.board[i, j];
                // Debug.Log("the piece want to move is "+currentPiece.getType());
                if (currentPiece != null && currentPiece.isWhite != piece.isWhite)
                {
                    List<Move> attackMoves = currentPiece.getAttackMove(checkBoard, i, j);
                    foreach (Move attackMove in attackMoves)
                    {
                        if (checkBoard.board[attackMove.toX, attackMove.toY] is King)
                        {
                            Debug.Log("King is in check from piece at (" + i + "," + j + ") attacking (" + attackMove.toX + "," + attackMove.toY + ")");
                            Debug.Log("Attacking piece: " + currentPiece.GetType().Name + " color " + (currentPiece.isWhite ? "White" : "Black"));
                            //piecesCurrentlyChecking.Add(currentPiece);
                           // checkBoard.PrintBoard();
                            return false;
                        }
                    }
                }
            }
        }
        Debug.Log("nothing found king is safe");
        return true;
    }

    private bool simulateMoveAndCheckSafety(Piece piece, Move move)
    {
        // Piece originalToPiece = board.board[move.toX, move.toY];
        // Piece originalFromPiece = board.board[move.fromX, move.fromY];

        // // Simulate the move
        // board.board[move.toX, move.toY] = piece;
        // board.board[move.fromX, move.fromY] = null;

        // bool isSafe = checkKingSafety(piece, move);

        // // Revert the move
        // board.board[move.toX, move.toY] = originalToPiece;
        // board.board[move.fromX, move.fromY] = originalFromPiece;

        Piece[,] virtualBoard = Board.DeepCopyBoard(board.board);

        virtualBoard[move.toX, move.toY] = piece;
        virtualBoard[move.fromX, move.fromY] = null;
        Debug.Log("now check with simulator");
        if (checkKingSafety(piece,new Board { board = virtualBoard }))
        {
           
            return true;
        }
         Debug.Log("Move from (" + move.fromX + "," + move.fromY + ") to (" + move.toX + "," + move.toY + ") would put king in check. Move is not safe.");


        return false;
    }




    public void onDotClicked(SuggestDot dot)
    {
        if (!isPieceSelected) return;

        Vector2Int from = selectedPiece.position;
        Vector2Int to = dot.position;

        Move move = new Move(from.x, from.y, to.x, to.y);

        if (dot.isAttackMove)
        {

            // Debug.Log($"Performing attack move... {from.x},{from.y} to {to.x},{to.y}");
            // Debug.Log("Attack move from (" + from.x + "," + from.y + ") to (" + to.x + "," + to.y + ")");
            PieceView targetPiece = pieceViews[to.x, to.y];
            // Debug.Log("Target piece at (" + to.x + "," + to.y + "): " + targetPiece.pieceData.GetType().Name);
            if (targetPiece != null)
            {
                //Debug.Log("Attacking piece at (" + to.x + "," + to.y + "): " + targetPiece.pieceData.GetType().Name);
                Destroy(targetPiece.gameObject);
                board.board[to.x, to.y] = null;
            }
        }
        MovePiece(selectedPiece, move);
        isPieceSelected = false;
        clearDots();



    }


    private void MovePiece(PieceView piece, Move move)
    {
        Vector2 spawnPos = new Vector2(changeXVector(move.toX), changeYVector(move.toY));
        piece.transform.position = spawnPos;
        board.board[move.toX, move.toY] = piece.pieceData;
        board.board[piece.position.x, piece.position.y] = null;

        pieceViews[move.toX, move.toY] = piece;
        pieceViews[piece.position.x, piece.position.y] = null;
        piece.position = new Vector2Int(move.toX, move.toY);
        piece.pieceData.hasMoved = true;

        gameTurnWhite = !gameTurnWhite;


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



                    GameObject obj = Instantiate(getPrefab(board.board[i, j]), spawnPos, Quaternion.identity);



                    PieceView view = obj.GetComponent<PieceView>();

                    pieceViews[i, j] = view;
                    view.id = i * board.boardSize + j;

                    if (view != null)
                    {
                        view.Init(board.board[i, j], i, j); // 🔥 THIS LINE connects logic → view
                    }





                    // DEBUG EVERYTHING
                    //Debug.Log("Spawned: " + obj.name);

                    // Check PieceView

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
        return x - 4;
    }
    private int changeYVector(int y)
    {
        return y - 3;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
