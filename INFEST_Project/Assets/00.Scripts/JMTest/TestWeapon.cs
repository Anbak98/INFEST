using Fusion;
using UnityEngine;

public class TestWeapon : NetworkBehaviour
{
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
    [Networked] private TickTimer _fireCooldown { get; set; } // �ൿ �� ��Ÿ��

    [Networked, Capacity(32)]
    private NetworkArray<ProjectileData> _projectileData { get; }

    private int _fireTicks; // ���� �ð�
    [Networked] private int _fireCount { get; set; } // ���� �߻� Ƚ��

    private int _visibleFireCount; // Ŭ���̾�Ʈ�� ���������� ó���� �߻� ��

    public Transform firstPersonMuzzleTransform;
    public Transform thirdPersonMuzzleTransform;
    public DummyProjectile dummyProjectilePrefab; // �Ѿ� ������
    private DummyProjectile dummyProjectile; // 
    private bool _reloadingVisible; // ���̴� ���ε� ����
    [SerializeField] private Transform _fireTransform; // �ѱ� ��ġ
    [SerializeField] private NetworkPrefabRef _realProjectilePrefab;

    public override void FixedUpdateNetwork()
    {
        if (!IsCollected) return;

        if (curClip == 0)
        {
            Reload();
        }

        if (IsReloading && _fireCooldown.ExpiredOrNotRunning(Runner))
        {
            if (HasStateAuthority)
            {
                IsReloading = false;

                int neededAmmo = startClip - curClip;
                int ammoToLoad = Mathf.Min(neededAmmo, possessionAmmo);

                curClip += ammoToLoad;
                possessionAmmo -= ammoToLoad;
                Debug.Log("����");
                _fireCooldown = TickTimer.CreateFromSeconds(Runner, 0.25f);
            }
        }
    }

    public override void Spawned()
    {
        if (HasStateAuthority)
        {
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
        Debug.Log("��!");
        Debug.Log("�� ��� �ִϸ��̼� ����");
    }

    /// <summary>
    /// �߻�
    /// </summary>
    public void Fire(Vector3 pos, Vector3 dir, bool holdingPressed)
    {
        if (!IsCollected)
            return;
        if (holdingPressed && !isAutomatic)
            return;
        if (IsReloading)
            return;
        if (!_fireCooldown.ExpiredOrNotRunning(Runner))
            return;
        if (curClip == 0)
            return;

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
                    ApplyDamage(hit.Hitbox, hit.Point, fireDirection);
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

        if (HasStateAuthority)
        {
            IsReloading = true;
            _fireCooldown = TickTimer.CreateFromSeconds(Runner, reloadTime);
            Debug.Log("��������");
        }
    }

    /// <summary>
    /// ����
    /// </summary>
    public void Aiming()
    {

    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    private void Rpc_SpawnDummyProjectile(Vector3 pos, Vector3 dir, Vector3 hitPos, Vector3 hitNormal, bool showHitEffect)
    {
        var dummy = DummyProjectilePool.Instance.Get();
        dummy.transform.position = pos;
        dummy.transform.rotation = Quaternion.LookRotation(dir);
        dummy.SetHit(hitPos, hitNormal, showHitEffect);
    }

    private struct ProjectileData : INetworkStruct
    {
        public Vector3 hitPosition;
        public Vector3 hitNormal;
        public NetworkBool showHitEffect;
    }
}
