using Fusion;
using Fusion.Addons.Physics;
using Fusion.Sockets;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary> 
/// INetworkRunnerCallbacks를 구현하면 
/// 너무 많은 기능을 맡게 되어 단일책임원칙을 지킬 수 없게 되는 문제가 있다
/// 하지만 INetworkRunnerCallbacks을 구현해야만 하는 경우가 있다
/// 
/// OnInput은 입력을 네트워크로 보내는 곳에서는 반드시 구현이 되어야한다
/// 그러면 방법은 INetworkRunnerCallbacks를 나누어 구현하는 것 뿐이다
/// Spawner에서는 OnPlayerJoined, OnPlayerLeft만 구현한다
/// OnGUI에서는 StartGame을 호출한다
/// </summary>
public class StandradSpawner : MonoBehaviour, INetworkRunnerCallbacks
{
    public NetworkRunner _runner;
    private bool _mouseButton0;
    private bool _mouseButton1;
    
    [SerializeField] private NetworkPrefabRef _scoreboardManagerPrefab;
    private NetworkObject _scoreboardManagerObject;

    [SerializeField] private NetworkPrefabRef _playerPrefab;
    [SerializeField] private PlayerInputActionHandler _playerInputActionHandler;
    private Dictionary<PlayerRef, NetworkObject> _spawnedCharacters = new Dictionary<PlayerRef, NetworkObject>();




    // Input Action을 이용한 키입력
    //private PlayerActionMap _inputActions;


    /// <summary>
    /// Fusion은 다음 두 가지 방식 중 하나로 입력 콜백을 받습니다:
    /// 
    /// 1. Runner를 시작한 MonoBehaviour가 INetworkRunnerCallbacks 구현한 경우
    /// NetworkRunner.StartGame() 호출 시 내부적으로 this.gameObject.GetComponents<INetworkRunnerCallbacks>()를 통해 
    /// Runner를 생성한 GameObject에 붙은 INetworkRunnerCallbacks 구현체들을 찾아서 자동으로 AddCallbacks 호출한다
    /// 
    /// 2. 직접 runner.AddCallbacks(playerInputSender) 호출한다(비추천)
    /// 
    /// </summary>
    /// <param name="mode"></param>
    async void StartGame(GameMode mode)
    {
        // Create the Fusion runner and let it know that we will be providing user input
        _runner = gameObject.AddComponent<NetworkRunner>();

        // 네트워크 물리가 러너 객체에서 RunnerSimulatePhysics3D 컴포넌트를 작동시키려면 필요 
        gameObject.AddComponent<RunnerSimulatePhysics3D>();

        _runner.ProvideInput = true;

        // Create the NetworkSceneInfo from the current scene
        var scene = SceneRef.FromIndex(SceneManager.GetActiveScene().buildIndex);
        var sceneInfo = new NetworkSceneInfo();
        if (scene.IsValid)
        {
            sceneInfo.AddSceneRef(scene, LoadSceneMode.Additive);
        }

        /// AddCallbacks 되어 있는 경우에만 키 입력시 OnInput 호출가능
        // Start or join (depends on gamemode) a session with a specific name
        await _runner.StartGame(new StartGameArgs()
        {
            GameMode = mode,
            SessionName = "TestRoom",
            Scene = scene,
            SceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>()
        });

        if (_runner.IsServer)
        {
            _scoreboardManagerObject = _runner.Spawn(_scoreboardManagerPrefab, Vector3.zero, Quaternion.identity);
        }
    }
    private void OnGUI()
    {
        if (_runner == null)
        {
            if (GUI.Button(new Rect(0, 0, 200, 40), "Host"))
            {
                StartGame(GameMode.Host);
            }
            if (GUI.Button(new Rect(0, 40, 200, 40), "Join"))
            {
                StartGame(GameMode.Client);
            }
        }
    }

    /// <summary>
    /// INetworkRunnerCallbacks 구현
    /// </summary>
    /// <param name="runner"></param>
    /// <param name="player"></param>
    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        // 서버만 오브젝트를 생성할 수 있다
        if (runner.IsServer)
        {
            // Create a unique position for the player
            Vector3 spawnPosition = new Vector3((player.RawEncoded % runner.Config.Simulation.PlayerCount) * 3, 1, 0);

            /// 플레이어 오브젝트를 생성
            NetworkObject networkPlayerObject = runner.Spawn(_playerPrefab, spawnPosition, Quaternion.identity, player);
            // Keep track of the player avatars for easy access

            _spawnedCharacters.Add(player, networkPlayerObject);
                       
            CharacterInfo info = DataManager.Instance.GetByKey<CharacterInfo>(player.PlayerId);
            CharacterInfoData infoData = new CharacterInfoData
            {
                nickname = info.Name
            };
            ScoreboardManager.Instance.RPC_AddPlayerRow(player, infoData);

            //Global.Instance.NetworkRunner = runner;
        }

    }
    // 접속종료
    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
        if (_spawnedCharacters.TryGetValue(player, out NetworkObject networkObject))
        {
            runner.Despawn(networkObject);
            _spawnedCharacters.Remove(player);

            ScoreboardManager.Instance.RPC_RemovePlayerRow(player);            
        }
    }

    #region 나머지는 필요한 곳에서 구현한다(Single Responsibility Principle)
    /// <summary>
    /// Input Action을 사용하여 C# 이벤트 받는 방식으로 수정했다
    /// 메서드의 위치를 적절한 곳으로 옮기는 것이 남았다
    /// runner.AddCallbakcs(this)가 호출되어야 OnInput이 호출된다
    /// 자동 호출되어 입력값을 서버로 전송
    /// </summary>
    /// <param name="runner"></param>
    /// <param name="input"></param>
    public void OnInput(NetworkRunner runner, NetworkInput input)
    {
        var data = _playerInputActionHandler.GetNetworkInput();

        if (data != null)
        {
            input.Set(data.Value);  
        }
    }

    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input) { }
    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason) { }
    public void OnConnectedToServer(NetworkRunner runner) { }
    public void OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason) { }
    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token) { }
    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason) { }
    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message) { }
    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList) { }
    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data) { }
    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken) { }
    public void OnSceneLoadDone(NetworkRunner runner) { }
    public void OnSceneLoadStart(NetworkRunner runner) { }
    public void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player) { }
    public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player) { }
    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, ArraySegment<byte> data) { }
    public void OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress) { }
    #endregion
}
