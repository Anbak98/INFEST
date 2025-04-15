using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIStateView : UIScreen
{
    public override void Awake()
    {
        base.Awake();        
    }    

    public override void Show()
    {
        base.Show();
        Debug.Log("ÄÑÁü");
    }

    public override void Hide()
    {
        base.Hide();
        Debug.Log("²¨Áü");
    }
}
