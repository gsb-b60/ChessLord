using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace ChessEngine
{
    public class ChessEngineManager : MonoBehaviour
    {
        public static ChessEngineManager Instance { get; private set; }

        [SerializeField] private int defaultLevel = 5;

        private ChessEngineService _service;
        private bool _isInitializing = false;
        private TaskCompletionSource<bool> _initTcs;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
                _service = new ChessEngineService();
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private async void Start()
        {
            await EnsureConnectedAsync();
        }

        public async Task<bool> EnsureConnectedAsync()
        {
            if (_service.IsConnected) return true;
            
            if (_isInitializing)
            {
                if (_initTcs != null)
                {
                    return await _initTcs.Task;
                }
                return false; // Should not happen with _isInitializing check
            }

            _isInitializing = true;
            _initTcs = new TaskCompletionSource<bool>();
            
            try
            {
                await _service.InitializeAsync();
                
                // Initial new game command to set level
                var request = new EngineRequest("new_game") { level = defaultLevel };
                await _service.SendCommandAsync(request);
                
                _initTcs.SetResult(true);
                return true;
            }
            catch (Exception ex)
            {
                UnityEngine.Debug.LogError($"[ChessEngineManager] Connection failed: {ex.Message}");
                _initTcs.SetResult(false);
                return false;
            }
            finally
            {
                _isInitializing = false;
                _initTcs = null;
            }
        }

        public async Task<EngineResponse> StartNewGame(int level)
        {
            await EnsureConnectedAsync();
            var request = new EngineRequest("new_game") { level = level };
            return await _service.SendCommandAsync(request);
        }

        public async Task<EngineResponse> PlayerMove(string moveUci)
        {
            await EnsureConnectedAsync();
            var request = new EngineRequest("player_move") { move = moveUci };
            return await _service.SendCommandAsync(request);
        }

        public async Task<EngineResponse> GetEngineMove()
        {
            await EnsureConnectedAsync();
            var request = new EngineRequest("engine_move");
            return await _service.SendCommandAsync(request);
        }

        public async Task<EngineResponse> SetPosition(string fen)
        {
            await EnsureConnectedAsync();
            var request = new EngineRequest("set_position") { fen = fen };
            return await _service.SendCommandAsync(request);
        }

        public async Task<EngineResponse> GetBoard()
        {
            await EnsureConnectedAsync();
            var request = new EngineRequest("get_board");
            return await _service.SendCommandAsync(request);
        }

        private void OnApplicationQuit()
        {
            _service?.Dispose();
        }
    }
}
