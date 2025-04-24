using System;
using Fusion;
using TMPro;
using UnityEngine;

//[Serializable]
//public struct UIPlayerData : INetworkStruct
//{
//    [Networked, Capacity(24)]
//    public string Nickname { get => default; set { } }

//    public PlayerRef PlayerRef;
//    public int Kills;
//    public int Deaths;
//    public int Golds;
//}

public class UIScoreboardRow : MonoBehaviour
{
    public TextMeshProUGUI nickName;
    public TextMeshProUGUI kills;
    public TextMeshProUGUI deaths;
    public TextMeshProUGUI golds;

    public void SetData(PlayerScoreData data)
    {
        kills.text = data.kills.ToString();
        deaths.text = data.deaths.ToString();
        golds.text = data.gold.ToString("N0");
    }

    public void SetNickname(string name)
    {
        nickName.text = name;
    }
}
