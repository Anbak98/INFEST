using Fusion;
using Fusion.Sockets;
using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Room : NetworkBehaviour, INetworkRunnerCallbacks
{
    [Networked] public PlayerRef HostPlayer { get; set; }
    [SerializeField] private NetworkPrefabRef _profilePrefab;

    public PlayerProfile MyProfile;
    [SerializeField] private List<PlayerProfile> _teamProfiles = new();

    private bool _isPrivate = false;

    public override void Spawned()
    {
        MatchManager.Instance.RoomUI.Room = this;
        MatchManager.Instance.Room = this;

        Runner.AddCallbacks(this);

        if (HostPlayer == PlayerRef.None)
        {
            // ù ��° �÷��̾ �������� ����
            HostPlayer = Runner.LocalPlayer;
            Debug.Log($"[Room] Host assigned to {Runner.LocalPlayer}");
        }

        Runner.Spawn(_profilePrefab, inputAuthority: Runner.LocalPlayer).GetComponent<PlayerProfile>();
    }

    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {

        // ������ ����, ���� ���� StateAuthority�� ���� ����
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
    }

    [Rpc(RpcSources.All, RpcTargets.All)]
    public void RPC_BroadcastUpdatePlayerProfile()
    {
        MyProfile.SetInfo();
        MatchManager.Instance.RoomUI.UpdateUI(_teamProfiles);
    }

    [Rpc(RpcSources.All, RpcTargets.All)]
    public void RPC_SendProfileToAll(PlayerProfile playerProfile)
    {
        if (MyProfile != playerProfile && !_teamProfiles.Contains(playerProfile))
            _teamProfiles.Add(playerProfile);

        MatchManager.Instance.RoomUI.UpdateUI(_teamProfiles);
    }

    [Rpc(RpcSources.All, RpcTargets.All)]
    public void RPC_RemoveProfileToAll(PlayerProfile playerProfile)
    {
        if (_teamProfiles.Contains(playerProfile))
            _teamProfiles.Remove(playerProfile);

        MatchManager.Instance.RoomUI.UpdateUI(_teamProfiles);
    }

    public void HostPlayGame()
    {
        RPC_BrodcastPlayGame();
    }

    [Rpc(RpcSources.All, RpcTargets.All)]
    public void RPC_BrodcastPlayGame()
    {
        if (Runner.IsSharedModeMasterClient)
        {
            Runner.LoadScene("PlayStage(MVP)");
        }
    }

    [Rpc(RpcSources.All, RpcTargets.All)]
    public void RPC_BrodcastChangeSession()
    {
        MatchManager.Instance.CreateNewSession(
            false,
            (MatchManager.GameType)(int)Runner.SessionInfo.Properties["type"],
            (MatchManager.GameMap)(int)Runner.SessionInfo.Properties["map"],
            Runner.SessionInfo.Name);
    }

    #region NOT USED
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
    #endregion
}