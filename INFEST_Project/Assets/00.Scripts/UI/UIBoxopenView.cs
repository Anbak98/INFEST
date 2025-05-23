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
#if UNITY_EDITOR
        mysteryBox.player.statHandler.CurGold += 2000;
#endif

        if (mysteryBox.player.statHandler.CurGold < 2000) return;
        mysteryBox.OpenBox();
    }

    public void CancleBtn()
    {
        Global.Instance.UIManager.Hide<UIBoxopenView>();
        Global.Instance.UIManager.Show<UIInteractiveView>();
    }
}
