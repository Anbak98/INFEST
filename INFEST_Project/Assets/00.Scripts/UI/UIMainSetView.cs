using UnityEngine;

public class UIMainSetView : UIScreen
{
    public void SessionExitbtn()
    {
        AudioManager.instance.PlaySfx(Sfxs.Click);
    }

    public void SetBtn()
    {
        AudioManager.instance.PlaySfx(Sfxs.Click);
        Global.Instance.UIManager.Show<UISetView>();
    }

    public void ExitBtn()
    {
        AudioManager.instance.PlaySfx(Sfxs.Click);
    }
}
