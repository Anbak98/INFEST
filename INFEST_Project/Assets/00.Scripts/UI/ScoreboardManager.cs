using System.Collections.Generic;
using Fusion;
using UnityEngine;

public struct PlayerScoreData : INetworkStruct
{
    public int kills;
    public int deaths;
    public int gold;
}

public struct CharacterInfoData : INetworkStruct
{
    public NetworkString<_16> nickname;
}


public class ScoreboardManager : NetworkBehaviour
{
    public static ScoreboardManager Instance { get; private set; }

    [Networked]
    public NetworkDictionary<PlayerRef, PlayerScoreData> PlayerScores => default;

    private UIScoreboardView scoreboardView;

    [SerializeField] private Dictionary<PlayerRef, CharacterInfoData> _playerInfos = new();

    public override void Spawned()
    {
        Instance = this;
        scoreboardView = UIScoreboardView.Instance;
    }

    public void OnPlayerJoined(PlayerRef newPlayer, CharacterInfoData info)
    {
        //if (!Runner.IsServer) return;

        //PlayerScores.Add(newPlayer, new PlayerScoreData());
        //_playerInfos[newPlayer] = info;

        //RPC_BroadcastAddPlayerRow(newPlayer, info, PlayerScores[newPlayer]);

        //foreach (var kvp in _playerInfos)
        //{
        //    var existing = kvp.Key;
        //    if (existing == newPlayer) continue;

        //    RPC_AddExistingPlayerRow(newPlayer, existing, kvp.Value, PlayerScores[existing]);
        //}
    }

    // 모든 클라이언트에게 새 플레이어 행 추가
    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    private void RPC_BroadcastAddPlayerRow(PlayerRef player, CharacterInfoData info, PlayerScoreData score)
    {
        scoreboardView.AddPlayerRow(player, info);
        scoreboardView.UpdatePlayerRow(player, score);
    }

    // 새로 들어온 클라이언트에게 기존 플레이어 정보를 알려 줌
    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    private void RPC_AddExistingPlayerRow(PlayerRef target, PlayerRef player, CharacterInfoData info, PlayerScoreData score)
    {
        scoreboardView.AddPlayerRow(player, info);
        scoreboardView.UpdatePlayerRow(player, score);
    }    

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void RPC_AddKill(PlayerRef player)
    {
        if (PlayerScores.TryGet(player, out var data))
        {
            data.kills++;
            PlayerScores.Set(player, data);
            scoreboardView.UpdatePlayerRow(player, data);
        }
    }

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void RPC_AddDeath(PlayerRef player)
    {
        if (PlayerScores.TryGet(player, out var data))
        {
            data.deaths++;
            PlayerScores.Set(player, data);
            scoreboardView.UpdatePlayerRow(player, data);
        }
    }

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void RPC_AddGold(PlayerRef player, int amount)
    {
        if (Runner.TryGetPlayerObject(player, out var playerObj))
        {
            var characterInfo = playerObj.GetComponent<CharacterInfoInstance>();
            characterInfo.curGold += amount;

            if (PlayerScores.TryGet(player, out var data))
            {
                data.gold += amount;
                PlayerScores.Set(player, data);
                scoreboardView.UpdatePlayerRow(player, data);
            }
        }
    }

    //[Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    //public void RPC_AddPlayerRow(PlayerRef player, CharacterInfoData info, PlayerScoreData score)
    //{
    //    if (!PlayerScores.ContainsKey(player))
    //    {
    //        PlayerScores.Add(player, score);
    //    }

    //    if (!_playerInfos.ContainsKey(player))
    //    {
    //        _playerInfos[player] = info;
    //    }

    //    scoreboardView.AddPlayerRow(player, info);
    //    scoreboardView.UpdatePlayerRow(player, score);
    //}

    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    public void RPC_RemovePlayerRow(PlayerRef player)
    {
        if (PlayerScores.ContainsKey(player))
        {
            PlayerScores.Remove(player);
        }

        if (_playerInfos.ContainsKey(player))
        {
            _playerInfos.Remove(player);
        }

        scoreboardView.RemovePlayerRow(player);
    }
}
