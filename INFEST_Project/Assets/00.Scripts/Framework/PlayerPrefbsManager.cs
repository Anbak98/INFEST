using Fusion;
using UnityEngine;

public class PlayerPrefsManager
{
    public static void SetNickname(string nickname) => PlayerPrefs.SetString("Nickname", nickname);
    public static string GetNickname() => PlayerPrefs.GetString("Nickname", "Unknown");

    public static void SetJob(JOB job) => PlayerPrefs.SetInt("JJob", (int)job);
    public static JOB GetJob() => (JOB)PlayerPrefs.GetInt("JJob", 0);

    public static void SetGameMode(GameMode gameMode) => PlayerPrefs.SetInt("GameMode", (int)gameMode);
    public static GameMode GetGameMode() => (GameMode)PlayerPrefs.GetInt("GameMode", (int)GameMode.Single);

    public static void SetSessionName(string sessionName) => PlayerPrefs.SetString("SessionName", sessionName);
    public static string GetSessionName() => PlayerPrefs.GetString("SessionName", "NO_DEFINED");
}
