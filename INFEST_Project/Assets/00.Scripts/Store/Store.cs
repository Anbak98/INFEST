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
    // Ÿ�̸�
    private TickTimer _storeTimer;
    private float _newStoreTime = 300f;
    private float _activateTime = 120f;
    
    
    //public Button[] btnBuy; // ���Ź�ư
    //public Button[] btnSell; // �ǸŹ�ư
    //public TextMeshProUGUI[] itmePriceBuy; // ���� ����
    //public TextMeshProUGUI[] btnPriceSell; // �Ǹ� ����


    /// <summary>
    /// ���� ������ Ÿ�̸� Ȱ��ȭ
    /// </summary>
    public void ActivateTimer()
    {
       // _storeTimer = TickTimer.CreateFromSeconds(Runner, _activateTime);
    }

    /// <summary>
    /// ���� ��ȣ�ۿ�� Ÿ�̸� Ȱ��ȭ
    /// </summary>
    public void NewStoreTimer()
    {
       // _storeTimer = TickTimer.CreateFromSeconds(Runner, _newStoreTime);
    }

    /// <summary>
    /// Ʈ���� ���˽�
    /// </summary>
    /// <param name="other"></param>
    public void OnTriggerEnter(Collider other)
    {
        Debug.Log("Ʈ���� IN");
        Global.Instance.UIManager.Show<UIStore>().gameObject.SetActive(true);
    }

    /// <summary>
    /// Ʈ���� ������
    /// </summary>
    /// <param name="other"></param>
    public void OnTriggerExit(Collider other)
    {

        Debug.Log("Ʈ���� OUT");
        //if (other.tag == "Player") 
        //Global.Instance.UIManager.Show<UIStore>.gameObject.SetActive(false);
    }

}
