using UnityEngine;
using UnityEngine.UI;

public class UIMenuView : UIScreen{
    public Button setBtn;
    public Button backBtn;

    public override void Awake()
    {
        base.Awake();
        Hide();                      
    }       

    public void OnClickSetBtn()
    {
        Controller.Get<UISetView>()?.Show();
    }

    public void OnClickBackBtn()
    {
        Hide();
    }    
}
