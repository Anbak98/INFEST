using Fusion;
using UnityEngine;
using UnityEngine.UI;

public class WeaponInstance 
{
    public readonly WeaponInfo data;

    public WeaponInstance(int key)
    {
        data = DataManager.Instance.GetByKey<WeaponInfo>(key);  
        curMagazineBullet = data.MagazineBullet;
        curBullet = data.MaxBullet;
        RecoilForce = data.RecoilForce;
        concentration = data.Concentration;
    }

    public int curMagazineBullet { get; private set; }                          // 현재 탄창의 총알
    public int curBullet { get; private set; }                                  // 현재 보유중인 총알
    public float RecoilForce { get; private set; }                              // 반동 (조준시 변경)
    public float concentration { get; private set; }                            // 집탄율 (조준시 변경)
    public Image icon { get; set; }                                             // UI에 사용될 아이콘
    public float fireRate { get; private set; } = 2;                            // 공격 속도
    public bool isAutomatic { get; private set; } = true;                       // 사격 방식 (단발 or 자동)
    public int projectilesPerShot { get; private set; } = 1;                    // 사격당 나가는 탄환 갯수
    public EWeaponType weaponType { get; private set; } = EWeaponType.Rifle;    // 무기 종류 (라이플, 샷권 등)

    public void ReloadShotgun(int _curBullet, int _curMagazineBullet)
    {
        curBullet = _curBullet;
        curMagazineBullet = _curMagazineBullet;
    }

    public void Reload(int _curBullet, int _curMagazineBullet)
    {
        curBullet = _curBullet;
        curMagazineBullet = _curMagazineBullet;
    }

    public void Fire(int _curBullet)
    {
        curMagazineBullet = _curBullet;
    }

    public void IsAiming()
    {
        concentration += 0.3f;
    }

    public void StopAiming()
    {
        concentration -= 0.3f;
    }

    public void SupplementBullet()
    {
        curBullet += data.MagazineBullet;
        curBullet = Mathf.Min(curBullet, data.MaxBullet);
    }
}
