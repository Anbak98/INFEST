using System;
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
    public Image profile;
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
    public Image[] weaponIcon;
    public TextMeshProUGUI[] weaponText; // 현재 총알 텍스트
    //public Image weaponIcon1;
    //public TextMeshProUGUI weaponText1;
    //public Image weaponIcon2;
    //public TextMeshProUGUI weaponText2;
    public Image[] itemWeaponIcon;
    public TextMeshProUGUI[] itemWeaponText;

    [Header("Gold")]
    public TextMeshProUGUI goldText;

    [Header("BuyItme")]
    public TextMeshProUGUI[] weaponNames;
    public TextMeshProUGUI[] ItmePrice;
    public Image[] itmeNameBG;
    public GameObject[] buyTaps;

    [Header("Button")]
    public List<Button> buyButton;                      // 구매 버튼
    public List<TextMeshProUGUI> buyButtonText;         // 구매 버튼 텍스트
    public List<Button> saleButton;                     // 판매 버튼
    public List<TextMeshProUGUI> saleButtonText;        // 판매 버튼 텍스트

    //WeaponInfo _subWeaponInfo;
    //WeaponInfo _mainWeaponInfo;
    //WeaponInfo _mainWeaponInfo2;
    //ConsumeItem _itemInfo;
    CharacterInfo _characterInfo;

    //[Header("CurBullet")]
    //public int subCurBullet;
    //public int main1CurBullet;
    //public int main2CurBullet;
    //public int consumeItem;

    [Header("BulletPrice")] // 구매 버튼 텍스트
    public TextMeshProUGUI[] weaponBullet; // 총알 가격

    //public TextMeshProUGUI mainWeapon1Bullet;
    //public TextMeshProUGUI mainWeapon2Bullet;
    public TextMeshProUGUI[] itemPrice; // 아이템 가격

    [Header("WeaponName")]
    public TextMeshProUGUI[] weaponName; // 무기 이름
    //public TextMeshProUGUI mainWeapon1Name;
    //public TextMeshProUGUI mainWeapon2Name;
    public TextMeshProUGUI[] itemName; // 아이템 이름

    [Networked]
    public Profile Info { get; set; }


    public override void Awake()
    {
        //_characterInfo = DataManager.Instance.GetByKey<CharacterInfo>(1);
        //_subWeaponInfo = DataManager.Instance.GetByKey<WeaponInfo>(10102);
        //_mainWeaponInfo = DataManager.Instance.GetByKey<WeaponInfo>(10201);
        //_mainWeaponInfo2 = DataManager.Instance.GetByKey<WeaponInfo>(10303);
        //_itemInfo = DataManager.Instance.GetByKey<ConsumeItem>(10701);

        //subCurBullet = _subWeaponInfo.MagazineBullet;
        //main1CurBullet = _mainWeaponInfo.MagazineBullet;
        //main2CurBullet = _mainWeaponInfo2.MagazineBullet;
        //consumeItem = _itemInfo.MaxNum;

        //ChoiceJob();
        //SetJobIcon();
        //DefSet();
        //UpdateJobIcon();
    }

    protected override void Start()
    {
        base.Start();
        bg.gameObject.SetActive(false);
        interactionText.gameObject.SetActive(false);
        profile.gameObject.SetActive(false);
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

    public void DefSet()
    {
        defText.text = $"{Player.local.characterInfoInstance.curDefGear}/200";
    }

    private void GoldSet()
    {
        goldText.text = $"{Player.local.characterInfoInstance.curGold}";
    }

    public void WeaponSet(int index)
    {
        Weapon[] weaponInv = { Player.local.inventory.auxiliaryWeapon[0], Player.local.inventory.weapon[0], Player.local.inventory.weapon[1] };
        if (weaponInv[index] == null)
        {
            weaponText[index].text = $"미보유";
            weaponBullet[index].text = $"- G";
            weaponName[index].text = $"미보유";
            return;
        }

        int differenceBullet = 0;
        if (weaponInv[index].curBullet + weaponInv[index].instance.data.MagazineBullet >= weaponInv[index].instance.data.MaxBullet)
            differenceBullet = weaponInv[index].curBullet % weaponInv[index].instance.data.MagazineBullet;

        weaponText[index].text = $"{weaponInv[index].curBullet}/{weaponInv[index].instance.data.MaxBullet}";
        weaponBullet[index].text = $"{(weaponInv[index].instance.data.MagazineBullet - differenceBullet) * weaponInv[index].instance.data.BulletPrice}G";
        weaponName[index].text = $"{weaponInv[index].instance.data.Name}";

        if (weaponInv[index].instance.data.MaxBullet <= weaponInv[index].curBullet)
        {
            weaponBullet[index].text = $"- G";
        }

    }

    public void ItemSet(int index)
    {
        ConsumeInstance[] ItemInv = { Player.local.inventory.consume[0], Player.local.inventory.consume[1], Player.local.inventory.consume[2] };

        if (ItemInv[index] == null)
        {
            itemWeaponText[index].text = $"미보유";
            itemPrice[index].text = $"- G";
            itemName[index].text = $"미보유";
            return;
        }

        itemWeaponText[index].text = $"{ItemInv[index].curNum}/{ItemInv[index].data.MaxNum}";
        itemPrice[index].text = $"{ItemInv[index].data.Price}G";
        itemName[index].text = $"{ItemInv[index].data.Name}";

        if (ItemInv[index].data.MaxNum <= ItemInv[index].curNum)
        {
            itemPrice[index].text = $"- G";
        }
    }
    //private void MainWeapon1Set()
    //{
    //      .text = $"{main1CurBullet}/{_mainWeaponInfo.MaxBullet}";
    //    mainWeapon1Bullet.text = $"탄창\n{(_mainWeaponInfo.MagazineBullet - main1CurBullet) * _mainWeaponInfo.BulletPrice}G";
    //    mainWeapon1Name.text = $"{_mainWeaponInfo.Name}";
    //}
    //private void MainWeapon2Set()
    //{
    //    weaponText2.text = $"{main2CurBullet}/{_mainWeaponInfo2.MaxBullet}";
    //    mainWeapon2Bullet.text = $"탄창\n{(_mainWeaponInfo2.MagazineBullet - main2CurBullet) * _mainWeaponInfo2.BulletPrice}G";
    //    mainWeapon2Name.text = $"{_mainWeaponInfo2.Name}";
    //}


    private void AllSupplementSet()
    {
        var inv = Player.local.inventory;
        int subPrice = 0;
        int main1Price = 0;
        int main2Price = 0;
        int item1Price = 0;
        int item2Price = 0;
        int item3Price = 0;

        if (inv.auxiliaryWeapon[0] != null)
            subPrice = (inv.auxiliaryWeapon[0].instance.data.MaxBullet - inv.auxiliaryWeapon[0].curBullet) * inv.auxiliaryWeapon[0].instance.data.BulletPrice;
        if (inv.weapon[0] != null)
            main1Price = (inv.weapon[0].instance.data.MaxBullet - inv.weapon[0].curBullet) * inv.weapon[0].instance.data.BulletPrice;
        if (inv.weapon[1] != null)
            main2Price = (inv.weapon[1].instance.data.MaxBullet - inv.weapon[1].curBullet) * inv.weapon[1].instance.data.BulletPrice;
        if (inv.consume[0] != null)
            item1Price = (inv.consume[0].data.MaxNum - inv.consume[0].curNum) * inv.consume[0].data.Price;
        if (inv.consume[1] != null)
            item2Price = (inv.consume[1].data.MaxNum - inv.consume[1].curNum) * inv.consume[1].data.Price;
        if (inv.consume[2] != null)
            item3Price = (inv.consume[2].data.MaxNum - inv.consume[2].curNum) * inv.consume[2].data.Price;

        if (Player.local.characterInfoInstance.curDefGear >= 200)
        {
            allSupplement.text = $"모두 보충\n({subPrice + main1Price + main2Price + item1Price + item2Price + item3Price}G)";
        }
        else
        {
            allSupplement.text = $"모두 보충\n({500 + subPrice + main1Price + main2Price + item1Price + item2Price + item3Price}G)";
        }
    }

    //public void OnClickAllBtn()
    //{
    //    int subPrice = (_subWeaponInfo.MagazineBullet - subCurBullet) * _subWeaponInfo.BulletPrice;
    //    int main1Price = (_mainWeaponInfo.MagazineBullet - main1CurBullet) * _mainWeaponInfo.BulletPrice;
    //    int main2Price = (_mainWeaponInfo2.MagazineBullet - main2CurBullet) * _mainWeaponInfo2.BulletPrice;
    //    int itemPrice = (_itemInfo.MaxNum - consumeItem) * _itemInfo.Price;

    //    if (_characterInfo.DefGear >= 200)
    //    {
    //        Player.local.characterInfoInstance.curGold -= subPrice + main1Price + main2Price + itemPrice;
    //    }
    //    else
    //    {
    //        Player.local.characterInfoInstance.curGold -= 500 + subPrice + main1Price + main2Price + itemPrice;
    //    }

    //    _characterInfo.DefGear += 200;
    //    _characterInfo.DefGear = Mathf.Min(_characterInfo.DefGear, 200);

    //    subCurBullet += _subWeaponInfo.MagazineBullet;
    //    subCurBullet = Mathf.Min(subCurBullet, _subWeaponInfo.MagazineBullet);

    //    main1CurBullet += _mainWeaponInfo.MagazineBullet;
    //    main1CurBullet = Mathf.Min(main1CurBullet, _mainWeaponInfo.MagazineBullet);

    //    main2CurBullet += _mainWeaponInfo2.MagazineBullet;
    //    main2CurBullet = Mathf.Min(main2CurBullet, _mainWeaponInfo2.MagazineBullet);

    //    consumeItem += _itemInfo.MaxNum;
    //    consumeItem = Mathf.Min(consumeItem, _itemInfo.MaxNum);
    //}



    public void BuyItemSet(Store stpre)
    {
        for (int i = 0; i < weaponNames.Length; i++)
        {
            if (i < 17)
            {
                weaponNames[i].text = DataManager.Instance.GetByKey<WeaponInfo>(stpre.idList[i]).Name;
                ItmePrice[i].text = $"{DataManager.Instance.GetByKey<WeaponInfo>(stpre.idList[i]).Price}G";
            }
            else
            {
                weaponNames[i].text = DataManager.Instance.GetByKey<ConsumeItem>(stpre.idList[i]).Name;
                ItmePrice[i].text = $"{DataManager.Instance.GetByKey<ConsumeItem>(stpre.idList[i]).Price}G";
            }
        }
    }

    //public void OnClickMainWeapon1()
    //{
    //    if (main1CurBullet >= _mainWeaponInfo.MagazineBullet)
    //        return;

    //    Player.local.characterInfoInstance.curGold -= _mainWeaponInfo.BulletPrice * (_mainWeaponInfo.MagazineBullet - main1CurBullet);
    //    main1CurBullet += _mainWeaponInfo.MagazineBullet;
    //    main1CurBullet = Mathf.Min(main1CurBullet, _mainWeaponInfo.MagazineBullet);        
    //}

    //public void OnClickMainWeapon2()
    //{
    //    if (main2CurBullet >= _mainWeaponInfo2.MagazineBullet)
    //        return;

    //    Player.local.characterInfoInstance.curGold -= _mainWeaponInfo2.BulletPrice * (_mainWeaponInfo2.MagazineBullet - main2CurBullet);
    //    main2CurBullet += _mainWeaponInfo2.MagazineBullet;
    //    main2CurBullet = Mathf.Min(main2CurBullet, _mainWeaponInfo2.MagazineBullet);
    //}

    //public void OnClickItemBuy()
    //{
    //    if (consumeItem >= _itemInfo.MaxNum) return;

    //    Player.local.characterInfoInstance.curGold -= _itemInfo.Price;
    //    consumeItem++;
    //    consumeItem = Mathf.Min(consumeItem, _itemInfo.MaxNum);
    //}
    public void OnClickAllBtn()
    {
        _store.RPC_RequestTryAllSupplement(Player.local, Player.local.Runner.LocalPlayer);
    }

    public void OnClickDefBtn()
    {

        _store.RPC_RequestTryDefSupplement(Player.local, Player.local.Runner.LocalPlayer);
    }

    public void OnClickBulletSupplementBtn(int index)
    {
        _store.RPC_RequestTryBulletSupplement(Player.local, Player.local.Runner.LocalPlayer, index);
    }

    public void OnClickItmeSupplementBtn(int index)
    {
        _store.RPC_RequestTryItmeSupplement(Player.local, Player.local.Runner.LocalPlayer, index);
    }

    public void OnClickBuyBtn(int index)
    {
        _store.RPC_RequestTryBuy(Player.local, Player.local.Runner.LocalPlayer, index);
    }

    public void OnClickSaleBtn(int index)
    {
        _store.RPC_RequestTrySale(Player.local, Player.local.Runner.LocalPlayer, index);

    }

    public void UpdateSaleButtonState()
    {
        if (HasOnlyOneNonNullInAllArrays())
        {
            for (int i = 0; i < saleButton.Count; i++)
            {
                saleButton[i].interactable = false;
            }
        }
        else
        {
            for (int i = 0; i < saleButton.Count; i++)
            {
                saleButton[i].interactable = true;
            }
        }
    }

    public bool HasOnlyOneNonNullInAllArrays()
    {
        int count = 0;

        foreach (var weapon in Player.local.inventory.weapon)
        {
            if (weapon != null) count++;
        }

        foreach (var weapon in Player.local.inventory.auxiliaryWeapon)
        {
            if (weapon != null) count++;
        }

        //foreach (var itme in Player.local.inventory.consume)
        //{
        //    if (itme != null) count++;
        //}

        return count == 1;
    }
    public void StoreInIt(Store store)
    {
        _store = store;
        BuyItemSet(store);
    }
    /// <summary>
    /// 구매버튼 업데이트
    /// </summary>
    public void UpdateButtonState()
    {
        var _inv = Player.local.inventory;
        for (int i = 0; i < buyButton.Count; i++)
        {

            if (Player.local == null || Player.local.inventory == null || _store == null || i >= _store.idList.Count)
                return;

            int weaponPrice = DataManager.Instance.GetByKey<WeaponInfo>(_store.idList[i])?.Price ?? 0;
            int consumePrice = DataManager.Instance.GetByKey<ConsumeItem>(_store.idList[i])?.Price ?? 0;

            int _itemKey = _store.idList[i] % 10000;
            #region 체크용 bool 값
            bool auxiliaryWeaponChk = _itemKey < 200;
            bool weaponChk = _itemKey < 700 && _itemKey > 200;
            bool throwingWeapon = _itemKey < 800 && _itemKey > 700;
            bool recoveryItem = _itemKey < 900 && _itemKey > 800;
            bool shieldItme = _itemKey < 1000 && _itemKey > 900;
            #endregion
            bool weaponDuplication = _inv.weapon[0]?.instance.data.key == _store.idList[i] || _inv.weapon[1]?.instance.data.key == _store.idList[i];
            Color32 color = Color.white;

            if (auxiliaryWeaponChk)
            {
                color = new Color32(209, 219, 189, 255);
            }
            else if (weaponChk)
            {
                color = new Color32(145, 170, 157, 255);
            }
            else if (throwingWeapon)
            {
                color = new Color32(103, 151, 157, 255);
            }
            else if (recoveryItem || shieldItme)
            {
                color = new Color32(103, 129, 156, 255);
            }
            itmeNameBG[i].color = color;


            if ((auxiliaryWeaponChk && _inv.auxiliaryWeapon[0] != null) || (Player.local.characterInfoInstance.curGold < weaponPrice))
                buyButton[i].interactable = false;

            else if ((weaponChk && _inv.weapon[0] != null && _inv.weapon[1] != null) || Player.local.characterInfoInstance.curGold < weaponPrice || weaponDuplication)
                buyButton[i].interactable = false;

            else if ((throwingWeapon && _inv.consume[0] != null) || Player.local.characterInfoInstance.curGold < consumePrice)
            {
                if (_inv.consume[0]?.data.key != _store.idList[i] || _inv.consume[0]?.curNum == _inv.consume[0]?.data.MaxNum)
                    buyButton[i].interactable = false;
            }

            else if ((recoveryItem && _inv.consume[1] != null) || Player.local.characterInfoInstance.curGold < consumePrice)
            {
                if (_inv.consume[1]?.data.key != _store.idList[i] || _inv.consume[1]?.curNum == _inv.consume[1]?.data.MaxNum)
                    buyButton[i].interactable = false;
            }

            else if ((shieldItme && _inv.consume[2] != null) || Player.local.characterInfoInstance.curGold < consumePrice)
            {
                if (_inv.consume[2]?.data.key != _store.idList[i] || _inv.consume[2]?.curNum == _inv.consume[2]?.data.MaxNum)
                    buyButton[i].interactable = false;
            }

            else
            {
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


        }
        GoldSet();
        DefSet();
        AllSupplementSet();
        ResetText();
    }
    public void SaleSet(int index)
    {
        Weapon[] weaponInv = { Player.local.inventory.auxiliaryWeapon[0], Player.local.inventory.weapon[0], Player.local.inventory.weapon[1] };
        ConsumeInstance[] ItemInv = { Player.local.inventory.consume[0], Player.local.inventory.consume[1], Player.local.inventory.consume[2] };


        if (index < 3)
        {
            if (weaponInv[index] != null)
                saleButtonText[index].text = $"{weaponInv[index].instance.data.Price / 2}G";
            else
                saleButtonText[index].text = $"- G";
        }
        else if (index > 2)
        {
            if (ItemInv[index - 3] != null)
                saleButtonText[index].text = $"{ItemInv[index - 3].data.Price / 2}G";
            else
                saleButtonText[index].text = $"- G";
        }
    }

    public void ResetText()
    {
        for (int i = 0; i < 3; i++)
        {
            WeaponSet(i);
            ItemSet(i);
            SaleSet(i);
            SaleSet(i + 3);
        }
    }

    public void OnClickTypeBtn(int index)
    {
        int _itemKey;

        for (int i = 0; i < buyTaps.Length; i++)
        {
            buyTaps[i].gameObject.SetActive(false);
        }

        switch (index)
        {

            case 0:
                for (int i = 0; i < buyTaps.Length; i++)
                {
                    buyTaps[i].gameObject.SetActive(true);
                }
                break;
            case 1:
                for (int i = 0; i < buyTaps.Length; i++)
                {
                    _itemKey = _store.idList[i] % 10000;

                    if (_itemKey < 200)
                        buyTaps[i].gameObject.SetActive(true);
                }
                break;
            case 2:
                for (int i = 0; i < buyTaps.Length; i++)
                {
                    _itemKey = _store.idList[i] % 10000;

                    if (_itemKey > 200 && _itemKey < 300)
                        buyTaps[i].gameObject.SetActive(true);
                }
                break;
            case 3:
                for (int i = 0; i < buyTaps.Length; i++)
                {
                    _itemKey = _store.idList[i] % 10000;

                    if (_itemKey > 300 && _itemKey < 400)
                        buyTaps[i].gameObject.SetActive(true);
                }
                break;
            case 4:
                for (int i = 0; i < buyTaps.Length; i++)
                {
                    _itemKey = _store.idList[i] % 10000;

                    if (_itemKey > 400 && _itemKey < 500)
                        buyTaps[i].gameObject.SetActive(true);
                }
                break;
            case 5:
                for (int i = 0; i < buyTaps.Length; i++)
                {
                    _itemKey = _store.idList[i] % 10000;

                    if (_itemKey > 500 && _itemKey < 700)
                        buyTaps[i].gameObject.SetActive(true);
                }
                break;
            case 6:
                for (int i = 0; i < buyTaps.Length; i++)
                {
                    _itemKey = _store.idList[i] % 10000;

                    if (_itemKey > 700 && _itemKey < 1000)
                        buyTaps[i].gameObject.SetActive(true);
                }
                break;
        }
    }
}
