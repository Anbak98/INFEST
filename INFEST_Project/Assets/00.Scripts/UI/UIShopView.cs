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


    public override void Awake()
    {
        base.Awake();
    }

    public override void Show()
    {
        base.Show();
    }

    public override void Hide()
    {
        base.Hide();
    }
}
