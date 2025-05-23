using UnityEngine;

public class DummyProjectile : MonoBehaviour
{
    [Header("Projectile Settings")]
    [SerializeField] private float _speed = 160f; // �Ѿ� �ӵ�
    [SerializeField] private float _lifeTimeAfterHit = 2f; // ������Ʈ ���� ���ð�

    [Header("Visuals")]
    [SerializeField] private GameObject _hitEffect; // ��Ʈ����Ʈ
    [SerializeField] private GameObject _visualRoot; // �Ѿ� ���� ������Ʈ

    private Vector3 _startPosition; // �Ѿ� ��� ��ġ
    private Vector3 _targetPosition; // �Ѿ� ��ǥ ��ġ
    private Vector3 _hitNormal;

    private bool _showHitEffect; // ��ǥ ���� ���� �� ����Ʈ ����
    private float _startTime; // �Ѿ��� ������ �ð�
    private float _duration; // �Ѿ� ��ǥ������ �ð�

    public void SetHit(Vector3 hitPosition, Vector3 hitNormal, bool showHitEffect)
    {
        _targetPosition = hitPosition;
        _showHitEffect = showHitEffect;
        _hitNormal = hitNormal;

        _startPosition = transform.position;
        _duration = Vector3.Distance(_startPosition, _targetPosition) / _speed;
        _startTime = Time.timeSinceLevelLoad;
    }

    private void Update()
    {
        float time = Time.timeSinceLevelLoad - _startTime;

        if (time < _duration)
        {
            transform.position = Vector3.Lerp(_startPosition, _targetPosition, time / _duration);
        }
        else
        {
            transform.position = _targetPosition;
            FinishProjectile();
        }
    }

    // �Ѿ� ���ӽð��� ������ ��
    public void FinishProjectile()
    {
        if (_showHitEffect == false)
        {
            DummyProjectilePool.Instance.Return(this);
            return;
        }

        enabled = false;

        _hitEffect.SetActive(true);
        _visualRoot.SetActive(false);

        Invoke(nameof(ReturnToPool), _lifeTimeAfterHit);
    }

    // Ǯ�� ���ư� �� �ʱ�ȭ
    private void ReturnToPool()
    {
        _hitEffect.SetActive(false);
        _visualRoot.SetActive(true);

        transform.position = Vector3.zero;

        DummyProjectilePool.Instance.Return(this);
    }

    // ������Ÿ���� ������ �ʱ�ȭ
    public void ResetProjectile()
    {
        _showHitEffect = false;
        _hitEffect.SetActive(false);
        _visualRoot.SetActive(true);

        enabled = true;
    }
}