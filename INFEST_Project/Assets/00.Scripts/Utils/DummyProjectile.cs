using UnityEngine;

public class DummyProjectile : MonoBehaviour
{
    [SerializeField] private float _speed = 80f; // 총알 속도
    [SerializeField] private float _maxDistance = 100f; // 최대 비행 거리
    [SerializeField] private GameObject _hitEffect; // 히트이펙트
    [SerializeField] private float _lifeTimeAfterHit = 2f; // 오브젝트 삭제 대기시간
    [SerializeField] private GameObject _visualRoot; // 총알 외형 오브젝트

    private Vector3 _startPosition; // 총알 출발 위치
    private Vector3 _targetPosition; // 총알 목표 위치
    private Vector3 _hitNormal;

    private bool _showHitEffect; // 목표 지점 도달 시 이펙트 여부

    private float _startTime; // 총알이 생성된 시간
    private float _duration; // 총알 목표까지의 시간


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