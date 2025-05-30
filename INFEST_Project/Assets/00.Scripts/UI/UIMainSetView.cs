using UnityEngine;

public class UIMainSetView : UIScreen
{
    public GameObject sessionOnBtn;
    public GameObject sessionOffBtn;

    public override void OnShow()
    {
        base.OnShow();
        //if(������������)
        //{
        //sessionOnBtn.SetActive(true);
        //sessionOffBtn.SetActive(false);
        //}
        //else if(���Ǿ�����)
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
            //���ǳ�����
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
