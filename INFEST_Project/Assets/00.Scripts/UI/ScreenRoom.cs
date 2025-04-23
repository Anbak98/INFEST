using static MatchManager;
using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class ScreenRoom : UIScreen
{
    [HideInInspector]
    public Room Room;

    [SerializeField] private UIPlayerProfile _uiMyProfile;
    [SerializeField] private UIPlayerProfile[] _uiTeamProfiles;
    [SerializeField] private TMP_Text _roomName;
    [SerializeField] private TMP_InputField _joinSessionCode;
    [SerializeField] private MatchManager _matchManager;

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

    #region Button Methode
    public void OnPressedMedicButton()
    {
        PlayerPrefs.SetInt("Job", (int)JOB.Medic);
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
        PlayerPrefs.SetInt("Job", (int)JOB.SWAT);
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
        _matchManager.CreateNewSession(true, GameType.BossHunt, GameMap.MVP);
    }

    public void OnPressedJoinSession()
    {
        _matchManager.JoinSession(_joinSessionCode.text);
    }

    public void OnPressedQuickMatch()
    {
        _matchManager.QuickMatch();
    }

    public void OnPressedPlayGame()
    {
        _matchManager.PlayGame();   
    }

    public void OnPressedInviteFriend()
    {
        _matchManager.CreateNewSession(false, GameType.BossHunt, GameMap.MVP);
    }
    #endregion
}
