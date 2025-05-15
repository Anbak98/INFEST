using Fusion;
using UnityEngine;

public struct GamePlayer : INetworkStruct
{
    public NetworkString<_16> nickname;
    public NetworkObject PlayerCharacter;
}


public class GameManager : SingletonNetworkBehaviour<GameManager>
{
    public NetworkDictionary<PlayerRef, GamePlayer> PlayerCharacters { get; set; }

    public void AddFromLocal(PlayerRef player, NetworkObject playerCharacter)
    {
        if (Runner.LocalPlayer == player)
        {
            GamePlayer gp = new()
            {
                nickname = PlayerPrefs.GetString("Nickname"),
                PlayerCharacter = playerCharacter
            };

            PlayerCharacters.Set(player, gp);
        }
        else
        {
            Debug.LogError("[ GameManager ] Unvalid add from other player");
        }
    }
}
