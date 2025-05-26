using UnityEngine;

public class SettingBtn : MonoBehaviour
{
    public void OnClickBtn()
    {
        Global.Instance.UIManager.Show<UISetView>();
    }
}
