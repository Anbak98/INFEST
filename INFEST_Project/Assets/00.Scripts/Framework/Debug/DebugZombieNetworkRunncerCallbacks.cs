using UnityEngine;
using Fusion;
using Fusion.Sockets;
using System.Collections.Generic;
using System;

public class DebugZombieNetworkRunncerCallbacks : MonoBehaviour, INetworkRunnerCallbacks
{
    [Header("Prefab For Runner.Spawn()")]
    [SerializeField] private NetworkPrefabRef _playerPrefab;

    [Header("Object Ref")]
    [SerializeField] private MonsterSpawner _monsterSpawner;

    [Header("Input Action")]
    [SerializeField] private PlayerInputActionHandler _playerInputActionHandler;
    [SerializeField] private InputManager _inputManager;

    [Header("Debug Related")]
    [SerializeField] private DebugerGameStarter _debuger;

    private Dictionary<PlayerRef, NetworkObject> _playerObjects = new();

    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        _debuger.AddDebugMessage(player + " " + runner.ToString() + "join");

        if (runner.LocalPlayer == player)
        {
            _inputManager.Init();
            _playerInputActionHandler.Init();
        }

        if (runner.IsServer)
        {
            NetworkObject newPlayerObj = runner.Spawn(_playerPrefab, inputAuthority: player);
            _playerObjects.Add(player, newPlayerObj);

            if(runner.LocalPlayer == player)
                _monsterSpawner.SpawnMonsterOnWave(transform);
        }
    }

    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
        _debuger.AddDebugMessage(player + " " + runner.ToString() + "left");

        if (runner.IsServer)
        {
            runner.Despawn(_playerObjects[player]);
            _playerObjects.Remove(player);
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

    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input) 
    {
        if (input.TryGet<NetworkInputData>(out var inputData))
        {
            _debuger.AddDebugMessage($"{player} MissingInput: {inputData}");
        }
        _debuger.AddDebugMessage($"{player} MissingInput: Unknown");
    }

    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason) 
    { 
        _debuger.AddErrorMessage(shutdownReason.ToString());
        _debuger.ShowErrorMessage();
    }

    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason)
    {
        _debuger.AddErrorMessage(remoteAddress + " " + reason.ToString());
        _debuger.ShowErrorMessage();
    }
    public void OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason)
    {
        _debuger.AddErrorMessage(reason.ToString());
        _debuger.ShowErrorMessage();
    }

    public void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player) { }
    public void OnConnectedToServer(NetworkRunner runner) { }
    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token) { }
    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message) { }
    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList) { }
    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data) { }
    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken) { }
    public void OnSceneLoadDone(NetworkRunner runner) { }
    public void OnSceneLoadStart(NetworkRunner runner) { }
    public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player) { }
    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, ArraySegment<byte> data) { }
    public void OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress) { }

}
