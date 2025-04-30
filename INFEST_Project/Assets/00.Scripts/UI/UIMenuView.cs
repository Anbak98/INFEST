using UnityEngine;
using UnityEngine.UI;

public class UIMenuView : UIScreen
{
    public Button setBtn;
    public Button backBtn;

    public InputManager inputManager;

    public override void Awake()
    {
        base.Awake();
        Hide();        
    }

    public override void Show()
    {
        base.Show();
        inputManager.SetActive(false);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public override void Hide()
    {
        base.Hide();
        inputManager.SetActive(true);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
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
