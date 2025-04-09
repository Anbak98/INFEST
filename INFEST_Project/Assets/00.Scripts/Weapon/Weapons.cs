using Cinemachine;
using Fusion;
using UnityEngine;

public class Weapons : NetworkBehaviour
{
    public CinemachineVirtualCamera camera;
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
    /// �߻�
    /// </summary>
    public void Fire(bool holdingPressed)
    {
        if (CurrentWeapon == null || IsSwitching)
            return;

        CurrentWeapon.Fire(fireTransform.position, fireTransform.forward, holdingPressed);
    }

    /// <summary>
    /// ������
    /// </summary>
    public void Reload()
    {
        if (CurrentWeapon == null || IsSwitching)
            return;

        CurrentWeapon.Reload();
    }

    /// <summary>
    /// ����
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
    /// ���� ��ü
    /// </summary>
    public void Swap()
    {

    }
}
