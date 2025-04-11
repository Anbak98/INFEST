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

    public UIPlayerProfile[] uiPlayerInfos;
    private List<PlayerProfile> _playerProfiles = new();
    private PlayerProfile _playerProfile;

    public override void Spawned()
    {
        _runner = Runner;
        _runner.AddCallbacks(this);
        NetworkObject profile = _runner.Spawn(_profilePrefab);
        uiPlayerInfos = FindObjectsOfType<UIPlayerProfile>();
        if (HostPlayer == PlayerRef.None)
        {
            // ù ��° �÷��̾ �������� ����
            HostPlayer = Runner.LocalPlayer;
            Debug.Log($"[Room] Host assigned to {Runner.LocalPlayer}");
        }
        _playerProfile = profile.GetComponent<PlayerProfile>();
    }

    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        // ������ ����, ���� ���� StateAuthority�� ���� ����
        if (HostPlayer == PlayerRef.None)
        {
            HostPlayer = player;
            Debug.Log($"[Room] Host reassigned to {player}");
        }
        RPC_SendProfileToAll(_playerProfile);
    }

    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
        if (player == HostPlayer)
        {
            // ������ �������� �ٸ� ��� �߿��� ���� ����
            foreach (var otherPlayer in runner.ActivePlayers)
            {
                if (otherPlayer != player)
                {
                    HostPlayer = otherPlayer;
                    Debug.Log($"[Room] Host transferred to {otherPlayer}");

                    break;
                }
            }

            // �ƹ��� ���ٸ� �ʱ�ȭ
            if (runner.ActivePlayers.Count() == 0)
            {
                HostPlayer = PlayerRef.None;
                Debug.Log($"[Room] No players left, host cleared.");
            }
        }

        RPC_SendProfileToAll(_playerProfile);
    }

    [Rpc(RpcSources.All, RpcTargets.All)]
    public void RPC_SendProfileToAll(PlayerProfile playerProfile)
    {
        if (!_playerProfiles.Contains(playerProfile))
        {
            _playerProfiles.Add(playerProfile);
        }

        int i = 0;
        foreach (var item in _playerProfiles)
        {
            uiPlayerInfos[i++].NickName.text = item.Runner.GetPlayerUserId();
        }

        Debug.Log(i);
    }

    [Rpc(RpcSources.All, RpcTargets.All)]
    public void RPC_RemoveProfileToAll(PlayerProfile playerProfile)
    {
        if (_playerProfiles.Contains(playerProfile))
        {
            _playerProfiles.Remove(playerProfile);
        }

        int i = 0;
        foreach (var item in _playerProfiles)
        {
            if (i > 3)
            {
                Debug.LogError("s");
            }
            uiPlayerInfos[i++].NickName.text = item.Runner.GetPlayerUserId();
        }
        Debug.Log(i);
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