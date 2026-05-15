Create a complete software architecture diagram set for a Unity chess project named “ChessLord”.

Use professional software engineering diagram conventions.
All labels, titles, components, arrows, and annotations must be written in English.
Style should be clean, modern, minimal, and suitable for technical documentation or a university software engineering report.

Generate the following diagrams:

1. High-Level System Architecture Diagram
2. UML Class Diagram
3. Component Diagram
4. Sequence Diagram for Move Workflow
5. Sequence Diagram for AI Recommendation Workflow
6. MVC / Layered Architecture Diagram
7. Data Flow Diagram
8. State Diagram for Game Flow
9. Folder / Project Structure Diagram

Project description:

# Project Overview

ChessLord is a Unity-based chess game using object-oriented architecture and integration with the Stockfish chess engine through the UCI protocol.

# Main Architecture Layers

## Core Logic Layer (Model)

### Board (`Board.cs`)

Responsibilities:

* Stores the logical 8x8 chess board using a 2D array of Piece objects.
* Maintains current board state.
* Supports deep copy simulation with `DeepCopyBoard`.
* Used for move validation and king safety simulation.

### Piece (`Piece.cs`)

Abstract base class for all chess pieces:

* Pawn
* Rook
* Knight
* Bishop
* Queen
* King

Responsibilities:

* Defines virtual methods:

  * `getAllValidMoves`
  * `getAttackMove`
* Stores:

  * `isWhite`
  * `hasMoved`

Inheritance:

* Pawn inherits Piece
* Rook inherits Piece
* Knight inherits Piece
* Bishop inherits Piece
* Queen inherits Piece
* King inherits Piece

### Move (`Move.cs`)

Represents a chess move.

Fields:

* `fromX`
* `fromY`
* `toX`
* `toY`
* `isCastle`
* `isEnPassant`
* `isPawnLongMove`
* `promoteTo`

---

# Controller Layer

## GameManager (`GameManager.cs`)

Main singleton controller managing the entire game flow.

Responsibilities:

* Turn management
* Piece selection
* Move validation
* Move execution
* Check/checkmate detection
* Draw detection
* Stalemate detection
* Virtual board simulation
* Communication with UI
* Communication with StockfishManager

Important methods:

* `onPieceClicked`
* `onDotClicked`
* `MovePiece`
* `simulateMoveAndCheckSafety`

Important state:

* `gameTurnWhite`

---

# Presentation Layer (View)

## PieceView

Unity MonoBehaviour attached to chess piece prefabs.

Responsibilities:

* Connects GameObject to logical Piece
* Handles visual positioning
* Displays board representation

## UI Components

### PromoteMenu

* Pawn promotion UI

### GameMatchScript

* End game result UI

### ListMoveScript

* Move history UI

### SuggestDot

* Visual indicator for valid moves
* Detects click input for move execution

---

# AI Integration Layer

## StockfishManager (`StockfishManager.cs`)

Handles communication with external Stockfish engine.

Responsibilities:

* Launches Stockfish process
* Uses UCI protocol
* Sends FEN strings
* Receives best move suggestions

Important methods:

* `SendCommand`
* `OnBestMoveFound`

---

# Key Workflow: Move Execution

Sequence:

1. User clicks PieceView
2. GameManager.onPieceClicked is triggered
3. GameManager requests moves from Piece
4. GameManager validates moves using virtual board simulation
5. SuggestDot objects are instantiated
6. User clicks SuggestDot
7. GameManager.onDotClicked executes move
8. Board logical state updates
9. PieceView visual state updates
10. Special moves handled:

* Castling
* En Passant
* Promotion

11. Turn changes
12. End-game conditions checked
13. FEN string generated
14. FEN sent to StockfishManager

---

# Key Workflow: AI Recommendation

Sequence:

1. GameManager exports FEN string
2. StockfishManager.SendCommand sends UCI command
3. Stockfish engine analyzes board
4. Engine returns `bestmove`
5. StockfishManager.OnBestMoveFound receives result
6. Recommendation displayed or logged

---

# Directory Structure

Assets/script/

* GameManager.cs
* Board.cs
* Piece.cs
* Move.cs
* StockfishManager.cs
* PieceView.cs
* pieces/

  * Pawn.cs
  * Rook.cs
  * Knight.cs
  * Bishop.cs
  * Queen.cs
  * King.cs
* UI/

  * PromoteMenu.cs
  * GameMatchScript.cs
  * ListMoveScript.cs
  * SuggestDot.cs

---

Diagram requirements:

* Use clear directional arrows.
* Differentiate layers using colors or containers.
* Use UML notation where appropriate.
* Sequence diagrams must include activation bars and message flow.
* Class diagrams must show inheritance relationships.
* Component diagrams must show dependencies and communication.
* State diagram should include:

  * Idle
  * Piece Selected
  * Move Validation
  * Moving Piece
  * Check
  * Checkmate
  * Stalemate
  * Promotion
  * Game End
* Data flow diagram should show:

  * User Input
  * UI Layer
  * GameManager
  * Board
  * Piece
  * StockfishManager
  * Stockfish Engine

Visual style:

* Dark tech style or modern software architecture style.
* High readability.
* Professional engineering presentation quality.
* Avoid excessive decoration.
* Use consistent spacing and alignment.
