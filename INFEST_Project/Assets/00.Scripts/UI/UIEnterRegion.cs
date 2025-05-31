using UnityEngine;

public class UIEnterRegion : UIScreen
{
    public TMPro.TMP_Text RegionName;
    public TMPro.TMP_Text Tooltip;

    public override void OnShow()
    {
        base.OnShow();
        AudioManager.instance.PlaySfx(Sfxs.Click);
    }

    public void SetText(string RegionName, string Tooltip)
    {
        this.RegionName.text = RegionName;
        this.Tooltip.text = Tooltip;
    }

    public void OnAnimationUIHideEnd()
    {
        Debug.Log("e");
        Global.Instance.UIManager.Hide<UIEnterRegion>();
    }
}
