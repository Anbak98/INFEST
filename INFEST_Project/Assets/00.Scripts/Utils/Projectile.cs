using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float _speed = 80f;
    [SerializeField] private float _maxDistance = 100f;
    [SerializeField] private GameObject _hitEffect;
    [SerializeField] private float _lifeTimeAfterHit = 2f;
    [SerializeField] private GameObject _visualRoot;

    private Vector3 _startPosition;
    private Vector3 _targetPosition;
    private bool _showHitEffect;

    private float _startTime;
    private float _duration;

    public void SetHitPosition(Vector3 hitPosition)
    {
        _targetPosition = hitPosition;
        _showHitEffect = hitPosition != Vector3.zero;
    }

    private void Awake()
    {
        if(_hitEffect != null)
        {
            _hitEffect.SetActive(false);
        }
    }

    private void Start()
    {
        _startPosition = transform.position;

        if(_targetPosition == Vector3.zero)
        {
            _targetPosition = _startPosition + transform.forward * _maxDistance;
        }

        _duration = Vector3.Distance(_startPosition, _targetPosition) / _speed;
        _startTime = Time.timeSinceLevelLoad;
    }

    private void Update()
    {
        float PlayTime = Time.timeSinceLevelLoad - _startTime;

        if (PlayTime < _duration)
        {
            transform.position = Vector3.Lerp(_startPosition, _targetPosition, PlayTime / _duration);
        }
        else
        {
            transform.position = _targetPosition;
            enabled = false;

            if (_showHitEffect == true && _hitEffect != null)
            {
                if (_visualRoot != null)
                {
                    _visualRoot.SetActive(false);
                }

                _hitEffect.SetActive(true);
                Destroy(gameObject, _lifeTimeAfterHit);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}
