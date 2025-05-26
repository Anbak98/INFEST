using Cinemachine;
using Fusion;
using KINEMATION.FPSAnimationPack.Scripts.Weapon;
using UnityEngine;

public enum EWeaponType
{
    Pistol,
    Rifle,
    Sniper,
    Shotgun,
    Machinegun,
    Launcher,
}

public class Weapon : NetworkBehaviour
{
    public int key;
    public WeaponInstance instance;
    public FPSWeapon FPSWeapon;
    public EWeaponType Type; // ���� ����
    public Sprite icon;
    public NetworkPrefabRef rocket;

    [Header("Firing")]
    private LayerMask HitMask = ~(1 << 0 | 1 << 6 | 1 << 7 | 1 << 9 | 1 << 16);
    public float reloadTime = 2.0f; // ���� �ӵ�

    [Networked] public int curMagazineBullet { get; set; } // ���� źâ�� ź��
    [Networked] public int curBullet { get; set; } // ���� ź��
    [Networked] public NetworkBool IsCollected { get; set; } = false; // �������ΰ�?
    [Networked] public NetworkBool IsReloading { get; set; } = false; // �������ΰ�?
    [Networked] public NetworkBool IsAiming { get; set; } = false; // �������ΰ�?
    [Networked] private TickTimer _fireCooldown { get; set; } // �ൿ �� ��Ÿ��
    [Networked, Capacity(32)] private NetworkArray<ProjectileData> _projectileData { get; }
    [Networked] private int _fireCount { get; set; } // ���� �߻� Ƚ��

    private int _visibleFireCount; // Ŭ���̾�Ʈ�� ���������� ó���� �߻� ��

    private int _fireTicks; // ���� �ð�
    private float _basicDispersion;

    public Transform firstPersonMuzzleTransform;
    public Transform thirdPersonMuzzleTransform;
    public DummyProjectile dummyProjectilePrefab; // �Ѿ� ������
    private DummyProjectile dummyProjectile; // �Ѿ� �ν��Ͻ�
    private bool _reloadingVisible; // ���̴� ���ε� ����

    public override void FixedUpdateNetwork()
    {
        if (!IsCollected) return;

        if (curMagazineBullet == 0)
        {
            StopAiming();
            Reload();
        }

        if (IsReloading && _fireCooldown.ExpiredOrNotRunning(Runner))
        {
            IsReloading = false;
            curBullet += curMagazineBullet;
            curMagazineBullet = Mathf.Min(curBullet, instance.data.MagazineBullet);
            curBullet -= Mathf.Min(curBullet, instance.data.MagazineBullet);
            _fireCooldown = TickTimer.CreateFromSeconds(Runner, 0.25f);
        }
    }

    public void Init()
    {

        instance = new(key);
        //_basicDispersion = Dispersion;
        ////curClip = Mathf.Clamp(curClip, 0, startClip);
        //possessionAmmo = maxAmmo;
        //curClip = startClip;
        if (instance == null) return;

        Type = (EWeaponType)instance.data.WeaponType;
        curMagazineBullet = instance.data.MagazineBullet;
        curBullet = instance.data.MaxBullet;
        FPSWeapon.activeAmmo = curMagazineBullet;


        _visibleFireCount = _fireCount;
        _reloadingVisible = IsReloading;

        float fireTime = 60f / (instance.data.FireRate * 100);
        _fireTicks = Mathf.CeilToInt(fireTime / Runner.DeltaTime); // �ݿø�
    }

    public override void Render()
    {
        if (_visibleFireCount < _fireCount)
        {
            PlayFireEffect();
        }

        _visibleFireCount = _fireCount;

        if (_reloadingVisible != IsReloading)
        {
            if (IsReloading)
            {
            }

            _reloadingVisible = IsReloading;
        }
    }

    private void PlayFireEffect()
    {
        int id = Animator.StringToHash("Fire");
    }


    /// <summary>
    /// �߻�
    /// </summary>
    public void Fire(bool holdingPressed)
    {
        if (!IsCollected) return;
        if (!holdingPressed && !instance.data.IsAutpmatic) return;
        if (!_fireCooldown.ExpiredOrNotRunning(Runner)) return;
        if (curMagazineBullet == 0) return;
        if (IsReloading) return;
        FPSWeapon.RPC_OnFirePressed();
        Random.InitState(Runner.Tick * unchecked((int)Object.Id.Raw)); // ������ ����

        for (int i = 0; i < instance.data.ProjectilesPerShot; i++)
        {
            var projectileDirection = firstPersonMuzzleTransform.forward;

            if (instance.data.Concentration > 0f)
            {
                var dispersionRotation = Quaternion.Euler(Random.insideUnitSphere * instance.data.Concentration);
                projectileDirection = dispersionRotation * firstPersonMuzzleTransform.forward; // ź����
            }
        
            FireProjectile(firstPersonMuzzleTransform.position, projectileDirection);
        }

        _fireCooldown = TickTimer.CreateFromTicks(Runner, _fireTicks);
        curMagazineBullet--;
    }

    private void FireProjectile(Vector3 firePosition, Vector3 fireDirection)
    {
        var projectileData = new ProjectileData();
        Vector3 characterPos = this.transform.position;

        var hitOptions = HitOptions.IncludePhysX | HitOptions.IgnoreInputAuthority;

        if (HasStateAuthority)
        {
            if (Type == EWeaponType.Launcher)
            {
                Quaternion baseRotation = Quaternion.LookRotation(firePosition);
                Quaternion offsetRotation = Quaternion.Euler(0, 90f, 0);
                Quaternion finalRotation = transform.rotation;

                Runner.Spawn(rocket, firePosition, finalRotation, Object.InputAuthority).GetComponent<Rocket>().weapon = this;
            }
            else
            {
                if (Runner.LagCompensation.Raycast(characterPos, fireDirection, instance.data.WeaponRange,
                        Object.InputAuthority, out var hit, HitMask, hitOptions, QueryTriggerInteraction.Ignore))
                {
                    Debug.Log($"{HitMask}");
                    projectileData.hitPosition = hit.Point;
                    projectileData.hitNormal = hit.Normal;

                    if (hit.Hitbox != null)
                    {
                        ApplyDamage(hit.Hitbox, hit.Point, fireDirection);
                    }
                    else
                    {
                        projectileData.showHitEffect = true;
                    }
                }
                else
                {
                    projectileData.hitPosition = characterPos + fireDirection * instance.data.WeaponRange;
                    projectileData.hitNormal = -fireDirection;
                    projectileData.showHitEffect = false; // �浹 �� ������ ��Ʈ����Ʈ ����
                }
                Rpc_SpawnDummyProjectile(firePosition, fireDirection, projectileData.hitPosition, projectileData.hitNormal, projectileData.showHitEffect);
                _projectileData.Set(_fireCount % _projectileData.Length, projectileData);
                _fireCount++;
            }
        }
    }

    private void ApplyDamage(Hitbox enemyHitbox, Vector3 pos, Vector3 dir)
    {
        if (enemyHitbox == null) return;

        var enemyHealth = enemyHitbox.Root.GetComponent<MonsterNetworkBehaviour>();        
        if (enemyHealth == null || enemyHealth.IsDead == true)
            return;

        float damageMultiplier = enemyHitbox is HeadHitbox bodyHitbox ? bodyHitbox.DamageMultiplier : 1f;
        bool isCriticalHit = damageMultiplier > 1f;

        float hitdamage = instance.data.Atk * damageMultiplier;

        if (enemyHealth.ApplyDamage(Object.InputAuthority, hitdamage, pos, dir, Type, isCriticalHit) == false)
            return;
    }

    /// <summary>
    /// ������
    /// </summary>
    public void Reload()
    {
        if (!IsCollected) return; // ���������� ������
        if (curMagazineBullet == instance.data.MagazineBullet) return; // ���� źâ�� ź���� �ִ��
        if (curBullet <= 0) return; // �������� ź���� ������
        if (IsReloading) return; // �������̸�
        if (!_fireCooldown.ExpiredOrNotRunning(Runner)) return; // �ൿ ��Ÿ�����̸�

        if(HasStateAuthority)
            FPSWeapon.RPC_OnReload();

        IsReloading = true;

        if (Type == EWeaponType.Shotgun)
        {
            int id = curMagazineBullet == instance.data.MagazineBullet - 1 ? Animator.StringToHash("Reload_End") : Animator.StringToHash("Reload_Start");
            //animator.SetTrigger(id);
        }
        else
        {
            int id = curMagazineBullet == 0 ? Animator.StringToHash("Reload_Empty") : Animator.StringToHash("Reload_Tac");
            //animator.SetTrigger(id);
        }
        _fireCooldown = TickTimer.CreateFromSeconds(Runner, reloadTime);

    }

    /// <summary>
    /// ����
    /// </summary>
    public void Aiming(/*CinemachineVirtualCamera cam*/)
    {
        if (!IsCollected) return; // ���������� ������
        if (curMagazineBullet <= 0) return; // ���� źâ�� ź���� ������
        if (IsAiming) return; // �������ΰ�?
        if (!_fireCooldown.ExpiredOrNotRunning(Runner)) return; // �ൿ���ΰ�?


        //gunRecoil.ChangePosition(_targetPosition);
        _fireCooldown = TickTimer.CreateFromSeconds(Runner, 0.5f);
        IsAiming = true;
        instance.IsAiming();

        //_cam = cam;
        Debug.Log("������\n ��ź�� : " + instance.concentration);
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    private void Rpc_SpawnDummyProjectile(Vector3 pos, Vector3 dir, Vector3 hitPos, Vector3 hitNormal, bool showHitEffect)
    {
        var dummy = DummyProjectilePool.Instance.Get();
        dummy.transform.position = pos;
        dummy.transform.rotation = Quaternion.LookRotation(dir);
        dummy.SetHit(hitPos, hitNormal, showHitEffect);
    }

    public void StopAiming()
    {
        if (!IsCollected) return; // ���������� ������
        if (!IsAiming) return; //�������� �ƴѰ�?
        if (!_fireCooldown.ExpiredOrNotRunning(Runner)) return; // �ൿ���ΰ�?

        //gunRecoil.ChangePosition(_startPosition);
        _fireCooldown = TickTimer.CreateFromSeconds(Runner, 0.25f);
        IsAiming = false;
        instance.StopAiming();


        Debug.Log("���س�\n ��ź�� : " + instance.concentration);
    }
    private struct ProjectileData : INetworkStruct
    {
        public Vector3 hitPosition;
        public Vector3 hitNormal;
        public NetworkBool showHitEffect;
    }

    public void SupplementBullet()
    {
        curBullet += instance.data.MagazineBullet;
        curBullet = Mathf.Min(curBullet, instance.data.MaxBullet);
    }
}
