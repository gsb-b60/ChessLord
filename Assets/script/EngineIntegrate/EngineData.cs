using System;

namespace ChessEngine
{
    [Serializable]
    public class EngineResponse
    {
        public bool ok;
        public string error;
        public string fen;
        public string move;
        public GameStatus status;
    }

    [Serializable]
    public class GameStatus
    {
        public bool over;
        public string winner; // "white", "black", or "draw"
        public string reason; // "checkmate", "stalemate", etc.
    }

    [Serializable]
    public class EngineRequest
    {
        public string cmd;
        public int level;
        public string move;
        public string fen;

        public EngineRequest(string cmd)
        {
            this.cmd = cmd;
        }
    }
}
