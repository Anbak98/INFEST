using System.Collections;
using System.Collections.Generic;
using Fusion;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Store : NetworkBehaviour
{
    private LayerMask layerMask;
    // 타이머
    private TickTimer _storeTimer;
    private float _newStoreTime = 300f;
    private float _activateTime = 120f;
    
    
    //public Button[] btnBuy; // 구매버튼
    //public Button[] btnSell; // 판매버튼
    //public TextMeshProUGUI[] itmePriceBuy; // 구매 가격
    //public TextMeshProUGUI[] btnPriceSell; // 판매 가격


    /// <summary>
    /// 상점 생성시 타이머 활성화
    /// </summary>
    public void ActivateTimer()
    {
       // _storeTimer = TickTimer.CreateFromSeconds(Runner, _activateTime);
    }

    /// <summary>
    /// 상점 상호작용시 타이머 활성화
    /// </summary>
    public void NewStoreTimer()
    {
       // _storeTimer = TickTimer.CreateFromSeconds(Runner, _newStoreTime);
    }

    /// <summary>
    /// 트리거 접촉시
    /// </summary>
    /// <param name="other"></param>
    public void OnTriggerEnter(Collider other)
    {
        Debug.Log("트리거 IN");
        Global.Instance.UIManager.Show<UIStore>().gameObject.SetActive(true);
    }

    /// <summary>
    /// 트리거 나갈시
    /// </summary>
    /// <param name="other"></param>
    public void OnTriggerExit(Collider other)
    {

        Debug.Log("트리거 OUT");
        //if (other.tag == "Player") 
        //Global.Instance.UIManager.Show<UIStore>.gameObject.SetActive(false);
    }

}
