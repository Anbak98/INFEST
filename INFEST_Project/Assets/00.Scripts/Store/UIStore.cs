using System;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.UI;
using Fusion;
using UnityEngine.InputSystem;

public enum EBtnType
{
    SaleauxiliaryWeapon,
    Weapon,
    Consume
}

public class UIStore : UIBase // ������ ���� UI�� ����ִ�.
{
    private NetworkObject _networkObject;
    private Player _player;
    // �⺻ ����
    public TextMeshProUGUI interactionText;             // ���� ���� ������ �ؽ�Ʈ
    public Image panel;                                 // ���� ȭ��

    // ���� ȭ�� ����
    public List<Button> buyButton;                      // ���� ��ư
    public List<TextMeshProUGUI> buyPiceText;           // ������ ���� ����
    public List<TextMeshProUGUI> buyNameText;           // �Ǹ� ������ �̸�


    // ���� ȭ�� ����
    public List<Image> icon;                            // ���� ������ �̹���
    public List<Button> saleWeaponButton;               // �ֹ��� �Ǹ� ��ư
    public Button saleauxiliaryWeaponButton;            // �������� �Ǹ� ��ư
    public List<Button> saleconsumeButton;              // �ֹ��� �Ǹ� ��ư
    public List<TextMeshProUGUI> salePriceText;         // �Ǹ� �ؽ�Ʈ \n �Ǹ� ����
    public List<Button> possessItemButton;              // ���� ������ ���� ��ư
    public List<TextMeshProUGUI> priceBullet;           // źâ �ؽ�Ʈ \n źâ ���� ����
    public Button repairButton;                         // ���� ��ư
    public TextMeshProUGUI repairText;                  // ���� �ؽ�Ʈ \n ���� �ݾ� ǥ��
    public Button AIIReplenishButton;                   // ��ü ����(�Ѿ� & ����)
    public TextMeshProUGUI AIIReplenishText;            // ��ü ���� �ؽ�Ʈ \n ��ü ���� �ݾ� ǥ��


    private void Start()
    {
        _player = GetComponentInParent<Player>();
        _networkObject = GetComponentInParent<NetworkObject>();
        Init();
        panel.gameObject.SetActive(false);
        interactionText.gameObject.SetActive(false);
        
    }

    private void Init()
    {
        
        for (int i = 0; i < buyButton.Count; i++)
        {
            if (buyButton[i] == null) return;
            if (buyButton[i] != null) buyButton[i].onClick.AddListener(() => _player.store.RPC_RequestTryBuy(_networkObject, _networkObject.InputAuthority, i));
            if(i < 2)
                if (saleWeaponButton[i] != null) saleWeaponButton[i].onClick.AddListener(() => _player.store.RPC_RequestTrySale(_networkObject, _networkObject.InputAuthority, i, EBtnType.Weapon));
            if(i < 3)
                if (saleconsumeButton[i] != null) saleconsumeButton[i].onClick.AddListener(() => _player.store.RPC_RequestTrySale(_networkObject, _networkObject.InputAuthority, i, EBtnType.Consume));
        }
        if (saleauxiliaryWeaponButton != null) saleauxiliaryWeaponButton.onClick.AddListener(() => _player.store.RPC_RequestTrySale(_networkObject, _networkObject.InputAuthority, 0, EBtnType.SaleauxiliaryWeapon));
    }

    private void UIRefresh(string text, int key)
    {
        DataManager.Instance.GetByKey<WeaponInfo>(key);
    }
}
