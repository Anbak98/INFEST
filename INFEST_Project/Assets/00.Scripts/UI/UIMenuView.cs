using UnityEngine;

public class UIMenuView : UIScreen
{
    public override void Awake()
    {
        base.Awake();
    }

    public override void OnShow()
    {
        base.OnShow();
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        AudioManager.instance.PlaySfx(Sfxs.Click);
    }

    public override void OnHide()
    {
        base.OnHide();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void OnClickSetBtn()
    {
        AudioManager.instance.PlaySfx(Sfxs.Click);
        Global.Instance.UIManager.Show<UISetView>();
    }

    public void OnClickBackBtn()
    {
        AudioManager.instance.PlaySfx(Sfxs.Click);
        Global.Instance.UIManager.Hide<UIMenuView>();        
    }
}
