using Fusion;
using Fusion.Sockets;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Room : NetworkBehaviour, INetworkRunnerCallbacks
{
    [Networked] public PlayerRef HostPlayer { get; set; }
    [SerializeField] private NetworkPrefabRef _profilePrefab;

    public NetworkRunner _runner;

    [SerializeField]
    private List<PlayerProfile> _playerProfiles;

    public override void Spawned()
    {
        _runner = Runner;
        _runner.AddCallbacks(this);

        if (HostPlayer == PlayerRef.None)
        {
            // 첫 번째 플레이어를 방장으로 설정
            HostPlayer = Runner.LocalPlayer;
            Debug.Log($"[Room] Host assigned to {Runner.LocalPlayer}");
        }

        var obj = Runner.Spawn(_profilePrefab, inputAuthority: Runner.LocalPlayer);
    }

    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {

        // 방장이 없고, 현재 내가 StateAuthority면 새로 지정
        if (HostPlayer == PlayerRef.None)
        {
            HostPlayer = player;
            Debug.Log($"[Room] Host reassigned to {player}");
        }

    }

    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
        if (player == HostPlayer)
        {
            // 방장이 나갔으면 다른 사람 중에서 새로 지정
            foreach (var otherPlayer in runner.ActivePlayers)
            {
                if (otherPlayer != player)
                {
                    HostPlayer = otherPlayer;
                    Debug.Log($"[Room] Host transferred to {otherPlayer}");

                    break;
                }
            }

            // 아무도 없다면 초기화
            if (runner.ActivePlayers.Count() == 0)
            {
                HostPlayer = PlayerRef.None;
                Debug.Log($"[Room] No players left, host cleared.");
            }
        }
    }

    [Rpc(RpcSources.All, RpcTargets.All)]
    public void RPC_SendProfileToAll(PlayerProfile playerProfile)
    {
        if (!_playerProfiles.Contains(playerProfile))
        {
            _playerProfiles.Add(playerProfile);
        }

        int i = 0;

        foreach (var profile in MatchManager.Instance.uiProfils)
        {
            profile.NickName.text = "";
        }

        foreach (var item in _playerProfiles)
        {
            MatchManager.Instance.uiProfils[i++].NickName.text = item.Info.NickName.ToString();
        }
    }

    [Rpc(RpcSources.All, RpcTargets.All)]
    public void RPC_RemoveProfileToAll(PlayerProfile playerProfile)
    {
        if (_playerProfiles.Contains(playerProfile))
        {
            _playerProfiles.Remove(playerProfile);
        }

        foreach (var profile in MatchManager.Instance.uiProfils)
        {
            profile.NickName.text = "";
        }

        int i = 0;
        foreach (var item in _playerProfiles)
        {
            if (i > 3)
            {
                Debug.LogError("s");
            }
            Debug.Log("HGI");
            MatchManager.Instance.uiProfils[i++].NickName.text = item.Info.NickName.ToString();
        }
    }

    public void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player) { }
    public void OnInput(NetworkRunner runner, NetworkInput input) { }
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
    public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player) { }
    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, ArraySegment<byte> data) { }
    public void OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress) { }
}