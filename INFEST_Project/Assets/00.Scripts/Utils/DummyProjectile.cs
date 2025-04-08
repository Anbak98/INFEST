using UnityEngine;

public class DummyProjectile : MonoBehaviour
{
    [SerializeField] private float _speed = 80f; // �Ѿ� �ӵ�
    [SerializeField] private float _maxDistance = 100f; // �ִ� ���� �Ÿ�
    [SerializeField] private GameObject _hitEffect; // ��Ʈ����Ʈ
    [SerializeField] private float _lifeTimeAfterHit = 2f; // ������Ʈ ���� ���ð�
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
    }

    private void Start()
    {
        _startPosition = transform.position;

        if (_targetPosition == Vector3.zero)
        {
            _targetPosition = _startPosition + transform.forward * _maxDistance;
        }

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

    private void FinishProjectile()
    {
        if (_showHitEffect == false)
        {
            DummyProjectilePool.Instance.Return(this);
            return; 
        }

        enabled = false;

        if(_visualRoot != null)
        {
            _visualRoot.gameObject.SetActive(false);
        }

        if(_hitEffect != null)
        {
            Instantiate(_hitEffect, _targetPosition,
                Quaternion.LookRotation(_hitNormal), transform);
        }

        Invoke(nameof(ReturnToPool), _lifeTimeAfterHit);
    }

    private void ReturnToPool()
    {
        DummyProjectilePool.Instance.Return(this);
    }   

    public void ResetProjectile()
    {
        _startPosition = Vector3.zero;
        _targetPosition = Vector3.zero;
        _hitNormal = Vector3.zero;

        _showHitEffect = false;

        _startTime = 0f;
        _duration = 0f;

        enabled = true;

        if(_visualRoot != null )
        {
            _visualRoot.SetActive(true);
        }
    }
}