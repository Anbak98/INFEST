using System.Collections.Generic;
using Fusion;
using INFEST.Game;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UITutorialAnswer : UIScreen
{
    protected override void Start()
    {
        base.Start();

        AnalyticsManager.SendFunnelStep(3);
    }

    public void OnPressedYesButton()
    {
        PlayerPrefs.SetInt("GameMode", (int)GameMode.Single);
        PlayerPrefs.SetString("RoomCode", "Tutorial");
        FindAnyObjectByType<MatchManager>().PlayerTutorial();
    }

    public void OnPressedNoButton() 
    {
        OnHide();
    }
}