using System.Collections.Generic;
using Fusion;
using UnityEngine;

public class UIScoreboardView : UIScreen
{
    //public static UIScoreboardView Instance { get; private set; }

    public RectTransform rowParent;
    //public UIScoreboardRow rowPrefab;

    private Dictionary<PlayerRef, UIScoreboardRow> activeRows = new();

    public override void Awake()
    {
        base.Awake();
    }

    public override void Init()
    {
        //Instance = this;                
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
        if (activeRows.ContainsKey(player)) return;
        
        foreach (Transform child in rowParent)
        {
            var row = child.GetComponent<UIScoreboardRow>();
            if (row != null && !row.gameObject.activeSelf)
            {
                row.gameObject.SetActive(true);
                row.SetNickname(info.nickname.ToString());
                activeRows[player] = row;
                return;
            }
        }
    }

    public void RemovePlayerRow(PlayerRef player)
    {
        if (activeRows.TryGetValue(player, out var row))
        {
            row.gameObject.SetActive(false);
            activeRows.Remove(player);
        }
    }

    public void UpdatePlayerRow(PlayerRef player, PlayerScoreData data)
    {
        if (activeRows.TryGetValue(player, out var row))
        {
            row.SetData(data);
        }
    }
}