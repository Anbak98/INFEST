using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIBase : MonoBehaviour
{
    [HideInInspector]
    public Canvas canvas;

    // Start is called before the first frame update
    void Start()
    {
        Global.Instance.UIManager.Show<UIShopView>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
