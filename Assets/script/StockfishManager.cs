using UnityEngine;
using System.Diagnostics;
using System.IO;
using System.Collections.Concurrent;
using System;

public class StockfishManager : MonoBehaviour
{
    private Process _engineProcess;
    private StreamWriter _engineInput;
    private ConcurrentQueue<string> _outputQueue = new ConcurrentQueue<string>();
    
    // Flags to track connection status
    private bool _isInitialized = false;
    private bool _isReady = false;

    public static StockfishManager Instance;

    void Start()
    {
        // The string here MUST match the file name exactly (including the .exe)
string path = Path.Combine(Application.streamingAssetsPath, "stockfish-windows-x86-64-avx2.exe");

        // Success Check 1: File Existence
        if (!File.Exists(path))
        {
            UnityEngine.Debug.LogError($"[Stockfish] Engine not found at: {path}. Check your StreamingAssets folder!");
            return;
        }

        _engineProcess = new Process();
        _engineProcess.StartInfo.FileName = path;
        _engineProcess.StartInfo.UseShellExecute = false;
        _engineProcess.StartInfo.RedirectStandardInput = true;
        _engineProcess.StartInfo.RedirectStandardOutput = true;
        _engineProcess.StartInfo.CreateNoWindow = true;

        _engineProcess.OutputDataReceived += (sender, e) => {
            if (!string.IsNullOrEmpty(e.Data)) _outputQueue.Enqueue(e.Data);
        };

        try 
        {
            _engineProcess.Start();
            _engineProcess.BeginOutputReadLine();
            _engineInput = _engineProcess.StandardInput;
            
            UnityEngine.Debug.Log("[Stockfish] Process started. Starting UCI handshake...");

            // Initialize UCI
            SendCommand("uci");
            SendCommand("isready");
        }
        catch (Exception ex)
        {
            UnityEngine.Debug.LogError($"[Stockfish] Failed to start process: {ex.Message}");
        }
    }

    public void SendCommand(string command)
    {
        if (_engineInput != null)
        {
            _engineInput.WriteLine(command);
        }
    }

    void Update()
    {
        while (_outputQueue.TryDequeue(out string line))
        {
            // Success Check 2: The Handshake
            if (line == "uciok")
            {
                _isInitialized = true;
                UnityEngine.Debug.Log("<color=green>[Stockfish] Handshake Complete! Engine identified.</color>");
            }

            // Success Check 3: Ready Status
            if (line == "readyok")
            {
                _isReady = true;
                UnityEngine.Debug.Log("<color=cyan>[Stockfish] Engine is Ready! You can now send moves.</color>");
            }

            if (line.StartsWith("bestmove"))
            {
                string move = line.Split(' ')[1];
                OnBestMoveFound(move);
            }
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (_isReady)
            {
                UnityEngine.Debug.Log("[Stockfish] Calculating best move...");
                SendCommand("position startpos");
                SendCommand("go depth 10");
            }
            else
            {
                UnityEngine.Debug.LogWarning("[Stockfish] Wait! Engine isn't ready yet.");
            }
        }
    }

    private void OnBestMoveFound(string move)
    {
        UnityEngine.Debug.Log($"<color=yellow>[Stockfish] Recommended Move: {move}</color>");
    }

    void OnApplicationQuit()
    {
        if (_engineProcess != null && !_engineProcess.HasExited)
        {
            SendCommand("quit");
            _engineProcess.Kill();
            _engineProcess.Dispose();
        }
    }


    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }
}