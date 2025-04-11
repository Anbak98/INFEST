using Fusion;
using UnityEngine;

public class TestDummyShooter : NetworkBehaviour
{
    [SerializeField] private Transform _fireTransform; // 총구 위치
    [SerializeField] private LayerMask _hitMask;       // 맞을 수 있는 레이어
    [SerializeField] private float _maxDistance = 100f;
    [SerializeField] private float _fireRate = 0.1f;

    [SerializeField] private NetworkPrefabRef _realProjectilePrefab;
    private float _lastFireTime;

    private void Update()
    {
        if (Input.GetMouseButton(0) && Time.time >= _lastFireTime + _fireRate)
        {
            Fire();
            _lastFireTime = Time.time;
        }
    }

    private void Fire()
    {
        Vector3 origin = _fireTransform.position;
        Vector3 direction = _fireTransform.forward;

        // Raycast로 충돌 위치 계산        
        Vector3 hitPosition;
        Vector3 hitNormal;
        bool showHitEffect = true;

        Ray ray = new Ray(origin, direction);
        if (Physics.Raycast(ray, out RaycastHit hitInfo, _maxDistance, _hitMask))
        {
            hitPosition = hitInfo.point;
            hitNormal = hitInfo.normal;

            // 피격된 오브젝트 로그 출력
            Debug.Log($"Hit: {hitInfo.collider.gameObject.name}");

            // Rigidbody가 있으면 밀기
            Rigidbody rb = hitInfo.collider.attachedRigidbody;
            Vector3 force = direction * 10f; // 힘 크기
            rb.AddForce(force, ForceMode.Impulse);
        }
        else
        {
            hitPosition = origin + direction * _maxDistance;
            hitNormal = -direction;
            showHitEffect = false; // 충돌 안 했으면 히트이펙트 없음
        }

        // 연출용 총알
        RPCSpawnDummyProjectile(origin, direction, hitPosition, hitNormal, showHitEffect);        

        // 실제 네트워크 발사체
        Runner.Spawn(_realProjectilePrefab, origin,
            Quaternion.LookRotation(direction), Object.InputAuthority,
           (runner, obj) => { obj.GetComponent<RealProjectile>().Init(direction); });
    }

    [Rpc(RpcSources.InputAuthority, RpcTargets.All)]
    private void RPCSpawnDummyProjectile(Vector3 origin, Vector3 direction,
        Vector3 hitPosition, Vector3 hitNormal, bool showHitEffect)
    {
        var projectile = DummyProjectilePool.Instance.Get();

        projectile.transform.position = origin;
        projectile.transform.rotation = Quaternion.LookRotation(direction);

        projectile.SetHit(hitPosition, hitNormal, showHitEffect);
    }
}
