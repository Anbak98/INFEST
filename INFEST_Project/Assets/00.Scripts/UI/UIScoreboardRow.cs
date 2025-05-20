using Fusion;
using INFEST.Game;
using TMPro;
using UnityEngine;

public class UIScoreboardRow : MonoBehaviour
{
    public TextMeshProUGUI nickName;
    public TextMeshProUGUI kills;
    public TextMeshProUGUI deaths;
    public TextMeshProUGUI golds;

    public void SetData(PlayerRef player)
    {
        nickName.text = NetworkGameManager.Instance.gamePlayers.GetProfile(player).nickname.ToString();
        kills.text = NetworkGameManager.Instance.gamePlayers.GetKillCount(player).ToString();
        deaths.text = NetworkGameManager.Instance.gamePlayers.GetDeathCount(player).ToString();
        golds.text = NetworkGameManager.Instance.gamePlayers.GetGoldCount(player).ToString();
    }

    public void SetNickname(string name)
    {
        nickName.text = name;
    }
}