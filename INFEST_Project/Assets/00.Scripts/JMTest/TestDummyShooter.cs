using Fusion;
using UnityEngine;

public class TestDummyShooter : NetworkBehaviour
{
    [SerializeField] private Transform _fireTransform; // �ѱ� ��ġ
    [SerializeField] private LayerMask _hitMask;       // ���� �� �ִ� ���̾�
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

        // Raycast�� �浹 ��ġ ���        
        Vector3 hitPosition;
        Vector3 hitNormal;
        bool showHitEffect = true;

        Ray ray = new Ray(origin, direction);
        if (Physics.Raycast(ray, out RaycastHit hitInfo, _maxDistance, _hitMask))
        {
            hitPosition = hitInfo.point;
            hitNormal = hitInfo.normal;

            // �ǰݵ� ������Ʈ �α� ���
            Debug.Log($"Hit: {hitInfo.collider.gameObject.name}");

            // Rigidbody�� ������ �б�
            Rigidbody rb = hitInfo.collider.attachedRigidbody;
            Vector3 force = direction * 10f; // �� ũ��
            rb.AddForce(force, ForceMode.Impulse);
        }
        else
        {
            hitPosition = origin + direction * _maxDistance;
            hitNormal = -direction;
            showHitEffect = false; // �浹 �� ������ ��Ʈ����Ʈ ����
        }

        // ����� �Ѿ�
        RPCSpawnDummyProjectile(origin, direction, hitPosition, hitNormal, showHitEffect);        

        // ���� ��Ʈ��ũ �߻�ü
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
