# ChessLord Architecture Overview

This document describes the high-level architecture of the ChessLord project, detailing the core systems, their responsibilities, and how they interact.

## 1. System Components

### Core Logic (Model)
- **Board (`Board.cs`)**:
  - Maintains the 8x8 logical state of the chess game using a 2D array of `Piece` objects.
  - Handles board-level operations like `DeepCopyBoard` for move simulation.
- **Piece (`Piece.cs`)**:
  - Abstract base class for all chess pieces (Pawn, Rook, Knight, Bishop, Queen, King).
  - Defines virtual methods `getAllValidMoves` and `getAttackMove` which are overridden by specific piece classes.
  - Tracks piece properties: color (`isWhite`) and movement status (`hasMoved`).
- **Move (`Move.cs`)**:
  - Data structure representing a move from `(fromX, fromY)` to `(toX, toY)`.
  - Includes flags for special moves: `isCastle`, `isEnPassant`, `isPawnLongMove`, and `promoteTo` type.

### Controller
- **GameManager (`GameManager.cs` / class `GameManage`)**:
  - The central singleton orchestrating game flow.
  - Manages turn logic (`gameTurnWhite`), piece selection, and move execution.
  - Validates moves by simulating them on a virtual board to check King safety.
  - Detects end-game states: Checkmate, Stalemate, and Draw conditions.
  - Interfaces with UI systems and the AI engine.

### Presentation (View)
- **PieceView**:
  - Unity `MonoBehaviour` attached to piece prefabs.
  - Links a physical GameObject with its logical `Piece` data.
  - Handles visual positioning on the board.
- **UI Systems**:
  - **PromoteMenu**: UI for selecting a piece during pawn promotion.
  - **GameMatchScript**: Displays the end-game result (Win/Loss/Draw).
  - **ListMoveScript**: Manages the move history panel visuals.
  - **SuggestDot**: Visual indicators for valid moves that handle click detection for move execution.

### AI Integration
- **StockfishManager (`StockfishManager.cs`)**:
  - Manages an external Stockfish engine process.
  - Communicates via UCI (Universal Chess Interface) protocol.
  - Receives FEN strings from `GameManager` and returns the "best move" recommendations.

---

## 2. Key Interactions

### Move Workflow
1. **Selection**: User clicks a `PieceView`. `GameManager.onPieceClicked` is triggered.
2. **Validation**:
   - `GameManager` requests raw moves from the logical `Piece`.
   - `GameManager` filters these moves by calling `simulateMoveAndCheckSafety`, which creates a copy of the `Board` to ensure the move doesn't leave the King in check.
3. **Visualization**: `GameManager` instantiates `SuggestDot` prefabs at all valid destination squares.
4. **Execution**:
   - User clicks a `SuggestDot`.
   - `GameManager.onDotClicked` updates the logical `Board`.
   - `GameManager.MovePiece` updates the physical position of the `PieceView`.
   - Special moves (Castle, En Passant, Promotion) are handled.
5. **Post-Move**:
   - The turn is toggled.
   - End-game conditions are checked.
   - A FEN string is exported and sent to `StockfishManager`.

### AI Recommendation
- After every move, `GameManager` generates a FEN string representing the current board state.
- `StockfishManager.SendCommand` sends this position to the engine.
- When Stockfish returns a `bestmove`, `OnBestMoveFound` logs the recommendation.

---

## 3. Directory Structure (Script Focus)
- `Assets/script/`:
  - `GameManager.cs`: Main Controller.
  - `Board.cs`: Board Logic & State.
  - `Piece.cs`: Piece Base Class & Logic.
  - `Move.cs`: Move Data Structure.
  - `StockfishManager.cs`: AI Engine Bridge.
  - `pieces/`: Contains specific piece logic (e.g., `Pawn.cs`, `King.cs`).
  - `UI/`: Contains UI-specific controllers (`PromoteMenu.cs`, etc.).
  - `PieceView.cs`: Visual representation link.
