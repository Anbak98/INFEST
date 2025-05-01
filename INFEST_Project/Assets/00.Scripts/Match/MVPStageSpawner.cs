using Fusion;
using Fusion.Addons.Physics;
using Fusion.Sockets;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class MVPStageSpawner : MonoBehaviour, INetworkRunnerCallbacks
{
    [Header("NetworkPrefabRef for Spawn")]
    [SerializeField] private NetworkPrefabRef _playerPrefab;
    [SerializeField] private NetworkPrefabRef _monsterSpawnerPrefab;
    [SerializeField] private NetworkPrefabRef _scoreboardManagerPrefab;
    [SerializeField] private NetworkPrefabRef _storePrefab;

    [Header("InputActionHandler for OnInput")]
    [SerializeField] private PlayerInputActionHandler _playerInputActionHandler;

    [Header("Spawn Point")]
    [SerializeField] private Transform _playerSpawnPoint;

    [Header("Loding UI")]
    [SerializeField] private GameObject _loadingUI;
    [SerializeField] private TMP_Text _loadingTextUI;

    private Dictionary<PlayerRef, NetworkObject> _spawnedCharacters = new Dictionary<PlayerRef, NetworkObject>();

    private NetworkRunner _runner;
    private NetworkObject _scoreboardManagerObject;

    MonsterSpawner monsterSpawner;

    public void SpawnMonster()
    {
        monsterSpawner.SpawnMonsterOnWave();
    }

    public void Start()
    {
        ReconnectionWithSwitchingSharedToHost();
    }

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

    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        if (runner.IsServer)
        {
            // Create a unique position for the player
            Vector3 spawnPosition = new Vector3(27, 1, -27);
            NetworkObject networkPlayerObject = runner.Spawn(_playerPrefab, spawnPosition, Quaternion.identity, player);
            // Keep track of the player avatars for easy access
            _spawnedCharacters.Add(player, networkPlayerObject);
            if (runner.LocalPlayer == player)
            {
                monsterSpawner = runner.Spawn(_monsterSpawnerPrefab, Vector3.zero).GetComponent<MonsterSpawner>();
                monsterSpawner.SpawnMonsterOnWave();

                runner.Spawn(_storePrefab, Vector3.zero);
            }

            CharacterInfoData infoData = new CharacterInfoData
            {
                nickname = DataManager.Instance.GetByKey<CharacterInfo>(player.PlayerId).Name
            };

            ScoreboardManager.Instance.OnPlayerJoined(player, infoData);
        }
        _loadingUI.SetActive(false);
    }

    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
        if (_spawnedCharacters.TryGetValue(player, out NetworkObject networkObject))
        {
            runner.Despawn(networkObject);
            _spawnedCharacters.Remove(player);
            ScoreboardManager.Instance.RPC_RemovePlayerRow(player);
        }
    }


    public void OnInput(NetworkRunner runner, NetworkInput input)
    {
        var data = _playerInputActionHandler.GetNetworkInput();

        if (data != null)
        {
            input.Set(data.Value);
        }
    }

    private async void ReconnectionWithSwitchingSharedToHost()
    {
        _runner = FindAnyObjectByType<NetworkRunner>();
        _loadingUI.SetActive(true);
        StartGameResult result;
        int retryCount = 0;

        if (_runner != null)
        {
            GameMode gameMode;

            if (PlayerPrefs.GetInt("Host") == 1)
            {
                gameMode = GameMode.Host;
                _loadingTextUI.text = $"게임을 생성 중입니다... {retryCount}";
            }
            else
            {
                gameMode = GameMode.Client;
                _loadingTextUI.text = $"호스트가 게임을 생성 중입니다... {retryCount}";
            }

            do
            {
                await _runner.Shutdown();

                var runnerGO = new GameObject("Runner (Host)");
                var newRunner = runnerGO.AddComponent<NetworkRunner>();

                _runner = newRunner;
                _runner.ProvideInput = true;

                // Create the NetworkSceneInfo from the current scene
                //var scene = SceneRef.FromIndex(SceneManager.GetActiveScene().buildIndex);
                //var sceneInfo = new NetworkSceneInfo();

                //if (scene.IsValid)
                //{
                //    sceneInfo.AddSceneRef(scene, LoadSceneMode.Additive);
                //}

                result = await newRunner.StartGame(new StartGameArgs
                {
                    GameMode = gameMode,
                    SessionName = PlayerPrefs.GetString("RoomCode"),
                    IsVisible = false,
                    //Scene = scene,
                    SceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>()
                });

                if (_runner.IsServer)
                {
                    _scoreboardManagerObject = _runner.Spawn(_scoreboardManagerPrefab, Vector3.zero, Quaternion.identity);
                }

                newRunner.AddCallbacks(this);

            } while (!result.Ok && retryCount < 15);
        }
    }

    public void OnConnectedToServer(NetworkRunner runner) { }
    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input) { }
    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason) { }
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
