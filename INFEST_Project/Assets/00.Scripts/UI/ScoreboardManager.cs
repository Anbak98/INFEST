using Fusion;

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

    public override void Spawned()
    {
        Instance = this;
        scoreboardView = UIScoreboardView.Instance;
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
        if (PlayerScores.TryGet(player, out var data))
        {
            data.gold += amount;
            PlayerScores.Set(player, data);

            scoreboardView.UpdatePlayerRow(player, data);
        }
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    public void RPC_AddPlayerRow(PlayerRef player, CharacterInfoData info)
    {
        if (!PlayerScores.ContainsKey(player))
        {
            PlayerScores.Add(player, new PlayerScoreData());
        }

        scoreboardView.AddPlayerRow(player, info);
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    public void RPC_RemovePlayerRow(PlayerRef player)
    {
        if (PlayerScores.ContainsKey(player))
        {
            PlayerScores.Remove(player);
        }

        scoreboardView.RemovePlayerRow(player);
    }
}
