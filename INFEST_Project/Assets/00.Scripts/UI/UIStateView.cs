using TMPro;
using UnityEngine.UI;

public class UIStateView : UIScreen
{
    public TextMeshProUGUI hpText;
    public TextMeshProUGUI defText;
    public TextMeshProUGUI goldText;
    public TextMeshProUGUI bulletText;
    public Image[] jobIcon;
    public Image[] weaponIcon;

    private CharacterInfo _characterInfo;
    private WeaponInfo _weaponInfo;

    public override void Awake()
    {
        base.Awake();

        _characterInfo = new CharacterInfo();
        _weaponInfo = new WeaponInfo();

        SetState();
    }

    public override void Show()
    {
        base.Show();
    }

    public override void Hide()
    {
        base.Hide();
    }

    //임시 함수
    public void SetState()
    {
        _characterInfo.Health = 100;
        _characterInfo.Def = 50;
        _characterInfo.StartGold = 500;
        _weaponInfo.MaxBullet = 120;
        _weaponInfo.MagazineBullet = 40;

        hpText.text = _characterInfo.Health.ToString();
        defText.text = _characterInfo.Def.ToString();
        goldText.text = _characterInfo.StartGold.ToString("N0");
        bulletText.text = _weaponInfo.MagazineBullet.ToString() + "/" + _weaponInfo.MaxBullet.ToString();        
    }

    public void UpdatePlayerState(Player player, CharacterInfo playerData, WeaponInfo weaponInfo)
    {
        hpText.text = playerData.Health.ToString();
        defText.text = playerData.Def.ToString();
        goldText.text = playerData.StartGold.ToString("N0");
        bulletText.text = $"{weaponInfo.MagazineBullet}/{weaponInfo.MaxBullet}";
    }

    public void UpdateJobIcon()
    {
        //직업에 따라 아이콘 변경
    }

    public void UpdateWeaponIcon()
    {
        //무기에 따라 아이콘 변경
    }
}
