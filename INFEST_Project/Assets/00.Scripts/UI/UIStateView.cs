using TMPro;
using UnityEngine.UI;

public class UIStateView : UIScreen
{
    public TextMeshProUGUI hpText;
    public TextMeshProUGUI defText;
    public TextMeshProUGUI goldText;
    public TextMeshProUGUI bulletText;
    public Image[] jobIcon;
    public Image weaponIcon;

    private CharacterInfo _player;
    private WeaponInfo _weaponInfo;
    private Weapon _weapon;

    public override void Awake()
    {
        base.Awake();

        _player = DataManager.Instance.GetByKey<CharacterInfo>(1);
        _weaponInfo = DataManager.Instance.GetByKey<WeaponInfo>(10101);

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

    public void UpdatePlayerState()
    {
        hpText.text = _player.Health.ToString();
        defText.text = _player.Def.ToString();
        goldText.text = _player.StartGold.ToString();
        bulletText.text = $"{_weaponInfo.MagazineBullet}/{_weaponInfo.MaxBullet}";
    }

    public void UpdateJobIcon()
    {
        //직업에 따라 아이콘 변경
    }

    public void UpdateWeapon(Weapons weapons)
    {
        SetWeapon(weapons.CurrentWeapon);

        if (_weapon == null) return;

        bulletText.text = $"{_weapon.curClip}/{_weapon.possessionAmmo}";
    }

    private void SetWeapon(Weapon weapon)
    {
        if (weapon == _weapon) return;

        _weapon = weapon;

        if(weapon == null) return;

        weaponIcon.sprite = weapon.icon;
    }
}
