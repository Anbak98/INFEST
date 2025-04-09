using Cinemachine;
using Fusion;
using UnityEngine;

public enum EWeaponType
{
    None,
    Rifle,
    Sniper,
    Shotgun,
}

public class Weapon : NetworkBehaviour
{
    private CinemachineVirtualCamera _cam;
    public Bullet bullet;
    public EWeaponType Type; // ���� ����

    [Header("Firing")]
    public bool isAutomatic = false; // ���� or �ܹ�
    public float damage = 10f; // ���ݷ�
    public float fireRate = 100f; // ���� �ӵ�
    public float maxHitDistance = 100f; // ��Ÿ�
    public float Dispersion = 0; // ��ź��
    public LayerMask HitMask;

    [Header("Ammo")]
    public int startClip = 30; // ���� źâ�� ź��
    public int curClip = 30; // ���� źâ�� ź��
    public int maxAmmo = 360; // �ִ� ź��
    public int possessionAmmo = 0; // ���� ź��
    public float reloadTime = 2.0f; // ���� �ӵ�
    [Range(1, 20)] public int ProjectilesPerShot = 1; // �߻�ü �� �Ѿ˼�

    [Header("Shop")]
    public int weaponPrice; // �� ����
    public int ammoPrice; // ź�� ����

    [Networked] public NetworkBool IsCollected { get; set; } = false; // �������ΰ�?
    [Networked] public NetworkBool IsReloading { get; set; } = false; // �������ΰ�?
    [Networked] public NetworkBool IsAiming { get; set; } = false; // �������ΰ�?
    [Networked] private TickTimer _fireCooldown { get; set; } // �ൿ �� ��Ÿ��

    private int _fireTicks; // ���� �ð�

    public override void FixedUpdateNetwork()
    {
        if (!IsCollected) return;

        if (curClip == 0)
        {
            StopAiming();
            Reload();
        }

        if (IsReloading && _fireCooldown.ExpiredOrNotRunning(Runner))
        {
            IsReloading = false;
            possessionAmmo += curClip;
            curClip = Mathf.Min(possessionAmmo, startClip);
            possessionAmmo -= Mathf.Min(possessionAmmo, startClip);
            Debug.Log("����");
            _fireCooldown = TickTimer.CreateFromSeconds(Runner, 0.25f);
        }
    }

    public override void Spawned()
    {
        if (HasStateAuthority)
        {
            possessionAmmo = 29;
            curClip = Mathf.Clamp(curClip, 0, startClip);
        }

        float fireTime = 60f / fireRate;
        _fireTicks = Mathf.CeilToInt(fireTime / Runner.DeltaTime); // �ݿø�
    }


    /// <summary>
    /// �߻�
    /// </summary>
    public void Fire(Vector3 pos, Vector3 dir, bool holdingPressed)
    {
        if (!IsCollected) return;
        if (holdingPressed && !isAutomatic) return;
        if (!_fireCooldown.ExpiredOrNotRunning(Runner)) return;
        if (curClip == 0) return;

        Random.InitState(Runner.Tick * unchecked((int)Object.Id.Raw)); // ������ ����

        for (int i = 0; i < ProjectilesPerShot; i++)
        {
            var projectileDirection = dir;

            if (Dispersion > 0f)
            {
                var dispersionRotation = Quaternion.Euler(Random.insideUnitSphere * Dispersion);
                projectileDirection = dispersionRotation * dir; // ź����
            }

            FireProjectile(pos, projectileDirection);
        }

        _fireCooldown = TickTimer.CreateFromTicks(Runner, _fireTicks);
        curClip--;
    }

    private void FireProjectile(Vector3 pos, Vector3 dir)
    {
        Runner.Spawn(bullet,
        transform.position,
        Quaternion.LookRotation(dir),
        Object.InputAuthority,
        (runner, o) =>
          {
              o.GetComponent<Bullet>().Init(pos, maxHitDistance);
          });

        Debug.Log("�ѽ��");
    }

    /// <summary>
    /// ������
    /// </summary>
    public void Reload()
    {
        if (!IsCollected) return; // ���������� ������
        if (curClip == startClip) return; // ���� źâ�� ź���� �ִ��
        if (possessionAmmo <= 0) return; // �������� ź���� ������
        if (IsReloading) return; // �������̸�
        if (!_fireCooldown.ExpiredOrNotRunning(Runner)) return; // �ൿ ��Ÿ�����̸�

        IsReloading = true;
        _fireCooldown = TickTimer.CreateFromSeconds(Runner, reloadTime);
        Debug.Log("��������");

    }

    /// <summary>
    /// ����
    /// </summary>
    public void Aiming(CinemachineVirtualCamera cam)
    {
        if (!IsCollected) return; // ���������� ������
        if (curClip <= 0) return; // ���� źâ�� ź���� ������
        if (IsAiming) return;
        if (!_fireCooldown.ExpiredOrNotRunning(Runner)) return;


        _fireCooldown = TickTimer.CreateFromSeconds(Runner, 0.5f);
        IsAiming = true;
        cam.m_Lens.FieldOfView = 20;
        _cam = cam;
        Debug.Log("������");
    }

    public void StopAiming()
    {
        if (!IsCollected) return; // ���������� ������
        if (!IsAiming) return;
        if (!_fireCooldown.ExpiredOrNotRunning(Runner)) return;


        _fireCooldown = TickTimer.CreateFromSeconds(Runner, 0.25f);
        IsAiming = false;
        _cam.m_Lens.FieldOfView = 40;
        _cam = null;
        Debug.Log("���س�");
    }

}
