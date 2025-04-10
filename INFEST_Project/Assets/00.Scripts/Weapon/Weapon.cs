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
    public Bullet bullet;
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
    public int curClip = 30; // 현재 탄창의 탄약
    public int maxAmmo = 360; // 최대 탄약
    public int possessionAmmo = 0; // 보유 탄약
    public float reloadTime = 2.0f; // 장전 속도
    [Range(1, 20)] public int ProjectilesPerShot = 1; // 발사체 당 총알수

    [Header("Shop")]
    public int weaponPrice; // 총 가격
    public int ammoPrice; // 탄약 가격

    [Networked] public NetworkBool IsCollected { get; set; } = false; // 보유중인가?
    [Networked] public NetworkBool IsReloading { get; set; } = false; // 장전중인가?
    [Networked] public NetworkBool IsAiming { get; set; } = false; // 장전중인가?
    [Networked] private TickTimer _fireCooldown { get; set; } // 행동 후 쿨타임

    private int _fireTicks; // 재사격 시간
    private float _basicDispersion;
    private Vector3 _startPosition = new Vector3(0.15f, 0.5f, 1f);
    private Vector3 _targetPosition = new Vector3(0, 0.6f, 0.5f);

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
        else if(!IsAiming && _cam != null)
        {
            _cam.m_Lens.FieldOfView = Mathf.Lerp(_cam.m_Lens.FieldOfView, 40, Time.deltaTime * 10);

            if (_cam.m_Lens.FieldOfView >= 39)
            {
                Dispersion = _basicDispersion + 0.3f;
                _cam.m_Lens.FieldOfView = 40;
                _cam = null;
            }
        }

        if (IsReloading && _fireCooldown.ExpiredOrNotRunning(Runner))
        {
            IsReloading = false;
            possessionAmmo += curClip;
            curClip = Mathf.Min(possessionAmmo, startClip);
            possessionAmmo -= Mathf.Min(possessionAmmo, startClip);
            Debug.Log("장전");
            _fireCooldown = TickTimer.CreateFromSeconds(Runner, 0.25f);
        }
    }

    public override void Spawned()
    {
        if (HasStateAuthority)
        {
            _basicDispersion = Dispersion;
            possessionAmmo = 29;
            curClip = Mathf.Clamp(curClip, 0, startClip);
        }

        float fireTime = 60f / fireRate;
        _fireTicks = Mathf.CeilToInt(fireTime / Runner.DeltaTime); // 반올림
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

        Random.InitState(Runner.Tick * unchecked((int)Object.Id.Raw)); // 랜덤값 고정

        for (int i = 0; i < ProjectilesPerShot; i++)
        {
            var projectileDirection = dir;

            if (Dispersion > 0f)
            {
                var dispersionRotation = Quaternion.Euler(Random.insideUnitSphere * Dispersion);
                projectileDirection = dispersionRotation * dir; // 탄퍼짐
            }

            int id = Animator.StringToHash("Fire");
            animator.SetTrigger(id);
            camRecoil.ApplyCamRecoil(Dispersion);
            gunRecoil.ApplyGunRecoil(Dispersion);
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

        Debug.Log("총쏜다");
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
        int id = curClip == 0 ? Animator.StringToHash("Reload_Empty") : Animator.StringToHash("Reload_Tac");
        animator.SetTrigger(id);
        _fireCooldown = TickTimer.CreateFromSeconds(Runner, reloadTime);
        Debug.Log("장전시작");

    }

    /// <summary>
    /// 조준
    /// </summary>
    public void Aiming(CinemachineVirtualCamera cam)
    {
        if (!IsCollected) return; // 보유중이지 않으면
        if (curClip <= 0) return; // 현재 탄창에 탄약이 없으면
        if (IsAiming) return;
        if (!_fireCooldown.ExpiredOrNotRunning(Runner)) return;

        gunRecoil.ChangePosition(_targetPosition);
        _fireCooldown = TickTimer.CreateFromSeconds(Runner, 0.5f);
        IsAiming = true;
        _cam = cam;
        Debug.Log("조준중");
    }

    public void StopAiming()
    {
        if (!IsCollected) return; // 보유중이지 않으면
        if (!IsAiming) return;
        if (!_fireCooldown.ExpiredOrNotRunning(Runner)) return;

        gunRecoil.ChangePosition(_startPosition);
        _fireCooldown = TickTimer.CreateFromSeconds(Runner, 0.25f);
        IsAiming = false;
        Debug.Log("조준끝");
    }

}
