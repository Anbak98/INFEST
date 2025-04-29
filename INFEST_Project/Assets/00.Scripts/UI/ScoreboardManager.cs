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

    public void OnPlayerJoined(PlayerRef joinedPlayer, PlayerProfile profile)
    {
        if (!Object.HasInputAuthority) return;

        var info = new CharacterInfoData
        {
            nickname = profile.Info.NickName
        };

        _playerInfos[joinedPlayer] = info;
        PlayerScores.Set(joinedPlayer, new PlayerScoreData());

        foreach (var kvp in _playerInfos)
        {
            RPC_SendPlayerInfoToTarget(joinedPlayer, kvp.Key, kvp.Value, PlayerScores[kvp.Key]);
        }

        RPC_AddPlayerRow(joinedPlayer, info, new PlayerScoreData());
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    private void RPC_SendPlayerInfoToTarget(PlayerRef target, PlayerRef player, CharacterInfoData info, PlayerScoreData score)
    {
        if (!PlayerScores.ContainsKey(player))
        {
            PlayerScores.Add(player, score);
        }

        if (!_playerInfos.ContainsKey(player))
        {
            _playerInfos[player] = info;
        }

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

    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    public void RPC_AddPlayerRow(PlayerRef player, CharacterInfoData info, PlayerScoreData score)
    {
        if (!PlayerScores.ContainsKey(player))
        {
            PlayerScores.Add(player, score);
        }

        if (!_playerInfos.ContainsKey(player))
        {
            _playerInfos[player] = info;
        }

        scoreboardView.AddPlayerRow(player, info);
        scoreboardView.UpdatePlayerRow(player, score);
    }

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
