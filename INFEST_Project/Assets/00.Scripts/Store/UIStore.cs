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

public class UIStore : UIBase // 유저의 상점 UI를 담고있다.
{
    private NetworkObject _networkObject;
    private Player _player;
    // 기본 세팅
    public TextMeshProUGUI interactionText;             // 존에 들어가면 나오는 텍스트
    public Image panel;                                 // 상점 화면

    // 상점 화면 우측
    public List<Button> buyButton;                      // 구매 버튼
    public List<TextMeshProUGUI> buyPiceText;           // 아이템 구매 가격
    public List<TextMeshProUGUI> buyNameText;           // 판매 아이템 이름


    // 상점 화면 좌측
    public List<Image> icon;                            // 소유 아이템 이미지
    public List<Button> saleWeaponButton;               // 주무기 판매 버튼
    public Button saleauxiliaryWeaponButton;            // 보조무기 판매 버튼
    public List<Button> saleconsumeButton;              // 주무기 판매 버튼
    public List<TextMeshProUGUI> salePriceText;         // 판매 텍스트 \n 판매 가격
    public List<Button> possessItemButton;              // 소유 아이템 구매 버튼
    public List<TextMeshProUGUI> priceBullet;           // 탄창 텍스트 \n 탄창 구매 가격
    public Button repairButton;                         // 수리 버튼
    public TextMeshProUGUI repairText;                  // 수리 텍스트 \n 수리 금액 표시
    public Button AIIReplenishButton;                   // 전체 보충(총알 & 구매)
    public TextMeshProUGUI AIIReplenishText;            // 전체 보충 텍스트 \n 전체 보충 금액 표시


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
