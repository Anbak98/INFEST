using static MatchManager;
using UnityEngine;
using System.Collections.Generic;
using TMPro;
using Fusion;

public class ScreenRoom : UIScreen
{
    [HideInInspector]
    public Room Room;

    [SerializeField] private UIPlayerProfile _uiMyProfile;
    [SerializeField] private UIPlayerProfile[] _uiTeamProfiles;
    [SerializeField] private TMP_Text _roomName;
    [SerializeField] private TMP_InputField _joinSessionCode;
    [SerializeField] private MatchManager _matchManager;
    [SerializeField] private GameObject _playSoloButtonObject;
    [SerializeField] private GameObject _playPartyButtonObject;
    [SerializeField] private GameObject _quickMatchButtonObject;

    protected override void Start()
    {
        base.Start();
        UpdateUI(null);
    }

    public void UpdateUI(List<PlayerProfile> teamProfiles)
    {
        UpdateMyProfile();
        UpdateRoomName();

        if (teamProfiles != null)
            UpdateTeamProfiles(teamProfiles);
    }

    public void UpdateUIWhenJoinRoom()
    {
        foreach(var ui in _uiTeamProfiles)
        {
            ui.gameObject.SetActive(true);
        }

        _playSoloButtonObject.SetActive(false);
        _quickMatchButtonObject.SetActive(false);
    }

    public void SetVisualablePlayPartyButtonOnHost(bool IsHost)
    {
        _playPartyButtonObject.SetActive(IsHost);
    }

    #region Private
    private void UpdateTeamProfiles(List<PlayerProfile> teamProfiles)
    {
        for (int i = 0; i < _uiTeamProfiles.Length; ++i)
        {
            if (i < teamProfiles.Count)
                _uiTeamProfiles[i].Set(teamProfiles[i].Info);
            else
                _uiTeamProfiles[i].Clear();
        }
    }

    private void UpdateMyProfile()
    {
        _uiMyProfile.Set(PlayerPrefs.GetString("Nickname"), (JOB)PlayerPrefs.GetInt("Job"));
    }

    private void UpdateRoomName()
    {
        if (Room != null)
            _roomName.text = Room.Runner.SessionInfo.Name;
    }
    #endregion

    #region Button Methode
    public void OnPressedMedicButton()
    {
        PlayerPrefs.SetInt("Job", (int)JOB.BattleMedic);
        UpdateMyProfile();

        if (Room != null)
            Room.RPC_BroadcastUpdatePlayerProfile();
    }

    public void OnPressedDefanderButton()
    {
        PlayerPrefs.SetInt("Job", (int)JOB.Defender);
        UpdateMyProfile();

        if (Room != null)
            Room.RPC_BroadcastUpdatePlayerProfile();
    }

    public void OnPressedSWATButton()
    {
        PlayerPrefs.SetInt("Job", (int)JOB.Commander);
        UpdateMyProfile();

        if (Room != null)
            Room.RPC_BroadcastUpdatePlayerProfile();
    }

    public void OnPressedDemolatorButton()
    {
        PlayerPrefs.SetInt("Job", (int)JOB.Demolator);
        UpdateMyProfile();

        if (Room != null)
            Room.RPC_BroadcastUpdatePlayerProfile();
    }

    public void OnPressedCreateSession()
    {
        Global.Instance.UIManager.Show<UILoadingPopup>();
        _matchManager.CreateNewSession(true);
    }

    public void OnPressedJoinSession()
    {
        Global.Instance.UIManager.Show<UILoadingPopup>();
        AnalyticsManager.analyticsMatching(2);
        _matchManager.JoinSession(_joinSessionCode.text);
    }

    public void OnPressedQuickMatch()
    {
        Global.Instance.UIManager.Show<UILoadingPopup>();
        AnalyticsManager.analyticsMatching(1);
        _matchManager.QuickMatch();
    }

    public void OnPressedPlaySoloGame()
    {
        Global.Instance.UIManager.Show<UILoadingPopup>();
        AnalyticsManager.analyticsBeforeInGame(10, 1);
        _matchManager.PlayerSoloGame();
    }

    public void OnPressedPlayPartyGame()
    {
        _matchManager.PlayPartyGame();
    }

    public void OnPressedCreateRoom()
    {
        Global.Instance.UIManager.Show<UILoadingPopup>();
        _matchManager.CreateNewSession(false);
    }

    public void OnPressedInviteFriend()
    {
        Global.Instance.UIManager.Show<UILoadingPopup>();
        _matchManager.CreateNewSession(false);
    }
    #endregion
}
