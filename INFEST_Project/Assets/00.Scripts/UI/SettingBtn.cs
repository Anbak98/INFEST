using UnityEngine;

public class SettingBtn : MonoBehaviour
{
    public void OnClickBtn()
    {
        AudioManager.instance.PlaySfx(Sfxs.Click);
        Global.Instance.UIManager.Show<UIMainSetView>();
    }
}
