using Fusion;
using INFEST.Game;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using UnityEngine.UI;

public class UIStateView : UIScreen
{
    [Header("State")]
    public TextMeshProUGUI hpText;
    public TextMeshProUGUI defText;
    public TextMeshProUGUI goldText;
    public TextMeshProUGUI bulletText;
    public TextMeshProUGUI[] itemText;

    [Header("Job Icon")]
    public Image comender;
    public Image medic;
    public Image demol;
    public Image warrior;

    [Header("Weapon Icon")]
    public Image pistol;
    public Image rifle;
    public Image shotgun;
    public Image sniper;
    public Image machinegun;
    public Image launcher;

    private WeaponInfo _weaponInfo;
    private CharacterInfo _characterInfo;

    public override void Awake()
    {
        base.Awake();

        localPlayer = NetworkGameManager.Instance.gamePlayers.GetPlayerObj(NetworkGameManager.Instance.Runner.LocalPlayer);
        SetJobIcon();
        SetWeaponIcon();

        _characterInfo = localPlayer.statHandler.info.data; // 각각의 플레이어 정보를 넣어주지 않으면 불가능.
        _weaponInfo = localPlayer.inventory.equippedWeapon.instance.data;
        ChoiceJob();

        UpdateJobIcon();
        UpdateWeaponIcon();
        OnShow();
    }

    private void Update()
    {
        if(localPlayer != null)
            UpdatePlayerState();
        if (localPlayer.inventory != null)
            UpdateWeaponIcon();

    }

    public override void OnShow()
    {
        base.OnShow();
    }

    public override void OnHide()
    {
        base.OnHide();
    }

    public void SetWeaponIcon()
    {
        pistol.gameObject.SetActive(false);
        rifle.gameObject.SetActive(false);
        shotgun.gameObject.SetActive(false);
        sniper.gameObject.SetActive(false);
        machinegun.gameObject.SetActive(false);
        launcher.gameObject.SetActive(false);
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
        if (PlayerPrefsManager.GetJob() == JOB.Commander)
        {
            _characterInfo = DataManager.Instance.GetByKey<CharacterInfo>(1);
        }
        else if (PlayerPrefsManager.GetJob() == JOB.BattleMedic)
        {
            _characterInfo = DataManager.Instance.GetByKey<CharacterInfo>(2);
        }
        else if (PlayerPrefsManager.GetJob() == JOB.Demolator)
        {
            _characterInfo = DataManager.Instance.GetByKey<CharacterInfo>(3);
        }
        else if (PlayerPrefsManager.GetJob() == JOB.Defender)
        {
            _characterInfo = DataManager.Instance.GetByKey<CharacterInfo>(4);
        }
    }

    Player localPlayer;
    public void UpdatePlayerState()
    {
        if (localPlayer.statHandler.info == null) return;

        PlayerStatHandler _info = localPlayer.statHandler;

        hpText.text = _info.CurHealth.ToString();
        defText.text = _info.CurDefGear.ToString();
        goldText.text = _info.CurGold.ToString();

        if (localPlayer.inventory.equippedWeapon == null)
        {
            bulletText.text = " - / - ";
            return;
        }

        if (localPlayer.inventory.equippedWeapon.key % 10000 < 700)
        {
            if(localPlayer.inventory.equippedWeapon.key == localPlayer.inventory.auxiliaryWeapon[0]?.key)
                bulletText.text = $"{localPlayer.inventory.auxiliaryWeapon[0].curMagazineBullet}/{localPlayer.inventory.auxiliaryWeapon[0].curBullet}";
            else if(localPlayer.inventory.equippedWeapon.key == localPlayer.inventory.weapon[0]?.key)
                bulletText.text = $"{localPlayer.inventory.weapon[0].curMagazineBullet}/{localPlayer.inventory.weapon[0].curBullet}";
            else if(localPlayer.inventory.equippedWeapon.key == localPlayer.inventory.weapon[1]?.key)
                bulletText.text = $"{localPlayer.inventory.weapon[1].curMagazineBullet}/{localPlayer.inventory.weapon[1].curBullet}";
        }

        for (int i = 0; i < itemText.Length; i++)
        {
            if (localPlayer.inventory.consume[i] == null)
            {
                itemText[i].text = "-";
            }
            else
            {
                itemText[i].text = $"{localPlayer.inventory.consume[i].curNum} / {localPlayer.inventory.consume[i].instance.data.MaxNum}";
            }
        }

        //else
        //{
        //    for (int i = 0; i < 3; i++)
        //    {
        //        if (localPlayer.inventory.equippedWeapon.key == localPlayer.inventory.consume[i]?.key)
        //            bulletText.text = $"1/{localPlayer.inventory.consume[i].curNum}";
        //    }
        //}


        //if (_characterInfo == null) return;

        //hpText.text = _characterInfo.Health.ToString();
        //defText.text = _characterInfo.Def.ToString();
        //goldText.text = _characterInfo.StartGold.ToString();
        //bulletText.text = $"{_weaponInfo.MagazineBullet}/{_weaponInfo.MaxBullet}";        
    }

    
    private void UpdateJobIcon()
    {
        //캐릭터인포 키값이 1이면 커맨더, 2면 전투의무병 , 3이면 데몰레이터, 4면 워리어
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

    //스왑할때 넣어줘야함
    public void UpdateWeaponIcon()
    {
        SetWeaponIcon();
        _weaponInfo = localPlayer.inventory.equippedWeapon.instance.data;
        int weaponType = (int)_weaponInfo.WeaponType;

        switch (weaponType)
        {
            case 0:
                pistol.gameObject.SetActive(true);
                break;
            case 1:
                rifle.gameObject.SetActive(true);
                break;
            case 2:
                shotgun.gameObject.SetActive(true);
                break;
            case 3:
                sniper.gameObject.SetActive(true);
                break;
            case 4:
                machinegun.gameObject.SetActive(true);
                break;
            case 5:
                launcher.gameObject.SetActive(true);
                break;
        }
    }
}
