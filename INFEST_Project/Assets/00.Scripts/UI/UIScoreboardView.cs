using System.Collections.Generic;
using Fusion;
using INFEST.Game;
using UnityEngine;

public class UIScoreboardView : UIScreen
{
    //public static UIScoreboardView Instance { get; private set; }

    public RectTransform rowParent;
    //public UIScoreboardRow rowPrefab;

    private Dictionary<PlayerRef, UIScoreboardRow> activeRows = new();
    private GamePlayerHandler gamePlayerHandler;

    public override void Awake()
    {
        base.Awake();
        Init();
        OnHide();
    }

    public override void Init()
    {
        //Instance = this;                
        base.Init();
        gamePlayerHandler = NetworkGameManager.Instance.gamePlayers;

        UpdateScoreboard();
        gamePlayerHandler.OnValueChanged += UpdateScoreboard;

        OnHide();
    }

    public override void OnShow()
    {
        base.OnShow();
    }

    public override void OnHide()
    {
        base.OnHide();        
    }

    public void UpdateScoreboard()
    {
        foreach (var player in gamePlayerHandler.GetPlayerRefs())
        {
            AddPlayerRow(player);
            UpdatePlayerRow(player);
        }
    }


    public void AddPlayerRow(PlayerRef player)
    {
        if (activeRows.ContainsKey(player)) 
            return;
        
        foreach (Transform child in rowParent)
        {
            var row = child.GetComponent<UIScoreboardRow>();
            if (row != null && !row.gameObject.activeSelf)
            {
                row.gameObject.SetActive(true);
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

    public void UpdatePlayerRow(PlayerRef player)
    {

        if (activeRows.TryGetValue(player, out var row))
        {
            row.SetData(player);
        }
    }
}