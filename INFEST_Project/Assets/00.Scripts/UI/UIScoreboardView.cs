using System.Collections.Generic;
using Fusion;
using UnityEngine;

public class UIScoreboardView : UIScreen
{
    public Transform rowParent;
    public UIScoreboardRow uirow;

    private Dictionary<PlayerRef, UIScoreboardRow> activeRows = new();

    private List<UIScoreboardRow> _rows = new(32);
    private CharacterInfo _characterInfo;

    [Networked]
    public Profile Info { get; set; }

    //[Networked]
    //public UIPlayerData UIPlayerData { get; set; }

    private int _kill = 0;
    private int _death = 0;

    private NetworkRunner _runner;

    public override void Awake()
    {
        base.Awake();                

        //uirow.nickName.text = (string)Info.NickName;
        //SetKDG();

        ////임시코드
        //uirow.nickName.text = "정민";
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

    private void SetKDG(UIScoreboardRow row)
    {
        _characterInfo = DataManager.Instance.GetByKey<CharacterInfo>(1);
        row.kills.text = _kill.ToString();
        row.deaths.text = _death.ToString();
        row.golds.text = _characterInfo.StartGold.ToString("N0");
    }

    public void AddPlayerRow(PlayerRef player, CharacterInfo info)
    {
        if (activeRows.ContainsKey(player))
            return;

        var row = Instantiate(uirow, rowParent);
        row.SetData(info);
        SetKDG(row);

        activeRows.Add(player, row);
        _rows.Add(row);        
    }

    public void RemovePlayerRow(PlayerRef player)
    {
        if(activeRows.TryGetValue(player, out var row))
        {
            Destroy(row.gameObject);
            activeRows.Remove(player);
            _rows.Remove(row);
        }
    }    

    public void KillCount()
    {
        _kill++;
        foreach(var row in _rows)
        {
            row.kills.text = _kill.ToString();
        }
    }

    public void DeathCount()
    {
        _death++;
        foreach (var row in _rows)
        {
            row.deaths.text = _death.ToString();
        }
    }

    public void GoldCount(int amount)
    {
        _characterInfo.StartGold += amount;
        foreach (var row in _rows)
        {
            row.golds.text = _characterInfo.StartGold.ToString("N0");
        }
    }    
}