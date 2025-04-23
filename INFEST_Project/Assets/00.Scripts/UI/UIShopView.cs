using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIShopView : UIScreen
{
    private Store _store;
    [Header("Interaction")]
    public Image bg;
    public TextMeshProUGUI interactionText;

    [Header("Job")]
    public Image jobImage;
    public TextMeshProUGUI jobText;

    [Header("Supplement")]
    public Image defIcon;
    public TextMeshProUGUI defText;
    public Image weaponIcon1;
    public TextMeshProUGUI weaponText1;
    public Image weaponIcon2;
    public TextMeshProUGUI weaponText2;
    public Image itemWeaponIcon;
    public TextMeshProUGUI itemWeaponText;

    [Header("Gold")]
    public TextMeshProUGUI goldText;

    [Header("BuyWeapon")]
    public TextMeshProUGUI[] weaponNames;

    [Header("Button")]
    public List<Button> buyButton;                      // 구매 버튼
    public List<TextMeshProUGUI> buyButtonText;         // 구매 버튼 텍스트
    public List<Button> saleButton;                     // 판매 버튼
    public List<Button> possessItemButton;              // 소유 아이템 구매 버튼

    WeaponInfo _weaponInfo;
    CharacterInfo _characterInfo;


    public override void Awake()
    {
        _characterInfo = DataManager.Instance.GetByKey<CharacterInfo>(1);
        DefSet();
    }

    protected override void Start()
    {
        base.Start();
        bg.gameObject.SetActive(false);
        interactionText.gameObject.SetActive(false);
    }

    public override void Show()
    {
        base.Show();
    }

    public override void Hide()
    {
        base.Hide();
    }

    private void DefSet()
    {
        defText.text = $"{_characterInfo.DefGear}/200";
    }

    private void GoldSet()
    {
        goldText.text = $"{Player.local.gold}";
    }

    public void OnClickDefBtn()
    {
        if (_characterInfo.DefGear >= 200)
        {
            return;
        }

        _characterInfo.DefGear += 200;
        _characterInfo.DefGear = Mathf.Min(_characterInfo.DefGear, 200);
        DefSet();
    }

    public void OnClickBuyBtn(int index)
    {
        _store.RPC_RequestTryBuy(Player.local, Player.local.networkObject.InputAuthority, index);
        UpdateButtonState();
    }

    public void OnClickSaleBtn(int index)
    {
        _store.RPC_RequestTrySale(Player.local, Player.local.networkObject.InputAuthority, index);
        UpdateButtonState();    
    }

    public void StoreInIt(Store store)
    {
        _store = store;
    }
    /// <summary>
    /// 구매버튼 업데이트
    /// </summary>
    public void UpdateButtonState()
    {
        var _inv = Player.local.inventory;
        for (int i = 0; i < buyButton.Count; i++)
        {
            {
                if (Player.local == null || Player.local.inventory == null || _store == null || i >= _store.idList.Count)
                    return;

                int weaponPrice = DataManager.Instance.GetByKey<WeaponInfo>(_store.idList[i])?.Price ?? 0;
                int consumePrice = DataManager.Instance.GetByKey<ConsumeItem>(_store.idList[i])?.Price ?? 0;

                int _itemKey = _store.idList[i] % 10000;
                #region 체크용 bool 값
                bool auxiliaryWeaponChk = _itemKey < 200;
                bool weaponChk = _itemKey < 600 && _itemKey > 200;
                bool throwingWeapon = _itemKey < 800 && _itemKey > 600;
                bool recoveryItem = _itemKey < 900 && _itemKey > 800; 
                bool shieldItme = _itemKey < 1000 && _itemKey > 900;
                #endregion

                if ((auxiliaryWeaponChk && _inv.auxiliaryWeapon[0] != null) || (Player.local.gold < weaponPrice))
                    buyButton[i].interactable = false;

                else if ((weaponChk && _inv.weapon[0] != null && _inv.weapon[1] != null) || Player.local.gold < weaponPrice)
                    buyButton[i].interactable = false;

                else if ((throwingWeapon && _inv.consume[0] != null) || Player.local.gold < consumePrice)
                {
                    if (_inv.consume[0].data.key != _store.idList[i] || _inv.consume[0].curNum == _inv.consume[0].data.MaxNum)
                        buyButton[i].interactable = false;
                }

                else if ((recoveryItem && _inv.consume[1] != null) || Player.local.gold < consumePrice)
                {
                    if (_inv.consume[1].data.key != _store.idList[i] || _inv.consume[1].curNum == _inv.consume[1].data.MaxNum)
                        buyButton[i].interactable = false;
                }

                else if ((shieldItme && _inv.consume[2] != null) || Player.local.gold < consumePrice)
                {
                    if (_inv.consume[2].data.key != _store.idList[i] || _inv.consume[2].curNum == _inv.consume[2].data.MaxNum)
                        buyButton[i].interactable = false;
                }

                else
                    buyButton[i].interactable = true;
            }

            if (buyButton[i].interactable)
            {
                buyButtonText[i].color = Color.white; // 활성화 시 흰색
            }
            else
            {
                buyButtonText[i].color = Color.red;   // 비활성화 시 빨간색
            }
            GoldSet();
        }
    }
}
