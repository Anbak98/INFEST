using UnityEngine.UI;

public class UIMenuView : UIScreen
{
    public Button setBtn;
    public Button backBtn;

    public override void Awake()
    {
        base.Awake();
        Hide();        
    }

    public override void Show()
    {
        base.Show();        
    }

    public override void Hide()
    {
        base.Hide();        
    }

    public void OnClickSetBtn()
    {
        //Controller.Get<UISetView>()?.Show();
        Global.Instance.UIManager.Show<UISetView>();
    }

    public void OnClickBackBtn()
    {
        Hide();                
    }
}
