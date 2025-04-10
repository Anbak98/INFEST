using Fusion;
using UnityEngine;

public class TestWeapons : NetworkBehaviour
{
    public Transform fireTransform;
    public float weaponSwitchTime = 1f;

    public bool IsSwitching => _switchTimer.ExpiredOrNotRunning(Runner) == false;

    public TestWeapon CurrentWeapon { get; set; }

    [HideInInspector] public TestWeapon[] AllWeapons;

    private TickTimer _switchTimer { get; set; }

    private void Awake()
    {
        AllWeapons = GetComponentsInChildren<TestWeapon>();
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
    /// ���� ��ü
    /// </summary>
    public void Swap()
    {

    }
}
