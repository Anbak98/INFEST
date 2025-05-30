using System.Collections.Generic;
using Fusion;
using UnityEngine;

public class GrenadeProjectile : NetworkBehaviour
{

    TickTimer _activeTimer;
    TickTimer _lifeTimer;
    public GrenadeExplosion GrenadeExplosion;
    public GameObject explosionParticles;
    private float _time;
    private Vector3 _gravity = new Vector3(0, -9.81f, 0);
    public Transform throwPoint;
    private Vector3 _velocity;
    public Grenade obj;
    public GameObject rendering;
    private float _castRadius = 0.2f;

    private float _boundCnt = 0;
    private bool _isBound = false;
    private bool _isStopped = false;
    [SerializeField] private LayerMask _layerMask;

    private RaycastHit[] _hitBuffer = new RaycastHit[5];

    Vector3 displacement;
    Vector3 currentPosition;
    Vector3 newPosition;

    private Animator animator;

    private int _isStopHash = Animator.StringToHash("IsStop");
    private List<LagCompensatedHit> hits = new();

    private float _safeTime = 0.5f;
    private float _spawnTime;


    private void Awake()
    {
        animator = GetComponent<Animator>();
        _spawnTime = Time.time;
    }

    public override void FixedUpdateNetwork()
    {
        if (!HasStateAuthority) return;



        if (_activeTimer.Expired(Runner))
        {
            RPC_Despawn();
            return;
        }

        if (_lifeTimer.Expired(Runner))
        {
            if (!GrenadeExplosion.gameObject.activeSelf)
            {
                RPC_Explode(transform.position);
                _lifeTimer = TickTimer.None;
            }
            return;
        }

        if (_isStopped) return;


        if (displacement.sqrMagnitude > 0.0001f)
        {
            Runner.LagCompensation.OverlapSphere(transform.position, _castRadius, Object.InputAuthority, hits);

            foreach (var hit in hits)
            {
                transform.position = hit.Point + Vector3.up * 0.01f;

                RPC_Explode(transform.position);
                _lifeTimer = TickTimer.None;
                return; 
            }
        }

        _time = Runner.DeltaTime;

        displacement = _velocity * Runner.DeltaTime + 0.5f * _gravity * _time * _time;
        currentPosition = transform.position;
        newPosition = currentPosition + displacement;
        //Vector3 direction = (newPosition - currentPosition).normalized;
        //float distance = Vector3.Distance(newPosition, currentPosition);

        _velocity += _gravity * Runner.DeltaTime;

        if (displacement.sqrMagnitude > 0.0001f)
        {
            Vector3 direction = displacement.normalized;
            float distance = displacement.magnitude;

            int hitCount = Physics.SphereCastNonAlloc(currentPosition, _castRadius, direction, _hitBuffer, distance, _layerMask, QueryTriggerInteraction.Collide);

            //if (Physics.SphereCast(transform.position, _castRadius, displacement.normalized, out RaycastHit hit, displacement.magnitude, layerMask))
            if (hitCount > 0)
            {
                RaycastHit closestHit = _hitBuffer[0];
                for (int i = 1; i < hitCount; i++)
                {
                    if (_hitBuffer[i].distance < closestHit.distance)
                    {
                        closestHit = _hitBuffer[i];
                    }
                }

                if (HandleCollision(closestHit)) return;
            }
        }

        transform.position = newPosition;
    }
    private bool HandleCollision(RaycastHit hit)
    {
        int hitLayer = hit.collider.gameObject.layer;

        // 폭발 체크
        if (hitLayer == 7)
        {
            if (Time.time - _spawnTime < _safeTime)
                return false;

            transform.position = hit.transform.root.position + new Vector3(0, 0.01f, 0);
            RPC_Explode(transform.position);
            _lifeTimer = TickTimer.None;
            return true;
        }
        else if (hitLayer == 11)
        {
            if (!_isBound)
            {
                _boundCnt++;
                _isBound = _boundCnt == 3? true : false;
                // 반사 벡터 계산
                _velocity = Vector3.Reflect(_velocity, hit.normal) * 0.6f;
                // 충돌 지점으로 위치 이동 (약간 띄워서 튕기기)
                transform.position = hit.point + Vector3.up * 0.01f;
                newPosition = hit.point + hit.normal * 0.01f + _velocity/8;
            }
            else
            {
                _isStopped = true;
                transform.position = hit.point + Vector3.up * 0.01f;
            }
        }
        else
        {
            // 반사 벡터 계산
            _velocity = Vector3.Reflect(_velocity, hit.normal) * 0.6f;
            // 충돌 지점으로 위치 이동 (약간 띄워서 튕기기)
            transform.position = hit.point + Vector3.up * 0.01f;
            newPosition = hit.point + hit.normal * 0.01f + _velocity;
        }

        return false;
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    private void RPC_Explode(Vector3 pos)
    {
        _isStopped = true;
        rendering.SetActive(false);
        transform.position = pos;
        explosionParticles.SetActive(true);
        GrenadeExplosion.gameObject.SetActive(true);
        GrenadeExplosion.Explosion();
        SetAnimation(true);
        _activeTimer = TickTimer.CreateFromSeconds(Runner,0.8f);
    }

    public void Init(Vector3 initialVelocity, Vector3 startPosition)
    {
        _lifeTimer = TickTimer.CreateFromSeconds(Runner, 3f);
        _velocity = initialVelocity;
        _time = 0f;
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    public void RPC_Despawn()
    {
        explosionParticles.SetActive(false);
        GrenadeExplosion.gameObject.SetActive(false);
        SetAnimation(false);
        _activeTimer = TickTimer.None;
        _isStopped = false;
        if (HasStateAuthority)
            Runner.Despawn(Object);

    }
    public void SetAnimation(bool Stop)
    {
        animator.SetBool(_isStopHash, Stop);
    }

    public void GetGrenade(Grenade grenade, Vector3 velocity)
    {
        obj = grenade;
        _velocity = velocity;
    }


}
