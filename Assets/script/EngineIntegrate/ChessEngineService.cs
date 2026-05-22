using System;
using System.Diagnostics;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ChessEngine
{
    public class ChessEngineService : IDisposable
    {
        private Process _engineProcess;
        private TcpClient _client;
        private NetworkStream _stream;
        private const string Host = "127.0.0.1";
        private const int Port = 8765;

        public bool IsConnected => _client != null && _client.Connected;

        public async Task InitializeAsync()
        {
            try
            {
                LaunchEngineProcess();
                await ConnectToSocketAsync();
            }
            catch (Exception ex)
            {
                UnityEngine.Debug.LogError($"[ChessEngine] Initialization failed: {ex.Message}");
                throw;
            }
        }

        private void LaunchEngineProcess()
        {
            string enginePath = Path.Combine(Application.streamingAssetsPath, "engine", "engine.exe");

            if (!File.Exists(enginePath))
            {
                throw new FileNotFoundException("Chess engine executable not found", enginePath);
            }

            _engineProcess = new Process();
            _engineProcess.StartInfo.FileName = enginePath;
            _engineProcess.StartInfo.CreateNoWindow = true;
            _engineProcess.StartInfo.UseShellExecute = false;
            _engineProcess.StartInfo.WorkingDirectory = Path.GetDirectoryName(enginePath);
            
            // Fix encoding issues on Windows
            _engineProcess.StartInfo.EnvironmentVariables["PYTHONIOENCODING"] = "utf-8";

            _engineProcess.Start();
            UnityEngine.Debug.Log("[ChessEngine] Process started.");
        }

        private async Task ConnectToSocketAsync()
        {
            int retryCount = 0;
            const int maxRetries = 10;
            const int retryDelayMs = 500;

            while (retryCount < maxRetries)
            {
                try
                {
                    _client = new TcpClient();
                    await _client.ConnectAsync(Host, Port);
                    _stream = _client.GetStream();
                    UnityEngine.Debug.Log("[ChessEngine] Connected to TCP server.");
                    return;
                }
                catch (SocketException)
                {
                    retryCount++;
                    UnityEngine.Debug.LogWarning($"[ChessEngine] Connection failed. Retrying ({retryCount}/{maxRetries})...");
                    await Task.Delay(retryDelayMs);
                }
            }

            throw new Exception("Could not connect to chess engine TCP server.");
        }

        public async Task<EngineResponse> SendCommandAsync(EngineRequest request)
        {
            if (!IsConnected)
            {
                throw new InvalidOperationException("Not connected to chess engine.");
            }

            try
            {
                string json = JsonUtility.ToJson(request) + "\n";
                byte[] data = Encoding.UTF8.GetBytes(json);
                await _stream.WriteAsync(data, 0, data.Length);

                byte[] buffer = new byte[8192];
                int bytesRead = await _stream.ReadAsync(buffer, 0, buffer.Length);
                string responseJson = Encoding.UTF8.GetString(buffer, 0, bytesRead);

                return JsonUtility.FromJson<EngineResponse>(responseJson);
            }
            catch (Exception ex)
            {
                UnityEngine.Debug.LogError($"[ChessEngine] Error sending command '{request.cmd}': {ex.Message}");
                return new EngineResponse { ok = false, error = ex.Message };
            }
        }

        public void Dispose()
        {
            _stream?.Dispose();
            _client?.Close();
            _client?.Dispose();

            if (_engineProcess != null && !_engineProcess.HasExited)
            {
                _engineProcess.Kill();
                _engineProcess.Dispose();
            }
        }
    }
}
