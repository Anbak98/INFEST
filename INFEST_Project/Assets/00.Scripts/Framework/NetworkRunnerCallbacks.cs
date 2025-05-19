using Fusion;
using Fusion.Sockets;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace INFEST.Game
{
    public class NetworkRunnerCallbacks : MonoBehaviour, INetworkRunnerCallbacks
    {
        [Header("Prefab For Runner.Spawn()")]
        [SerializeField] private NetworkPrefabRef _playerPrefab;

        [Header("Input Related")]
        [SerializeField] private PlayerInputActionHandler _playerInputActionHandler;
        [SerializeField] private InputManager _InputManager;

        public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
        {
            if (runner.LocalPlayer == player)
            {
                _InputManager.Init();
                _playerInputActionHandler.Init();
            }

            if (runner.IsServer)
            {
                NetworkObject netObj = runner.Spawn(_playerPrefab, NetworkGameManager.Instance.gamePlayers.PlayerSpawnPoints[0].position, inputAuthority: player);
                NetworkGameManager.Instance.gamePlayers.AddPlayerObj(player, netObj.Id);
                netObj.GetComponent<PlayerStatHandler>().Init(player);
            }
        }

        public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
        {
            if (runner.IsServer)
            {
                NetworkGameManager.Instance.gamePlayers.RemovePlayer(player);
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
        }

        public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
        {
        }

        public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason)
        {
        }
        public void OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason)
        {
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
}
