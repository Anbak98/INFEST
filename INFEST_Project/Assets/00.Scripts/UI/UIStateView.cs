using Fusion;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIStateView : UIScreen
{
    [Header("State")]
    public TextMeshProUGUI hpText;
    public TextMeshProUGUI defText;
    public TextMeshProUGUI goldText;
    public TextMeshProUGUI bulletText;

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

    [Networked]
    public Profile Info { get; set; }

    public override void Awake()
    {
        base.Awake();

        SetJobIcon();
        SetWeaponIcon();

        _weaponInfo = DataManager.Instance.GetByKey<WeaponInfo>(10201);
        ChoiceJob();

        UpdateJobIcon();
        UpdateWeaponIcon();
    }

    private void Update()
    {
        UpdatePlayerState();
    }

    public override void Show()
    {
        base.Show();
    }

    public override void Hide()
    {
        base.Hide();
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

    public void UpdatePlayerState()
    {
        if (_characterInfo == null) return;

        hpText.text = _characterInfo.Health.ToString();
        defText.text = _characterInfo.Def.ToString();
        goldText.text = _characterInfo.StartGold.ToString();
        bulletText.text = $"{_weaponInfo.MagazineBullet}/{_weaponInfo.MaxBullet}";        
    }

    
    private void UpdateJobIcon()
    {
        //ĳ�������� Ű���� 1�̸� Ŀ�Ǵ�, 2�� �����ǹ��� , 3�̸� ����������, 4�� ������
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

    //�����Ҷ� �־������
    public void UpdateWeaponIcon()
    {
        SetWeaponIcon();
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
