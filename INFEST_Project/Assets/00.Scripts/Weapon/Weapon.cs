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
    public Animator animator;
    private CinemachineVirtualCamera _cam;
    //public Bullet bullet;
    public Recoil camRecoil;
    public Recoil gunRecoil;
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
    //public int curClip = 30; // ���� źâ�� ź��
    //public int possessionAmmo = 0; // ���� ź��
    [Networked] public int curClip { get; set; } // ���� źâ�� ź��
    [Networked] public int possessionAmmo { get; set; } // ���� ź��
    public int maxAmmo = 360; // �ִ� ź��
    public float reloadTime = 2.0f; // ���� �ӵ�
    [Range(1, 20)] public int ProjectilesPerShot = 1; // �߻�ü �� �Ѿ˼�

    [Header("Shop")]
    public int weaponPrice; // �� ����
    public int ammoPrice; // ź�� ����

    [Networked] public NetworkBool IsCollected { get; set; } = false; // �������ΰ�?
    [Networked] public NetworkBool IsReloading { get; set; } = false; // �������ΰ�?
    [Networked] public NetworkBool IsAiming { get; set; } = false; // �������ΰ�?
    [Networked] private TickTimer _fireCooldown { get; set; } // �ൿ �� ��Ÿ��
    [Networked, Capacity(32)] private NetworkArray<ProjectileData> _projectileData { get; }
    [Networked] private int _fireCount { get; set; } // ���� �߻� Ƚ��

    private int _visibleFireCount; // Ŭ���̾�Ʈ�� ���������� ó���� �߻� ��

    private int _fireTicks; // ���� �ð�
    private float _basicDispersion;
    private Vector3 _startPosition = new Vector3(0.15f, 0.5f, 1f);
    private Vector3 _targetPosition = new Vector3(0, 0.6f, 0.5f);

    public Transform firstPersonMuzzleTransform;
    public Transform thirdPersonMuzzleTransform;
    public DummyProjectile dummyProjectilePrefab; // �Ѿ� ������
    private DummyProjectile dummyProjectile; // �Ѿ� �ν��Ͻ�
    private bool _reloadingVisible; // ���̴� ���ε� ����
    [SerializeField] private Transform _fireTransform; // �ѱ� ��ġ
    [SerializeField] private NetworkPrefabRef _realProjectilePrefab;

    public override void FixedUpdateNetwork()
    {
        if (!IsCollected) return;

        if (curClip == 0)
        {
            StopAiming();
            Reload();
        }

        if (IsAiming)
        {
            _cam.m_Lens.FieldOfView = Mathf.Lerp(_cam.m_Lens.FieldOfView, 20, Time.deltaTime * 10);

            if (_cam.m_Lens.FieldOfView <= 21)
            {
                Dispersion = _basicDispersion - 0.3f;
                _cam.m_Lens.FieldOfView = 20;
            }
        }
        else if (!IsAiming && _cam != null)
        {
            _cam.m_Lens.FieldOfView = Mathf.Lerp(_cam.m_Lens.FieldOfView, 40, Time.deltaTime * 10);

            if (_cam.m_Lens.FieldOfView >= 39)
            {
                Dispersion = _basicDispersion + 0.3f;
                _cam.m_Lens.FieldOfView = 40;
                _cam = null;
            }
        }
        if (IsReloading && _fireCooldown.ExpiredOrNotRunning(Runner) && Type == EWeaponType.Shotgun)
        {
            IsReloading = false;
            possessionAmmo--;
            curClip++;
            Debug.Log("���� �Ϸ�");
            if (curClip < startClip)
                Reload();
        }
        else if (IsReloading && _fireCooldown.ExpiredOrNotRunning(Runner))
        {
            IsReloading = false;
            possessionAmmo += curClip;
            curClip = Mathf.Min(possessionAmmo, startClip);
            possessionAmmo -= Mathf.Min(possessionAmmo, startClip);
            Debug.Log("���� �Ϸ�");
            _fireCooldown = TickTimer.CreateFromSeconds(Runner, 0.25f);
        }
    }

    public override void Spawned()
    {
        if (HasStateAuthority)
        {
            _basicDispersion = Dispersion;
            //curClip = Mathf.Clamp(curClip, 0, startClip);
            possessionAmmo = maxAmmo;
            curClip = startClip;
        }

        _visibleFireCount = _fireCount;
        _reloadingVisible = IsReloading;

        float fireTime = 60f / fireRate;
        _fireTicks = Mathf.CeilToInt(fireTime / Runner.DeltaTime); // �ݿø�

        dummyProjectile = Instantiate(dummyProjectilePrefab, HasInputAuthority ? firstPersonMuzzleTransform : thirdPersonMuzzleTransform);
        dummyProjectile.FinishProjectile();
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
            Debug.Log("���� �ִϸ��̼� ����");

            if (IsReloading)
            {
                Debug.Log("���� ����");
            }

            _reloadingVisible = IsReloading;
        }
    }

    private void PlayFireEffect()
    {
        Debug.Log("�� ��� ���� ����");
        Debug.Log("�� ��� �ִϸ��̼� ����");
        int id = Animator.StringToHash("Fire");
        animator.SetTrigger(id);
        camRecoil.ApplyCamRecoil(IsAiming ? 0.5f : 1f);
        gunRecoil.ApplyGunRecoil(Dispersion);
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
        if (IsReloading) return;

        Random.InitState(Runner.Tick * unchecked((int)Object.Id.Raw)); // ������ ����

        for (int i = 0; i < ProjectilesPerShot; i++)
        {
            var projectileDirection = dir;

            if (Dispersion > 0f)
            {
                var dispersionRotation = Quaternion.Euler(Random.insideUnitSphere * Dispersion);
                projectileDirection = dispersionRotation * dir; // ź����
            }

            //int id = Animator.StringToHash("Fire");
            //animator.SetTrigger(id);
            //camRecoil.ApplyCamRecoil(IsAiming ? 0.5f : 1f);
            //gunRecoil.ApplyGunRecoil(Dispersion);
            FireProjectile(pos, projectileDirection);
        }

        _fireCooldown = TickTimer.CreateFromTicks(Runner, _fireTicks);
        curClip--;
    }

    private void FireProjectile(Vector3 firePosition, Vector3 fireDirection)
    {
        var projectileData = new ProjectileData();
        Vector3 origin = _fireTransform.position;
        Vector3 direction = _fireTransform.forward;

        var hitOptions = HitOptions.IncludePhysX | HitOptions.IgnoreInputAuthority;

        if (HasStateAuthority)
        {
            if (Runner.LagCompensation.Raycast(firePosition, fireDirection, maxHitDistance,
                    Object.InputAuthority, out var hit, HitMask, hitOptions))
            {
                projectileData.hitPosition = hit.Point;
                projectileData.hitNormal = hit.Normal;

                if (hit.Hitbox != null)
                {
                    //ApplyDamage(hit.Hitbox, hit.Point, fireDirection);
                }
                else
                {
                    projectileData.showHitEffect = true;
                }
            }
            else
            {
                projectileData.hitPosition = origin + direction * maxHitDistance;
                projectileData.hitNormal = -direction;
                projectileData.showHitEffect = false; // �浹 �� ������ ��Ʈ����Ʈ ����
            }
        }

        Rpc_SpawnDummyProjectile(origin, direction, projectileData.hitPosition, projectileData.hitNormal, projectileData.showHitEffect);

        if (HasStateAuthority)
        {
            Runner.Spawn(_realProjectilePrefab, origin, Quaternion.LookRotation(direction),
                Object.InputAuthority,
                (runner, obj) =>
                {
                    obj.GetComponent<RealProjectile>().Init(direction);
                });
        }

        _projectileData.Set(_fireCount % _projectileData.Length, projectileData);
        _fireCount++;
        //Runner.Spawn(bullet,
        //transform.position,
        //Quaternion.LookRotation(dir),
        //Object.InputAuthority,
        //(runner, o) =>
        //  {
        //      o.GetComponent<Bullet>().Init(pos, maxHitDistance);
        //  });

        Debug.Log("�ѽ��");
    }

    private void ApplyDamage(Hitbox enemyHitbox, Vector3 pos, Vector3 dir)
    {
        if (enemyHitbox == null) return;

        var enemyHealth = enemyHitbox.Root.GetComponent<TestHp>();
        if (enemyHealth == null || enemyHealth.isAlive == false)
            return;

        float damageMultiplier = enemyHitbox is BodyHitbox bodyHitbox ? bodyHitbox.DamageMultiplier : 1f;
        bool isCriticalHit = damageMultiplier > 1f;

        float hitdamage = damage * damageMultiplier;
        //if (_sceneObjects.Gameplay.DoubleDamageActive)
        //{
        //    damage *= 2f;
        //}

        if (enemyHealth.ApplyDamage(Object.InputAuthority, damage, pos, dir, Type, isCriticalHit) == false)
            return;

        //if (HasInputAuthority && Runner.IsForward)
        //{
        //    _sceneObjects.GameUI.PlayerView.Crosshair.ShowHit(enemyHealth.IsAlive == false, isCriticalHit);
        //}
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

        if (Type == EWeaponType.Shotgun)
        {
            int id = curClip == startClip - 1 ? Animator.StringToHash("Reload_End") : Animator.StringToHash("Reload_Start");
            animator.SetTrigger(id);
        }
        else
        {
            int id = curClip == 0 ? Animator.StringToHash("Reload_Empty") : Animator.StringToHash("Reload_Tac");
            animator.SetTrigger(id);
        }
        Debug.Log("���� ����");
        _fireCooldown = TickTimer.CreateFromSeconds(Runner, reloadTime);

    }

    /// <summary>
    /// ����
    /// </summary>
    public void Aiming(CinemachineVirtualCamera cam)
    {
        if (!IsCollected) return; // ���������� ������
        if (curClip <= 0) return; // ���� źâ�� ź���� ������
        if (IsAiming) return; // �������ΰ�?
        if (!_fireCooldown.ExpiredOrNotRunning(Runner)) return; // �ൿ���ΰ�?

        gunRecoil.ChangePosition(_targetPosition);
        _fireCooldown = TickTimer.CreateFromSeconds(Runner, 0.5f);
        IsAiming = true;
        _cam = cam;
        Debug.Log("������");
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

        gunRecoil.ChangePosition(_startPosition);
        _fireCooldown = TickTimer.CreateFromSeconds(Runner, 0.25f);
        IsAiming = false;
        Debug.Log("���س�");
    }
    private struct ProjectileData : INetworkStruct
    {
        public Vector3 hitPosition;
        public Vector3 hitNormal;
        public NetworkBool showHitEffect;
    }
}
