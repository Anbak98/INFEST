using UnityEngine;

public class TestDummyShooter : MonoBehaviour
{
    [SerializeField] private Transform _fireTransform; // �ѱ� ��ġ
    [SerializeField] private LayerMask _hitMask;       // ���� �� �ִ� ���̾�
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
        // ������Ʈ Ǯ���� �Ѿ� ������
        var projectile = DummyProjectilePool.Instance.Get();

        // ��ġ ����
        projectile.transform.position = _fireTransform.position;
        projectile.transform.rotation = _fireTransform.rotation;

        // Raycast�� �浹 ��ġ ���
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
            showHitEffect = false; // �浹 �� ������ ��Ʈ����Ʈ ����
        }

        // �Ѿ˿� hit ���� �ѱ��
        projectile.SetHit(hitPosition, hitNormal, showHitEffect);
    }
}
