using Fusion;
using Fusion.Addons.Physics;
using Fusion.Sockets;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// 이름만 Spawner 
/// 
/// 실제 기능이 달라 
/// </summary>
public class BasicSpawner : MonoBehaviour, INetworkRunnerCallbacks
{
    private NetworkRunner _runner;
    private bool _mouseButton0;
    private bool _mouseButton1;

    [SerializeField] private NetworkPrefabRef _playerPrefab;
    private Dictionary<PlayerRef, NetworkObject> _spawnedCharacters = new Dictionary<PlayerRef, NetworkObject>();

    // Input Action을 이용한 키입력
    private PlayerActionMap _inputActions;

    // awake가 되는지는 모르겠는데...? MonoBehavior 상속받았으니까 되겠지?
    


    async void StartGame(GameMode mode)
    {
        // Create the Fusion runner and let it know that we will be providing user input
        _runner = gameObject.AddComponent<NetworkRunner>();

        // _runner가 생성된 다음이니... 가능하지 않을까?
        _inputActions = new PlayerActionMap();
        _inputActions.Enable();

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

        // Start or join (depends on gamemode) a session with a specific name
        await _runner.StartGame(new StartGameArgs()
        {
            GameMode = mode,
            SessionName = "TestRoom",
            Scene = scene,
            SceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>()
        });
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

    private void Update()
    {
        //  빠른 탭을 놓치지 않도록  마우스 버튼을 샘플링하고 입력 구조체에 기록되면 다시 설정합니다:
        _mouseButton0 = _mouseButton0 || Input.GetMouseButton(0);
        _mouseButton1 = _mouseButton1 || Input.GetMouseButton(1);
    }


    /// <summary>
    /// INetworkRunnerCallbacks 구현
    /// 이걸 상속받는 객체와 runner는 동일해야한다
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
        }
    }

    // 접속종료
    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
        if (_spawnedCharacters.TryGetValue(player, out NetworkObject networkObject))
        {
            runner.Despawn(networkObject);
            _spawnedCharacters.Remove(player);
        }
    }
    
    /// <summary>
    /// Input Action을 사용하여 C# 이벤트 받는 방식으로 수정했다
    /// 메서드의 위치를 적절한 곳으로 옮기는 것이 남았다
    /// </summary>
    /// <param name="runner"></param>
    /// <param name="input"></param>
    public void OnInput(NetworkRunner runner, NetworkInput input)
    {
        var data = new NetworkInputData();

        //if (Input.GetKey(KeyCode.W))
        //    data.direction += Vector3.forward;

        //if (Input.GetKey(KeyCode.S))
        //    data.direction += Vector3.back;

        //if (Input.GetKey(KeyCode.A))
        //    data.direction += Vector3.left;

        //if (Input.GetKey(KeyCode.D))
        //    data.direction += Vector3.right;

        //data.buttons.Set(NetworkInputData.MOUSEBUTTON0, _mouseButton0);
        //_mouseButton0 = false;
        //data.buttons.Set(NetworkInputData.MOUSEBUTTON1, _mouseButton1);
        //_mouseButton1 = false;


        // 1. 이동 입력 (WASD → Vector2)
        Vector2 move = _inputActions.Player.Move.ReadValue<Vector2>();
        data.direction = new Vector3(move.x, 0, move.y);

        // 2. 버튼 입력(세부동작 구현은 아직)
        if (_inputActions.Player.Fire.WasPressedThisFrame())
            data.buttons.Set(NetworkInputData.BUTTON_FIRE, true);
        if (_inputActions.Player.Zoom.WasPressedThisFrame())
            data.buttons.Set(NetworkInputData.BUTTON_ZOOM, true);
        if (_inputActions.Player.Jump.WasPressedThisFrame())
            data.buttons.Set(NetworkInputData.BUTTON_JUMP, true);
        if (_inputActions.Player.Reload.WasPressedThisFrame())
            data.buttons.Set(NetworkInputData.BUTTON_RELOAD, true);
        if (_inputActions.Player.Interaction.WasPressedThisFrame())
            data.buttons.Set(NetworkInputData.BUTTON_INTERACT, true);
        if (_inputActions.Player.UseItem.WasPressedThisFrame())
            data.buttons.Set(NetworkInputData.BUTTON_USEITEM, true);
        if (_inputActions.Player.Run.IsPressed())
            data.buttons.Set(NetworkInputData.BUTTON_RUN, true);
        if (_inputActions.Player.Sit.IsPressed())
            data.buttons.Set(NetworkInputData.BUTTON_SIT, true);
        if (_inputActions.Player.ScoreBoard.IsPressed())
            data.buttons.Set(NetworkInputData.BUTTON_SCOREBOARD, true);

        input.Set(data);
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
}
