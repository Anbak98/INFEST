using Cinemachine;
using Fusion;
using UnityEngine;

public class Weapons : NetworkBehaviour
{
    public CinemachineVirtualCamera camera;
    public Transform fireTransform;
    public float weaponSwitchTime = 1f;

    public bool IsSwitching => _switchTimer.ExpiredOrNotRunning(Runner) == false;

    public Weapon CurrentWeapon;

    public Weapon[] AllWeapons;

    private TickTimer _switchTimer { get; set; }
    private int _weaponIdx = -1;

    public override void Spawned()
    {
        if (HasStateAuthority)
        {
            CurrentWeapon = AllWeapons[0];
            AllWeapons[1].IsCollected = true;
            CurrentWeapon.IsCollected = true;
            AllWeapons[1].GetComponentInChildren<Transform>().gameObject.SetActive(false);
        }
    }



    /// <summary>
    /// 발사
    /// </summary>
    public void Fire(bool holdingPressed)
    {
        if (CurrentWeapon == null || IsSwitching)
            return;

        CurrentWeapon.Fire();
    }

    /// <summary>
    /// 재장전
    /// </summary>
    public void Reload()
    {
        if (CurrentWeapon == null || IsSwitching)
            return;

        CurrentWeapon.Reload();
    }

    /// <summary>
    /// 조준
    /// </summary>
    public void Aiming()
    {
        if (CurrentWeapon == null || IsSwitching)
            return;

        CurrentWeapon.Aiming(camera);
    }

    public void StopAiming()
    {
        if (CurrentWeapon == null || IsSwitching)
            return;
        if(CurrentWeapon.IsAiming)
            CurrentWeapon.StopAiming();
    }

    /// <summary>
    /// 무기 교체
    /// </summary>
    public void Swap(float scrollWheelValue)
    {
        //for(int i = 0; i < AllWeapons.Length; i++)
        //{
        //    if (AllWeapons[i] == CurrentWeapon)
        //    {
        //        CurrentWeapon.GetComponentInChildren<Transform>().gameObject.SetActive(false);
        //        _weaponIdx = i;
        //        break;
        //    }
        //}

        //if (scrollWheelValue > 0) // 스크롤 업
        //{
        //    if (CurrentWeapon == AllWeapons[AllWeapons.Length - 1])
        //        CurrentWeapon = AllWeapons[0];
        //    else
        //        CurrentWeapon = AllWeapons[_weaponIdx + 1];

        //    CurrentWeapon.GetComponentInChildren<Transform>().gameObject.SetActive(true);
        //}
        //else if(scrollWheelValue < 0) // 스크롤 다운
        //{
        //    if (CurrentWeapon == AllWeapons[0])
        //        CurrentWeapon = AllWeapons[AllWeapons.Length - 1];
        //    else
        //        CurrentWeapon = AllWeapons[_weaponIdx - 1];

        //    CurrentWeapon.GetComponentInChildren<Transform>().gameObject.SetActive(true);
        //}
        //else
        //{
        //    CurrentWeapon.GetComponentInChildren<Transform>().gameObject.SetActive(true);

        //    Debug.Log("스크롤버튼 클릭");
        //}

    }
}
