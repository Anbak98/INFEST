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

    public void SetData(CharacterInfo info)
    {
        nickName.text = info.Name;
    }
}
