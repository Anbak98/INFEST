using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DownLoadLink : MonoBehaviour
{
    public void OnClick_OpenURL()
    {
        Application.OpenURL("");//���� ����̺� ��ũ
    }

    public void OnClick_OpenURL(string url)
    {
        Application.OpenURL(url);
    }
}
