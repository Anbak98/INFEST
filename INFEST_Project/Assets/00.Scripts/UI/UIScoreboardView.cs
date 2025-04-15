using System.Collections.Generic;
using Fusion;
using UnityEngine;

public class UIScoreboardView : UIScreen
{
    public float disconnectedPlayerAlpha = 0.4f;
    private UIController _uiController;
    public UIScoreboardRow uirow;

    private List<UIScoreboardRow> _rows = new(32);


    public override void Awake()
    {
        base.Awake();
        _uiController = GetComponentInParent<UIController>();
        _rows.Add(uirow);
    }

    public override void Init()
    {
        base.Init();
        Hide();
    }

    public override void Show()
    {
        base.Show();
        Debug.Log("ÄÑÁü");
    }

    public override void Hide()
    {
        base.Hide();
        Debug.Log("²¨Áü");
    }
}
