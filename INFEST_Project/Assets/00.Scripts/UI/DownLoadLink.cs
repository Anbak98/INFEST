using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DownLoadLink : MonoBehaviour
{
    public void OnClick_OpenURL()
    {
        Application.OpenURL("");//구글 드라이브 링크
    }

    public void OnClick_OpenURL(string url)
    {
        Application.OpenURL(url);
    }
}
