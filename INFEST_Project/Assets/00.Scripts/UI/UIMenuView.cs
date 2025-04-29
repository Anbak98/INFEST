using UnityEngine;
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
        PlayerCameraHandler playerCameraHandler = FindObjectOfType<PlayerCameraHandler>();
        playerCameraHandler.LockCamera(true);        
    }

    public override void Hide()
    {
        base.Hide();
        PlayerCameraHandler playerCameraHandler = FindObjectOfType<PlayerCameraHandler>();
        playerCameraHandler.LockCamera(false);
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
