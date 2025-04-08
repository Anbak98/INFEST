using Fusion;
using UnityEngine;

public class Weapons : NetworkBehaviour
{
    public Transform fireTransform;
    public float weaponSwitchTime = 1f;

    public bool IsSwitching => _switchTimer.ExpiredOrNotRunning(Runner) == false;

    public Weapon CurrentWeapon { get; set; }

    [HideInInspector] public Weapon[] AllWeapons;

    private TickTimer _switchTimer { get; set; }

    private void Awake()
    {
        AllWeapons = GetComponentsInChildren<Weapon>();
    }

    public override void FixedUpdateNetwork()
    {

    }

    public override void Spawned()
    {
        if (HasStateAuthority)
        {
            CurrentWeapon = AllWeapons[0];
            CurrentWeapon.IsCollected = true;
        }
    }

    /// <summary>
    /// 발사
    /// </summary>
    public void Fire(bool holdingPressed)
    {
        if (CurrentWeapon == null || IsSwitching)
            return;

        CurrentWeapon.Fire(fireTransform.position, fireTransform.forward, holdingPressed);
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
    /// 무기 교체
    /// </summary>
    public void Swap()
    {

    }
}
