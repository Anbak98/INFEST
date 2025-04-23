using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//public struct UIPlayerData : INetworkStruct
//{
//    [Networked, Capacity(24)]
//    public string Nickname { get => default; set { } }
//    public PlayerRef PlayerRef;
//    public int Kills;
//    public int Deaths;
//    public int Golds;
//    public int LastKillTick;
//    public int StatisticPosition;
//    public bool IsAlive;
//    public bool IsConnected;
//}

//public enum EGameplayState
//{
//    Skirmish = 0,
//    Running = 1,
//    Finished = 2,
//}

//public class TestPlay : NetworkBehaviour
//{
//    public TestGameUI gameUI;
//    public TestPlayer playerPrefab;
//    public float gameDuration = 180f;
//    public float PlayerRespawnTime = 5f;
//    public float DoubleDamageDuration = 30f;

//    [Networked]
//    [Capacity(32)]
//    [HideInInspector]
//    public NetworkDictionary<PlayerRef, UIPlayerData> PlayerData { get; }

//    [Networked]
//    [HideInInspector]
//    public TickTimer RemainingTime { get; set; }

//    [Networked]
//    [HideInInspector]
//    public EGameplayState State { get; set; }

//    public bool DoubleDamageActive => State == EGameplayState.Running && RemainingTime.RemainingTime(Runner).GetValueOrDefault() < DoubleDamageDuration;

//    private bool _isNicknameSent;
//    private float _runningStateTime;
//    private List<TestPlayer> _spawnedPlayers = new(16);
//    private List<PlayerRef> _pendingPlayers = new(16);
//    private List<UIPlayerData> _tempPlayerData = new(16);
//    private List<Transform> _recentSpawnPoints = new(4);

//    public void PlayerKilled(PlayerRef killerPlayerRef, PlayerRef victimPlayerRef, EWeaponType weaponType, bool isCriticalKill)
//    {
//        if (HasStateAuthority == false)
//            return;

//        // Update statistics of the killer player.
//        if (PlayerData.TryGet(killerPlayerRef, out UIPlayerData killerData))
//        {
//            killerData.Kills++;
//            killerData.LastKillTick = Runner.Tick;
//            PlayerData.Set(killerPlayerRef, killerData);
//        }

//        // Update statistics of the victim player.
//        var playerData = PlayerData.Get(victimPlayerRef);
//        playerData.Deaths++;
//        playerData.IsAlive = false;
//        PlayerData.Set(victimPlayerRef, playerData);

//        // Inform all clients about the kill via RPC.
//        RPC_PlayerKilled(killerPlayerRef, victimPlayerRef, weaponType, isCriticalKill);

//        StartCoroutine(RespawnPlayer(victimPlayerRef, PlayerRespawnTime));

//        RecalculateStatisticPositions();
//    }

//    public override void FixedUpdateNetwork()
//    {
//        if (HasStateAuthority == false)
//            return;

//        // PlayerManager is a special helper class which iterates over list of active players (NetworkRunner.ActivePlayers) and call spawn/despawn callbacks on demand.
//        PlayerManager.UpdatePlayerConnections(Runner, SpawnPlayer, DespawnPlayer);

//        // Start gameplay when there are enough players connected.
//        if (State == EGameplayState.Skirmish && PlayerData.Count > 1)
//        {
//            StartGameplay();
//        }

//        if (State == EGameplayState.Running)
//        {
//            _runningStateTime += Runner.DeltaTime;

//            var sessionInfo = Runner.SessionInfo;

//            // Hide the match after 60 seconds. Players won't be able to randomly connect to existing game and start new one instead.
//            // Joining via party code should work.
//            if (sessionInfo.IsVisible && (_runningStateTime > 60f || sessionInfo.PlayerCount >= sessionInfo.MaxPlayers))
//            {
//                sessionInfo.IsVisible = false;
//            }

//            if (RemainingTime.Expired(Runner))
//            {
//                StopGameplay();
//            }
//        }
//    }

//    public override void Render()
//    {
//        if (Runner.Mode == SimulationModes.Server)
//            return;

//        // Every client must send its nickname to the server when the game is started.
//        if (_isNicknameSent == false)
//        {
//            RPC_SetPlayerNickname(Runner.LocalPlayer, PlayerPrefs.GetString("Photon.Menu.Username"));
//            _isNicknameSent = true;
//        }
//    }

//    private void SpawnPlayer(PlayerRef playerRef)
//    {
//        if (PlayerData.TryGet(playerRef, out var playerData) == false)
//        {
//            playerData = new UIPlayerData();
//            playerData.PlayerRef = playerRef;
//            playerData.Nickname = playerRef.ToString();
//            playerData.StatisticPosition = int.MaxValue;
//            playerData.IsAlive = false;
//            playerData.IsConnected = false;
//        }

//        if (playerData.IsConnected == true)
//            return;

//        Debug.LogWarning($"{playerRef} connected.");

//        playerData.IsConnected = true;
//        playerData.IsAlive = true;

//        PlayerData.Set(playerRef, playerData);

//        var spawnPoint = GetSpawnPoint();
//        var player = Runner.Spawn(playerPrefab, spawnPoint.position, spawnPoint.rotation, playerRef);

//        // Set player instance as PlayerObject so we can easily get it from other locations.
//        Runner.SetPlayerObject(playerRef, player.Object);

//        RecalculateStatisticPositions();
//    }

//    private void DespawnPlayer(PlayerRef playerRef, TestPlayer player)
//    {
//        if (PlayerData.TryGet(playerRef, out var playerData) == true)
//        {
//            if (playerData.IsConnected == true)
//            {
//                Debug.LogWarning($"{playerRef} disconnected.");
//            }

//            playerData.IsConnected = false;
//            playerData.IsAlive = false;
//            PlayerData.Set(playerRef, playerData);
//        }

//        Runner.Despawn(player.Object);

//        RecalculateStatisticPositions();
//    }

//    private IEnumerator RespawnPlayer(PlayerRef playerRef, float delay)
//    {
//        if (delay > 0f)
//            yield return new WaitForSecondsRealtime(delay);

//        if (Runner == null)
//            yield break;

//        // Despawn old player object if it exists.
//        var playerObject = Runner.GetPlayerObject(playerRef);
//        if (playerObject != null)
//        {
//            Runner.Despawn(playerObject);
//        }

//        // Don't spawn the player for disconnected clients.
//        if (PlayerData.TryGet(playerRef, out UIPlayerData playerData) == false || playerData.IsConnected == false)
//            yield break;

//        // Update player data.
//        playerData.IsAlive = true;
//        PlayerData.Set(playerRef, playerData);

//        var spawnPoint = GetSpawnPoint();
//        var player = Runner.Spawn(playerPrefab, spawnPoint.position, spawnPoint.rotation, playerRef);

//        // Set player instance as PlayerObject so we can easily get it from other locations.
//        Runner.SetPlayerObject(playerRef, player.Object);
//    }

//    private Transform GetSpawnPoint()
//    {
//        Transform spawnPoint = default;

//        // Iterate over all spawn points in the scene.
//        var spawnPoints = Runner.SimulationUnityScene.GetComponents<SpawnPoint>(false);
//        for (int i = 0, offset = Random.Range(0, spawnPoints.Length); i < spawnPoints.Length; i++)
//        {
//            spawnPoint = spawnPoints[(offset + i) % spawnPoints.Length].transform;

//            if (_recentSpawnPoints.Contains(spawnPoint) == false)
//                break;
//        }

//        // Add spawn point to list of recently used spawn points.
//        _recentSpawnPoints.Add(spawnPoint);

//        // Ignore only last 3 spawn points.
//        if (_recentSpawnPoints.Count > 3)
//        {
//            _recentSpawnPoints.RemoveAt(0);
//        }

//        return spawnPoint;
//    }

//    private void StartGameplay()
//    {
//        // Stop all respawn coroutines.
//        StopAllCoroutines();

//        State = EGameplayState.Running;
//        RemainingTime = TickTimer.CreateFromSeconds(Runner, gameDuration);

//        // Reset player data after skirmish and respawn players.
//        foreach (var playerPair in PlayerData)
//        {
//            var data = playerPair.Value;

//            data.Kills = 0;
//            data.Deaths = 0;
//            data.StatisticPosition = int.MaxValue;
//            data.IsAlive = false;

//            PlayerData.Set(data.PlayerRef, data);

//            StartCoroutine(RespawnPlayer(data.PlayerRef, 0f));
//        }
//    }

//    private void StopGameplay()
//    {
//        RecalculateStatisticPositions();

//        State = EGameplayState.Finished;
//    }

//    private void RecalculateStatisticPositions()
//    {
//        if (State == EGameplayState.Finished)
//            return;

//        _tempPlayerData.Clear();

//        foreach (var pair in PlayerData)
//        {
//            _tempPlayerData.Add(pair.Value);
//        }

//        _tempPlayerData.Sort((a, b) =>
//        {
//            if (a.Kills != b.Kills)
//                return b.Kills.CompareTo(a.Kills);

//            return a.LastKillTick.CompareTo(b.LastKillTick);
//        });

//        for (int i = 0; i < _tempPlayerData.Count; i++)
//        {
//            var playerData = _tempPlayerData[i];
//            playerData.StatisticPosition = playerData.Kills > 0 ? i + 1 : int.MaxValue;

//            PlayerData.Set(playerData.PlayerRef, playerData);
//        }
//    }

//    [Rpc(RpcSources.StateAuthority, RpcTargets.All, Channel = RpcChannel.Reliable)]
//    private void RPC_PlayerKilled(PlayerRef killerPlayerRef, PlayerRef victimPlayerRef, EWeaponType weaponType, bool isCriticalKill)
//    {
//        string killerNickname = "";
//        string victimNickname = "";

//        if (PlayerData.TryGet(killerPlayerRef, out UIPlayerData killerData))
//        {
//            killerNickname = killerData.Nickname;
//        }

//        if (PlayerData.TryGet(victimPlayerRef, out UIPlayerData victimData))
//        {
//            victimNickname = victimData.Nickname;
//        }

//        //GameUI.GameplayView.KillFeed.ShowKill(killerNickname, victimNickname, weaponType, isCriticalKill);
//    }

//    [Rpc(RpcSources.All, RpcTargets.StateAuthority, Channel = RpcChannel.Reliable)]
//    private void RPC_SetPlayerNickname(PlayerRef playerRef, string nickname)
//    {
//        if (string.IsNullOrWhiteSpace(nickname))
//        {
//            nickname = "aaa";
//        }

//        if (!PlayerData.TryGet(playerRef, out UIPlayerData playerData))
//        {
//            Debug.LogWarning($"[Nickname] PlayerData not found for {playerRef}, creating new.");
//            playerData = new UIPlayerData
//            {
//                PlayerRef = playerRef,                
//                IsConnected = true,
//                IsAlive = false,
//                StatisticPosition = int.MaxValue
//            };
//            Debug.Log($"PlayerRef: {playerRef}");
//        }

//        playerData.Nickname = nickname;
//        PlayerData.Set(playerRef, playerData);
//    }
//}