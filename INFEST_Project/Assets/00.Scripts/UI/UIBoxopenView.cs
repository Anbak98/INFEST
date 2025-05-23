using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIBoxopenView : UIScreen
{
    [HideInInspector] public MysteryBox mysteryBox;

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
        if (mysteryBox.player.Weapons.IsSwitching) return;
        mysteryBox.OpenBox();
    }

    public void CancleBtn()
    {
        Global.Instance.UIManager.Hide<UIBoxopenView>();
    }
}
