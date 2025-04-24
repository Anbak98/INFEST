using System.Collections.Generic;
using Fusion;
using UnityEngine;

public class UIScoreboardView : UIScreen
{
    public Transform rowParent;
    public UIScoreboardRow rowPrefab;

    private Dictionary<PlayerRef, UIScoreboardRow> activeRows = new();

    [Networked]
    public Profile Info { get; set; }


    public override void Awake()
    {
        base.Awake();                
    }

    public override void Init()
    {
        base.Init();
        Hide();
    }

    public override void Show()
    {
        base.Show();
    }

    public override void Hide()
    {
        base.Hide();
    }    

    public void AddPlayerRow(PlayerRef player, CharacterInfoData info)
    {        
        UIScoreboardRow row = Instantiate(rowPrefab, rowParent);
        row.SetNickname(info.nickname.ToString());

        PlayerScoreData init = new PlayerScoreData
        {
            kills = 0,
            deaths = 0,
            gold = 500
        };
        row.SetData(init);

        activeRows[player] = row;
    }

    public void RemovePlayerRow(PlayerRef player)
    {
        if(activeRows.TryGetValue(player, out var row))
        {
            Destroy(row.gameObject);
            activeRows.Remove(player);
        }
    }    

   public void UpdatePlayerRow(PlayerRef player, PlayerScoreData data)
    {
        if(activeRows.TryGetValue(player, out var row))
        {
            row.SetData(data);
        }
    }
}