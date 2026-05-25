# ChessLord System Detailed Description

Tài liệu này mô tả chi tiết các hệ thống kỹ thuật trong dự án ChessLord, ánh xạ các khái niệm game truyền thống vào logic của một trò chơi cờ vua.

## 1. Hệ thống Quân cờ (Character System)

Hệ thống quân cờ được xây dựng trên mô hình Model-View. Dữ liệu logic nằm trong các lớp kế thừa từ `Piece`, và hiển thị được quản lý bởi `PieceView`.

### Cơ chế di chuyển (Movement Mechanism)
- **Logic**: Mỗi loại quân (`Pawn`, `Rook`, `Knight`, `Bishop`, `Queen`, `King`) ghi đè phương thức `getAllValidMoves` và `getAttackMove` để xác định các ô có thể đi.
- **Thực thi**: `GameManager.onPieceClicked` sẽ tính toán các nước đi hợp lệ. Khi người chơi chọn một ô đích (thông qua `SuggestDot`), `GameManager.MovePiece` sẽ cập nhật vị trí logic trên mảng `Board` và vị trí vật lý `transform.position` của quân cờ.
- **Ràng buộc**: Mọi nước đi đều được kiểm tra thông qua `simulateMoveAndCheckSafety` để đảm bảo không để Vua bị chiếu (King safety).

### Cơ chế "Nhảy" (Jumping Mechanism)
- Trong ChessLord, cơ chế này đặc thù cho quân **Mã (Knight)**. 
- Khác với các quân cờ khác (Rook, Bishop, Queen) phải kiểm tra vật cản trên đường đi, logic của `Knight.cs` trực tiếp xác định ô đích trong phạm vi hình chữ L, cho phép nó "nhảy" qua các quân cờ khác.

### Hệ thống Trạng thái / HP (Health System)
- Trong Cờ vua, quân cờ không có chỉ số HP. Trạng thái chỉ bao gồm: **Active** (trên bàn) hoặc **Captured** (bị ăn).
- **Cơ chế ăn quân**: Khi một quân cờ di chuyển vào ô của đối thủ (`isAttackMove`), đối tượng `PieceView` của đối thủ sẽ bị `Destroy`, xóa khỏi danh sách `pieceOnBoard` và thêm vào danh sách quân đã bị bắt (`capturedWhitePieces`/`capturedBlackPieces`).

### Animation states
- Hiện tại, việc di chuyển quân cờ được thực hiện ngay lập tức qua `transform.position`.
- Trạng thái trực quan được thể hiện qua:
  - Highlight quân đang chọn.
  - `SuggestDot`: Hiển thị các ô có thể đi.
  - `lastMovePrefab`: Highlight ô đi và ô đến của nước đi cuối cùng.
  - Hiệu ứng đổi màu đỏ (Color.red) khi Vua đang bị chiếu.

---

## 2. Hệ thống Đối thủ (Enemy System)

### Các loại đối thủ (Enemy Types)
- Đối thủ được xác định bởi màu sắc quân cờ ngược lại với người chơi (Trắng hoặc Đen).
- Hệ thống bao gồm đầy đủ 6 loại quân cờ tiêu chuẩn với logic hành vi riêng biệt.

### AI / Hành vi (AI Behavior)
- **Stockfish Integration**: Hệ thống sử dụng engine **Stockfish** (thông qua `StockfishManager.cs`).
- **Quy trình**:
  1. Sau mỗi nước đi của người chơi, `GameManager` xuất chuỗi FEN (Forsyth-Edwards Notation) đại diện cho trạng thái bàn cờ.
  2. Chuỗi FEN được gửi đến Stockfish qua giao thức UCI.
  3. Stockfish tính toán và trả về `bestmove`.
  4. Hệ thống thực thi nước đi đó cho phía đối thủ.

### Cơ chế va chạm (Collision/Capture)
- Va chạm được xử lý ở mức độ logic ô cờ (Grid-based).
- Khi một nước đi tấn công được xác nhận, hệ thống thực hiện "va chạm" bằng cách loại bỏ instance của quân cờ bị tấn công khỏi scene.

---

## 3. Hệ thống UI (UI System)

Hệ thống UI được quản lý tập trung và phản ứng theo trạng thái của `GameManager`.

### Các màn hình (Screens)
- **Menu (menuScence)**: Màn hình khởi đầu cho phép bắt đầu game.
- **Gameplay (chessScence)**: Màn hình chính chứa bàn cờ, khu vực quân bị bắt và lịch sử nước đi.
- **GameOver**: Một Overlay được kích hoạt bởi `gameMatchPanel` khi phát hiện Chiếu bí (Checkmate) hoặc Hòa (Stalemate).

### Các UI Elements
- **Move History (`listMoveScript`)**: Hiển thị danh sách các nước đi đã thực hiện theo định dạng chuẩn cờ vua.
- **Capture Containers**: Hai khu vực hiển thị các quân cờ đã bị ăn của người chơi và máy.
- **Promote Menu (`PromoteMenu.cs`)**: Hiển thị khi quân Tốt tiến đến hàng cuối cùng, cho phép người chơi chọn quân để phong cấp (Hậu, Xe, Tượng, Mã).
- **Side/Level Select**: Cho phép chọn phe (Trắng/Đen) và độ sâu tính toán của AI.
