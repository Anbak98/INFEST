using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DownLoadLink : MonoBehaviour
{
    public void OnClick_OpenURL()
    {
        Application.OpenURL("https://drive.google.com/file/d/1WxN30yHi-k7sNujvA6iWThqjcNWfL9Zo/view?usp=sharing");//구글 드라이브 링크
    }

    public void OnClick_OpenURL(string url)
    {
        Application.OpenURL(url);
    }

    public void LoadTutorail()
    {
        MatchManager.Instance.PlayerTutorial();
    }
}
