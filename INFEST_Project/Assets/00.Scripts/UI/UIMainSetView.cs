using UnityEngine;

public class UIMainSetView : UIScreen
{
    public GameObject sessionOnBtn;
    public GameObject sessionOffBtn;

    public override void OnShow()
    {
        base.OnShow();
        //if(세션이있을때)
        //{
        //sessionOnBtn.SetActive(true);
        //sessionOffBtn.SetActive(false);
        //}
        //else if(세션없을때)
        //{
        //    sessionOnBtn.SetActive(false);
        //    sessionOffBtn.SetActive(true);
        //}
    }

    public void SessionExitbtn()
    {
        AudioManager.instance.PlaySfx(Sfxs.Click);        
        if(sessionOnBtn.activeSelf == true)
        {
            //세션나가기
        }
    }

    public void SetBtn()
    {
        AudioManager.instance.PlaySfx(Sfxs.Click);
        Global.Instance.UIManager.Show<UISetView>();
    }

    public void ExitBtn()
    {
        AudioManager.instance.PlaySfx(Sfxs.Click);
        Global.Instance.UIManager.Show<UIExitPopup>();
    }
}
