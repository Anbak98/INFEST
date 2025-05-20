using System.Collections.Generic;
using Fusion;
using INFEST.Game;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UITutorialAnswer : UIScreen
{    
    public void OnPressedYesButton()
    {
        PlayerPrefs.SetInt("GameMode", (int)GameMode.Single);
        PlayerPrefs.SetString("RoomCode", "Tutorial");
        SceneManager.LoadScene("Tutorial");
    }

    public void OnPressedNoButton() 
    {
        Hide();
    }
}