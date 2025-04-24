using System.Collections.Generic;
using Fusion;
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
    public Image comender;
    public Image medic;
    public Image demol;
    public Image warrior;
    public TextMeshProUGUI jobText;

    [Header("Supplement")]
    public TextMeshProUGUI allSupplement;
    public Image defIcon;
    public TextMeshProUGUI defText;
    public Image subWeapon;
    public TextMeshProUGUI subWeaponText;
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

    WeaponInfo _subWeaponInfo;
    WeaponInfo _mainWeaponInfo;
    WeaponInfo _mainWeaponInfo2;
    ConsumeItem _itemInfo;
    CharacterInfo _characterInfo;

    [Header("CurBullet")]
    public int subCurBullet;
    public int main1CurBullet;
    public int main2CurBullet;
    public int consumeItem;

    [Header("BulletPrice")]
    public TextMeshProUGUI subWeaponBullet;
    public TextMeshProUGUI mainWeapon1Bullet;
    public TextMeshProUGUI mainWeapon2Bullet;
    public TextMeshProUGUI itemPrice;

    [Header("WeaponName")]
    public TextMeshProUGUI subWeaponName;
    public TextMeshProUGUI mainWeapon1Name;
    public TextMeshProUGUI mainWeapon2Name;
    public TextMeshProUGUI itemName;

    [Networked]
    public Profile Info { get; set; }


    public override void Awake()
    {
        //_characterInfo = DataManager.Instance.GetByKey<CharacterInfo>(1);
        _subWeaponInfo = DataManager.Instance.GetByKey<WeaponInfo>(10102);
        _mainWeaponInfo = DataManager.Instance.GetByKey<WeaponInfo>(10201);
        _mainWeaponInfo2 = DataManager.Instance.GetByKey<WeaponInfo>(10303);
        _itemInfo = DataManager.Instance.GetByKey<ConsumeItem>(10701);

        subCurBullet = _subWeaponInfo.MagazineBullet;
        main1CurBullet = _mainWeaponInfo.MagazineBullet;
        main2CurBullet = _mainWeaponInfo2.MagazineBullet;
        consumeItem = _itemInfo.MaxNum;

        ChoiceJob();
        SetJobIcon();
        DefSet();
        UpdateJobIcon();
    }

    protected override void Start()
    {
        base.Start();
        bg.gameObject.SetActive(false);
        interactionText.gameObject.SetActive(false);
    }

    private void Update()
    {
        //DefSet();
        //SubWeaponSet();
        //MainWeapon1Set();
        //MainWeapon2Set();
        //ItemSet();
        //AllSupplementSet();
        //GoldSet();
    }

    public override void Show()
    {
        base.Show();
    }

    public override void Hide()
    {
        base.Hide();
    }

    private void SetJobIcon()
    {
        comender.gameObject.SetActive(false);
        medic.gameObject.SetActive(false);
        demol.gameObject.SetActive(false);
        warrior.gameObject.SetActive(false);
    }

    private void ChoiceJob()
    {
        if (Info.Job == JOB.SWAT)
        {
            _characterInfo = DataManager.Instance.GetByKey<CharacterInfo>(1);
        }
        else if (Info.Job == JOB.Medic)
        {
            _characterInfo = DataManager.Instance.GetByKey<CharacterInfo>(2);
        }
        else if (Info.Job == JOB.Demolator)
        {
            _characterInfo = DataManager.Instance.GetByKey<CharacterInfo>(3);
        }
        else if (Info.Job == JOB.Defender)
        {
            _characterInfo = DataManager.Instance.GetByKey<CharacterInfo>(4);
        }
    }

    private void UpdateJobIcon()
    {
        switch (_characterInfo.key)
        {
            case 1:
                comender.gameObject.SetActive(true);
                break;
            case 2:
                medic.gameObject.SetActive(true);
                break;
            case 3:
                demol.gameObject.SetActive(true);
                break;
            case 4:
                warrior.gameObject.SetActive(true);
                break;
        }
    }

    private void DefSet()
    {
        defText.text = $"{_characterInfo.DefGear}/200";
    }

    private void GoldSet()
    {
        goldText.text = $"{Player.local.gold}";
    }

    private void SubWeaponSet()
    {
        subWeaponText.text = $"{subCurBullet}/{_subWeaponInfo.MaxBullet}";
        subWeaponBullet.text = $"탄창\n{(_subWeaponInfo.MagazineBullet - subCurBullet) * _subWeaponInfo.BulletPrice}G";
        subWeaponName.text = $"{_subWeaponInfo.Name}";
    }

    private void MainWeapon1Set()
    {
        weaponText1.text = $"{main1CurBullet}/{_mainWeaponInfo.MaxBullet}";
        mainWeapon1Bullet.text = $"탄창\n{(_mainWeaponInfo.MagazineBullet - main1CurBullet) * _mainWeaponInfo.BulletPrice}G";
        mainWeapon1Name.text = $"{_mainWeaponInfo.Name}";
    }

    private void MainWeapon2Set()
    {
        weaponText2.text = $"{main2CurBullet}/{_mainWeaponInfo2.MaxBullet}";
        mainWeapon2Bullet.text = $"탄창\n{(_mainWeaponInfo2.MagazineBullet - main2CurBullet) * _mainWeaponInfo2.BulletPrice}G";
        mainWeapon2Name.text = $"{_mainWeaponInfo2.Name}";
    }

    private void ItemSet()
    {
        itemWeaponText.text = $"{consumeItem}/{_itemInfo.MaxNum}";
        itemPrice.text = $"낱개 구매\n{_itemInfo.Price}G";
        itemName.text = $"{_itemInfo.Name}";
    }

    private void AllSupplementSet()
    {
        int subPrice = (_subWeaponInfo.MagazineBullet - subCurBullet) * _subWeaponInfo.BulletPrice;
        int main1Price = (_mainWeaponInfo.MagazineBullet - main1CurBullet) * _mainWeaponInfo.BulletPrice;
        int main2Price = (_mainWeaponInfo2.MagazineBullet - main2CurBullet) * _mainWeaponInfo2.BulletPrice;
        int itemPrice = (_itemInfo.MaxNum - consumeItem) * _itemInfo.Price;

        if (_characterInfo.DefGear >= 200)
        {
            allSupplement.text = $"모두 보충\n({subPrice + main1Price + main2Price + itemPrice}G)";
        }
        else
        {
            allSupplement.text = $"모두 보충\n({500 + subPrice + main1Price + main2Price + itemPrice}G)";
        }
    }

    public void OnClickAllBtn()
    {
        int subPrice = (_subWeaponInfo.MagazineBullet - subCurBullet) * _subWeaponInfo.BulletPrice;
        int main1Price = (_mainWeaponInfo.MagazineBullet - main1CurBullet) * _mainWeaponInfo.BulletPrice;
        int main2Price = (_mainWeaponInfo2.MagazineBullet - main2CurBullet) * _mainWeaponInfo2.BulletPrice;
        int itemPrice = (_itemInfo.MaxNum - consumeItem) * _itemInfo.Price;

        if (_characterInfo.DefGear >= 200)
        {
            Player.local.gold -= subPrice + main1Price + main2Price + itemPrice;
        }
        else
        {
            Player.local.gold -= 500 + subPrice + main1Price + main2Price + itemPrice;
        }

        _characterInfo.DefGear += 200;
        _characterInfo.DefGear = Mathf.Min(_characterInfo.DefGear, 200);

        subCurBullet += _subWeaponInfo.MagazineBullet;
        subCurBullet = Mathf.Min(subCurBullet, _subWeaponInfo.MagazineBullet);

        main1CurBullet += _mainWeaponInfo.MagazineBullet;
        main1CurBullet = Mathf.Min(main1CurBullet, _mainWeaponInfo.MagazineBullet);

        main2CurBullet += _mainWeaponInfo2.MagazineBullet;
        main2CurBullet = Mathf.Min(main2CurBullet, _mainWeaponInfo2.MagazineBullet);

        consumeItem += _itemInfo.MaxNum;
        consumeItem = Mathf.Min(consumeItem, _itemInfo.MaxNum);
    }

    public void OnClickDefBtn()
    {
        if (_characterInfo.DefGear >= 200)
            return;

        Player.local.gold -= 500;
        _characterInfo.DefGear += 200;
        _characterInfo.DefGear = Mathf.Min(_characterInfo.DefGear, 200);   
    }

    public void OnClickSubWeaponBtn()
    {
        if (subCurBullet >= _subWeaponInfo.MagazineBullet)
            return;

        Player.local.gold -= _subWeaponInfo.BulletPrice * (_subWeaponInfo.MagazineBullet - subCurBullet);
        subCurBullet += _subWeaponInfo.MagazineBullet;
        subCurBullet = Mathf.Min(subCurBullet, _subWeaponInfo.MagazineBullet);
    }

    public void OnClickMainWeapon1()
    {
        if (main1CurBullet >= _mainWeaponInfo.MagazineBullet)
            return;

        Player.local.gold -= _mainWeaponInfo.BulletPrice * (_mainWeaponInfo.MagazineBullet - main1CurBullet);
        main1CurBullet += _mainWeaponInfo.MagazineBullet;
        main1CurBullet = Mathf.Min(main1CurBullet, _mainWeaponInfo.MagazineBullet);        
    }

    public void OnClickMainWeapon2()
    {
        if (main2CurBullet >= _mainWeaponInfo2.MagazineBullet)
            return;

        Player.local.gold -= _mainWeaponInfo2.BulletPrice * (_mainWeaponInfo2.MagazineBullet - main2CurBullet);
        main2CurBullet += _mainWeaponInfo2.MagazineBullet;
        main2CurBullet = Mathf.Min(main2CurBullet, _mainWeaponInfo2.MagazineBullet);
    }

    public void OnClickItemBuy()
    {
        if (consumeItem >= _itemInfo.MaxNum) return;

        Player.local.gold -= _itemInfo.Price;
        consumeItem++;
        consumeItem = Mathf.Min(consumeItem, _itemInfo.MaxNum);
    }

    public void OnClickBuyBtn(int index)
    {
        _store.RPC_RequestTryBuy(Player.local, Player.local.Runner.LocalPlayer, index);
        UpdateButtonState();
    }

    public void OnClickSaleBtn(int index)
    {
        _store.RPC_RequestTrySale(Player.local, Player.local.Runner.LocalPlayer, index);
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
