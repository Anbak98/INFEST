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

    [Networked] public bool Lock { get; private set; } = false;

    //private bool _isPrivate = false;

    public override void Spawned()
    {
        MatchManager.Instance.RoomUI.Room = this;
        MatchManager.Instance.Room = this;

        Runner.AddCallbacks(this);

        if (HostPlayer == PlayerRef.None)
        {
            // 첫 번째 플레이어를 방장으로 설정
            HostPlayer = Runner.LocalPlayer;
            Debug.Log($"[Room] Host assigned to {Runner.LocalPlayer}");
        }

        Runner.Spawn(_profilePrefab, inputAuthority: Runner.LocalPlayer).GetComponent<PlayerProfile>();
        PlayerPrefs.SetString("RoomCode", Runner.SessionInfo.Name);
    }

    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        if(!Lock)
        {
            // 방장이 없고, 현재 내가 StateAuthority면 새로 지정
            if (HostPlayer == PlayerRef.None)
            {
                HostPlayer = player;
                Debug.Log($"[Room] Host reassigned to {player}");
            }

            MatchManager.Instance.RoomUI.SetVisualablePlayPartyButtonOnHost(HostPlayer == Runner.LocalPlayer);
        }
    }

    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
        if (!Lock)
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

            if (MatchManager.Instance != null)
                MatchManager.Instance.RoomUI.SetVisualablePlayPartyButtonOnHost(HostPlayer == Runner.LocalPlayer);
        }
    }

    [Rpc(RpcSources.All, RpcTargets.All)]
    public void RPC_BroadcastUpdatePlayerProfile()
    {
        if (!Lock)
        {
            MyProfile.SetInfo();
            MatchManager.Instance.RoomUI.UpdateUI(_teamProfiles);
        }
    }

    [Rpc(RpcSources.All, RpcTargets.All)]
    public void RPC_SendProfileToAll(PlayerProfile playerProfile)
    {
        if(!Lock)
        {
            if (MyProfile != playerProfile && !_teamProfiles.Contains(playerProfile))
                _teamProfiles.Add(playerProfile);

            MatchManager.Instance.RoomUI.UpdateUIWhenJoinRoom();
            MatchManager.Instance.RoomUI.UpdateUI(_teamProfiles);
        }
    }

    [Rpc(RpcSources.All, RpcTargets.All)]
    public void RPC_RemoveProfileToAll(PlayerProfile playerProfile)
    {
        if(!Lock)
        {
            if (_teamProfiles.Contains(playerProfile))
                _teamProfiles.Remove(playerProfile);

            if (MatchManager.Instance != null && MatchManager.Instance.RoomUI != null)
                MatchManager.Instance.RoomUI.UpdateUI(_teamProfiles);
        }
    }

    public void HostPlayGame()
    {
        Lock = true;
        if (Runner.LocalPlayer == HostPlayer)
        {
            RPC_BrodcastPlayGame();
        }
    }

    [Rpc(RpcSources.All, RpcTargets.All)]
    public void RPC_BrodcastPlayGame()
    {
        Lock = true;

        if (Runner.LocalPlayer == HostPlayer)
            PlayerPrefs.SetInt("Host", 1);
        else
            PlayerPrefs.SetInt("Host", 0);

        if (Runner.IsSharedModeMasterClient)
        {
            Runner.LoadScene("RuinedCity");
        }
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