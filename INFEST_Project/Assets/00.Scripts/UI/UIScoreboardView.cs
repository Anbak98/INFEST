using System.Collections.Generic;
using Fusion;

public class UIScoreboardView : UIScreen
{
    public UIScoreboardRow uirow;

    private List<UIScoreboardRow> _rows = new(32);
    private List<UIPlayerData> _players = new(32);

    private TestGameUI _gameUI;

    public override void Awake()
    {
        base.Awake();
        _rows.Add(uirow);

        _gameUI = GetComponentInParent<TestGameUI>();
    }

    public override void Init()
    {
        base.Init();
        Hide();
    }

    public override void Show()
    {
        base.Show();
        UpdateScoreboard();
    }

    public override void Hide()
    {
        base.Hide();
    }

    private void UpdateScoreboard()
    {
        if (_gameUI.runner == null)
            return;

        _players.Clear();

        foreach (var record in _gameUI.testPlay.PlayerData)
        {
            _players.Add(record.Value);
        }

        PlayerRef localPlayer = _gameUI.runner.LocalPlayer;
        _players.Sort((a, b) =>
        {
            bool aIsLocal = a.PlayerRef == localPlayer;
            bool bIsLocal = b.PlayerRef == localPlayer;

            if (aIsLocal && !bIsLocal) return -1;
            if (!aIsLocal && bIsLocal) return 1;

            return a.StatisticPosition.CompareTo(b.StatisticPosition);
        });

        PrepareRows(_players.Count);
        UpdateRows();
    }

    private void PrepareRows(int playerCount)
    {
        for (int i = _rows.Count; i < playerCount; i++)
        {
            var row = Instantiate(uirow, uirow.transform.parent);
            row.gameObject.SetActive(true);

            _rows.Add(row);
        }

        // Activate correct count of rows
        for (int i = 0; i < _rows.Count; i++)
        {
            _rows[i].gameObject.SetActive(i < playerCount);
        }
    }

    private void UpdateRows()
    {
        for (int i = 0; i < _players.Count; i++)
        {
            var row = _rows[i];
            var data = _players[i];

            row.nickName.text = data.Nickname;
            row.kills.text = data.Kills.ToString();
            row.deaths.text = data.Deaths.ToString();
            row.golds.text = data.Golds.ToString();
        }
    }
}
