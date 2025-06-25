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
    [SerializeField] private TMP_InputField _sessionCode;
    [SerializeField] private MatchManager _matchManager;
    [SerializeField] private GameObject _playSoloButtonObject;
    [SerializeField] private GameObject _playPartyButtonObject;
    [SerializeField] private GameObject _quickMatchButtonObject;
    [SerializeField] private GameObject[] _playerOnlyModels;
    [SerializeField] private GameObject _uiJoinSessionCode;
    [SerializeField] private GameObject _uICopySessionCode;

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

        _uICopySessionCode.SetActive(true);
        _uiJoinSessionCode.SetActive(false);
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
        foreach(var obj in _playerOnlyModels)
        {
            obj.SetActive(false);
        }

        for (int i = 0; i < _uiTeamProfiles.Length; ++i)
        {
            if (i < teamProfiles.Count)
            {
                _uiTeamProfiles[i].Set(teamProfiles[i].Info);
                foreach (var model in _playerOnlyModels)
                {
                    if(model.activeSelf == false)
                    {
                        model.SetActive(true);
                        break;
                    }
                }
            }
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
        AudioManager.instance.PlaySfx(Sfxs.Click);

        if (Room != null)
            Room.RPC_BroadcastUpdatePlayerProfile();
    }

    public void OnPressedDefanderButton()
    {
        PlayerPrefs.SetInt("Job", (int)JOB.Defender);
        UpdateMyProfile();
        AudioManager.instance.PlaySfx(Sfxs.Click);

        if (Room != null)
            Room.RPC_BroadcastUpdatePlayerProfile();
    }

    public void OnPressedSWATButton()
    {
        PlayerPrefs.SetInt("Job", (int)JOB.Commander);
        UpdateMyProfile();
        AudioManager.instance.PlaySfx(Sfxs.Click);

        if (Room != null)
            Room.RPC_BroadcastUpdatePlayerProfile();
    }

    public void OnPressedDemolatorButton()
    {
        PlayerPrefs.SetInt("Job", (int)JOB.Demolator);
        UpdateMyProfile();
        AudioManager.instance.PlaySfx(Sfxs.Click);

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
        AudioManager.instance.PlaySfx(Sfxs.Click);
        Global.Instance.UIManager.Show<UILoadingPopup>();
        AnalyticsManager.analyticsMatching(2);
        _matchManager.JoinSession(_sessionCode.text);
    }

    public void OnPressedQuickMatch()
    {
        AudioManager.instance.PlaySfx(Sfxs.Click);
        Global.Instance.UIManager.Show<UILoadingPopup>();
        AnalyticsManager.analyticsMatching(1);
        _matchManager.QuickMatch();
    }

    public void OnPressedPlaySoloGame()
    {
        AudioManager.instance.PlaySfx(Sfxs.Click);
        Global.Instance.UIManager.Show<UILoadingPopup>();
        AnalyticsManager.analyticsBeforeInGame(10, 1);
        _matchManager.PlayerSoloGame();
    }

    public void OnPressedPlayPartyGame()
    {
        AudioManager.instance.PlaySfx(Sfxs.Click);
        _matchManager.PlayPartyGame();
    }

    public void OnPressedCreateRoom()
    {
        AudioManager.instance.PlaySfx(Sfxs.Click);
        Global.Instance.UIManager.Show<UILoadingPopup>();
        _matchManager.CreateNewSession(false);
    }

    public void OnPressedInviteFriend()
    {
        AudioManager.instance.PlaySfx(Sfxs.Click);
        Global.Instance.UIManager.Show<UILoadingPopup>();
        _matchManager.CreateNewSession(false);
    }
    public void OnPressedCopyutton()
    {
        AudioManager.instance.PlaySfx(Sfxs.Click);
        GUIUtility.systemCopyBuffer = _roomName.text;
    }
    #endregion
}
