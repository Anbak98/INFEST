using UnityEngine;

public class DummyProjectile : MonoBehaviour
{
    [Header("Projectile Settings")]
    [SerializeField] private float _speed = 160f; // 총알 속도
    [SerializeField] private float _lifeTimeAfterHit = 2f; // 오브젝트 삭제 대기시간

    [Header("Visuals")]
    [SerializeField] private GameObject _hitEffect; // 히트이펙트
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

    // 총알 지속시간이 끝났을 때
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

    // 풀에 돌아갈 때 초기화
    private void ReturnToPool()
    {
        _hitEffect.SetActive(false);
        _visualRoot.SetActive(true);

        transform.position = Vector3.zero;

        DummyProjectilePool.Instance.Return(this);
    }

    // 프로젝타일이 켜질때 초기화
    public void ResetProjectile()
    {
        _showHitEffect = false;
        _hitEffect.SetActive(false);
        _visualRoot.SetActive(true);

        enabled = true;
    }
}