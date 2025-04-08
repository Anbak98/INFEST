using UnityEngine;

public class TestDummyShooter : MonoBehaviour
{
    [SerializeField] private Transform _fireTransform; // 총구 위치
    [SerializeField] private LayerMask _hitMask;       // 맞을 수 있는 레이어
    [SerializeField] private float _maxDistance = 100f;

    [SerializeField] private float _fireRate = 0.1f;
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
        // 오브젝트 풀에서 총알 꺼내기
        var projectile = DummyProjectilePool.Instance.Get();

        // 위치 세팅
        projectile.transform.position = _fireTransform.position;
        projectile.transform.rotation = _fireTransform.rotation;

        // Raycast로 충돌 위치 계산
        Vector3 hitPosition;
        Vector3 hitNormal;
        bool showHitEffect = true;

        Ray ray = new Ray(_fireTransform.position, _fireTransform.forward);
        if (Physics.Raycast(ray, out RaycastHit hitInfo, _maxDistance, _hitMask))
        {
            hitPosition = hitInfo.point;
            hitNormal = hitInfo.normal;
        }
        else
        {
            hitPosition = _fireTransform.position + _fireTransform.forward * _maxDistance;
            hitNormal = -_fireTransform.forward;
            showHitEffect = false; // 충돌 안 했으면 히트이펙트 없음
        }

        // 총알에 hit 정보 넘기기
        projectile.SetHit(hitPosition, hitNormal, showHitEffect);
    }
}
