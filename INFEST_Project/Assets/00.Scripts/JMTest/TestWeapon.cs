using Fusion;
using UnityEngine;

public class TestWeapon : NetworkBehaviour
{
    public EWeaponType Type; // 무기 종류

    [Header("Firing")]
    public bool isAutomatic = false; // 연사 or 단발
    public float damage = 10f; // 공격력
    public float fireRate = 100f; // 공격 속도
    public float maxHitDistance = 100f; // 사거리
    public float Dispersion = 0; // 집탄율
    public LayerMask HitMask;

    [Header("Ammo")]
    public int startClip = 30; // 시작 탄창의 탄약
    [Networked] public int curClip { get; set; } // 현재 탄창의 탄약
    [Networked] public int possessionAmmo { get; set; } // 보유 탄약
    public int maxAmmo = 360; // 최대 탄약
    public float reloadTime = 2.0f; // 장전 속도
    [Range(1, 20)] public int ProjectilesPerShot = 1; // 발사체 당 총알수

    [Header("Shop")]
    public int weaponPrice; // 총 가격
    public int ammoPrice; // 탄약 가격

    [Networked] public NetworkBool IsCollected { get; set; } = false; // 보유중인가?
    [Networked] public NetworkBool IsReloading { get; set; } = false; // 장전중인가?
    [Networked] private TickTimer _fireCooldown { get; set; } // 행동 후 쿨타임

    [Networked, Capacity(32)]
    private NetworkArray<ProjectileData> _projectileData { get; }

    private int _fireTicks; // 재사격 시간
    [Networked] private int _fireCount { get; set; } // 누적 발사 횟수

    private int _visibleFireCount; // 클라이언트가 마지막으로 처리한 발사 수

    public Transform firstPersonMuzzleTransform;
    public Transform thirdPersonMuzzleTransform;
    public DummyProjectile dummyProjectilePrefab; // 총알 프리팹
    private DummyProjectile dummyProjectile; // 
    private bool _reloadingVisible; // 보이는 리로딩 상태
    [SerializeField] private Transform _fireTransform; // 총구 위치
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
                Debug.Log("장전");
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
        _fireTicks = Mathf.CeilToInt(fireTime / Runner.DeltaTime); // 반올림

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
            Debug.Log("장전 애니메이션 실행");

            if (IsReloading)
            {
                Debug.Log("장전 사운드");
            }

            _reloadingVisible = IsReloading;
        }
    }

    private void PlayFireEffect()
    {
        Debug.Log("탕!");
        Debug.Log("총 쏘는 애니메이션 실행");
    }

    /// <summary>
    /// 발사
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

        Random.InitState(Runner.Tick * unchecked((int)Object.Id.Raw)); // 랜덤값 고정

        for (int i = 0; i < ProjectilesPerShot; i++)
        {
            var projectileDirection = dir;

            if (Dispersion > 0f)
            {
                var dispersionRotation = Quaternion.Euler(Random.insideUnitSphere * Dispersion);
                projectileDirection = dispersionRotation * dir; // 탄퍼짐
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
                projectileData.showHitEffect = false; // 충돌 안 했으면 히트이펙트 없음
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

        Debug.Log("총쏜다");
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
    /// 재장전
    /// </summary>
    public void Reload()
    {
        if (!IsCollected) return; // 보유중이지 않으면
        if (curClip == startClip) return; // 현재 탄창의 탄약이 최대면
        if (possessionAmmo <= 0) return; // 보유중인 탄약이 없으면
        if (IsReloading) return; // 장전중이면
        if (!_fireCooldown.ExpiredOrNotRunning(Runner)) return; // 행동 쿨타임중이면

        if (HasStateAuthority)
        {
            IsReloading = true;
            _fireCooldown = TickTimer.CreateFromSeconds(Runner, reloadTime);
            Debug.Log("장전시작");
        }
    }

    /// <summary>
    /// 조준
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
