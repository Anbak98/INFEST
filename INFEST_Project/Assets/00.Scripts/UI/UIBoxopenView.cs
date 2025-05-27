using INFEST.Game;
using UnityEngine;

public class UIBoxopenView : UIScreen
{
    [HideInInspector] public MysteryBox mysteryBox;

    public override void OnHide()
    {
        base.OnHide();
        NetworkGameManager.Instance.inputManager.ShopSetActive(true);
    }

    public override void OnShow()
    {
        base.OnShow();
        NetworkGameManager.Instance.inputManager.ShopSetActive(false);
    }

    public void OkBtn()
    {
//#if UNITY_EDITOR
//        mysteryBox.player.statHandler.CurGold += 2000;
//#endif

//        if (mysteryBox.player.statHandler.CurGold < 2000) return;
//        mysteryBox.player.statHandler.CurGold -= 2000;

        AudioManager.instance.PlaySfx(Sfxs.Click);
        mysteryBox.OpenBox();
    }

    public void CancleBtn()
    {
        AudioManager.instance.PlaySfx(Sfxs.Click);
        Global.Instance.UIManager.Hide<UIBoxopenView>();
        Global.Instance.UIManager.Show<UIInteractiveView>();
    }
}
