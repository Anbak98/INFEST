using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIShopView : UIScreen
{
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

    WeaponInfo _weaponInfo;
    CharacterInfo _characterInfo;


    public override void Awake()
    {
        base.Awake();

        _characterInfo = DataManager.Instance.GetByKey<CharacterInfo>(1);

        DefSet();
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
}
