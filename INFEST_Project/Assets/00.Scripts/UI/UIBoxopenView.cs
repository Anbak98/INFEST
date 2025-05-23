using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIBoxopenView : UIScreen
{
    public override void OnHide()
    {
        base.OnHide();
    }

    public override void OnShow()
    {
        base.OnShow();
    }

    public void OkBtn()
    {

    }

    public void CancleBtn()
    {
        Global.Instance.UIManager.Hide<UIBoxopenView>();
    }
}
