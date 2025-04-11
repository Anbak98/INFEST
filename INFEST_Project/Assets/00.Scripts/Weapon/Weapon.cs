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
    //public int curClip = 30; // 현재 탄창의 탄약
    //public int possessionAmmo = 0; // 보유 탄약
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
    [Networked] public NetworkBool IsAiming { get; set; } = false; // 장전중인가?
    [Networked] private TickTimer _fireCooldown { get; set; } // 행동 후 쿨타임
    [Networked, Capacity(32)] private NetworkArray<ProjectileData> _projectileData { get; }
    [Networked] private int _fireCount { get; set; } // 누적 발사 횟수

    private int _visibleFireCount; // 클라이언트가 마지막으로 처리한 발사 수

    private int _fireTicks; // 재사격 시간
    private float _basicDispersion;
    private Vector3 _startPosition = new Vector3(0.15f, 0.5f, 1f);
    private Vector3 _targetPosition = new Vector3(0, 0.6f, 0.5f);

    public Transform firstPersonMuzzleTransform;
    public Transform thirdPersonMuzzleTransform;
    public DummyProjectile dummyProjectilePrefab; // 총알 프리팹
    private DummyProjectile dummyProjectile; // 총알 인스턴스
    private bool _reloadingVisible; // 보이는 리로딩 상태
    [SerializeField] private Transform _fireTransform; // 총구 위치
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
            Debug.Log("장전 완료");
            if (curClip < startClip)
                Reload();
        }
        else if (IsReloading && _fireCooldown.ExpiredOrNotRunning(Runner))
        {
            IsReloading = false;
            possessionAmmo += curClip;
            curClip = Mathf.Min(possessionAmmo, startClip);
            possessionAmmo -= Mathf.Min(possessionAmmo, startClip);
            Debug.Log("장전 완료");
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
        Debug.Log("총 쏘는 사운드 실행");
        Debug.Log("총 쏘는 애니메이션 실행");
        int id = Animator.StringToHash("Fire");
        animator.SetTrigger(id);
        camRecoil.ApplyCamRecoil(IsAiming ? 0.5f : 1f);
        gunRecoil.ApplyGunRecoil(Dispersion);
    }


    /// <summary>
    /// 발사
    /// </summary>
    public void Fire(Vector3 pos, Vector3 dir, bool holdingPressed)
    {
        if (!IsCollected) return;
        if (holdingPressed && !isAutomatic) return;
        if (!_fireCooldown.ExpiredOrNotRunning(Runner)) return;
        if (curClip == 0) return;
        if (IsReloading) return;

        Random.InitState(Runner.Tick * unchecked((int)Object.Id.Raw)); // 랜덤값 고정

        for (int i = 0; i < ProjectilesPerShot; i++)
        {
            var projectileDirection = dir;

            if (Dispersion > 0f)
            {
                var dispersionRotation = Quaternion.Euler(Random.insideUnitSphere * Dispersion);
                projectileDirection = dispersionRotation * dir; // 탄퍼짐
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
        //Runner.Spawn(bullet,
        //transform.position,
        //Quaternion.LookRotation(dir),
        //Object.InputAuthority,
        //(runner, o) =>
        //  {
        //      o.GetComponent<Bullet>().Init(pos, maxHitDistance);
        //  });

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
        Debug.Log("장전 시작");
        _fireCooldown = TickTimer.CreateFromSeconds(Runner, reloadTime);

    }

    /// <summary>
    /// 조준
    /// </summary>
    public void Aiming(CinemachineVirtualCamera cam)
    {
        if (!IsCollected) return; // 보유중이지 않으면
        if (curClip <= 0) return; // 현재 탄창에 탄약이 없으면
        if (IsAiming) return; // 조준중인가?
        if (!_fireCooldown.ExpiredOrNotRunning(Runner)) return; // 행동중인가?

        gunRecoil.ChangePosition(_targetPosition);
        _fireCooldown = TickTimer.CreateFromSeconds(Runner, 0.5f);
        IsAiming = true;
        _cam = cam;
        Debug.Log("조준중");
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
        if (!IsCollected) return; // 보유중이지 않으면
        if (!IsAiming) return; //조준중이 아닌가?
        if (!_fireCooldown.ExpiredOrNotRunning(Runner)) return; // 행동중인가?

        gunRecoil.ChangePosition(_startPosition);
        _fireCooldown = TickTimer.CreateFromSeconds(Runner, 0.25f);
        IsAiming = false;
        Debug.Log("조준끝");
    }
    private struct ProjectileData : INetworkStruct
    {
        public Vector3 hitPosition;
        public Vector3 hitNormal;
        public NetworkBool showHitEffect;
    }
}
