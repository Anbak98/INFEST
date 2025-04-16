using TMPro;
using UnityEngine.UI;

public class UIStateView : UIScreen
{
    public TextMeshProUGUI hpText;
    public TextMeshProUGUI defText;
    public TextMeshProUGUI goldText;
    public TextMeshProUGUI bulletText;
    public Image jobIcon;
    public Image weaponIcon;

    private CharacterInfo _characterInfo;
    private WeaponInfo _weaponInfo;
    private Weapon _weapon;

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
        bulletText.text = weaponInfo.MagazineBullet.ToString() + "/" + weaponInfo.MaxBullet.ToString();
    }

    public void UpdateWeapon(Weapons weapons)
    {
        SetWeapon(weapons.CurrentWeapon);

        for (int i = 0; i < weapons.AllWeapons.Length; i++)
        {
            var weapon = weapons.AllWeapons[i];            
        }

        if (_weapon == null)
            return;
    }

    private void SetWeapon(Weapon weapon)
    {
        
    }
}
