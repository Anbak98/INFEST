using Fusion;
using UnityEngine;

public class GrenadeProjectile : NetworkBehaviour
{
    [Networked] TickTimer _lifeTimer { get; set; }
    public GrenadeExplosion GrenadeExplosion;
    public GameObject explosionParticles;
    private float _time;
    private Vector3 _gravity = new Vector3(0, -9.81f, 0);
    public Transform throwPoint;
    private Vector3 _startPosition;
    private Vector3 _velocity;
    public Grenade obj;

    private float _castRadius = 0.2f;

    private float _radius = 1f;

    private bool _isStopped = false;
    private bool _oneBound = false;
    [SerializeField] private LayerMask _layerMask = 1 << 12;
    

    private RaycastHit[] _hitBuffer = new RaycastHit[5];

    Vector3 displacement;
    Vector3 currentPosition;
    Vector3 newPosition;

    private Animator animator;

    private int _isStopHash = Animator.StringToHash("IsStop");

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public override void FixedUpdateNetwork()
    {
        if (_lifeTimer.Expired(Runner))
        {
            if (!GrenadeExplosion.gameObject.activeSelf)
            {
                Explode();
            }
            return;
        }

        if (_isStopped) return;

        _time = Runner.DeltaTime;

        displacement = _velocity * Runner.DeltaTime  + 0.5f * _gravity * _time * _time;
        currentPosition = transform.position;
        newPosition = currentPosition + displacement;
        //Vector3 direction = (newPosition - currentPosition).normalized;
        //float distance = Vector3.Distance(newPosition, currentPosition);

        _velocity += _gravity * Runner.DeltaTime;

        if (displacement.sqrMagnitude > 0.0001f)
        {
            Vector3 direction = displacement.normalized;
            float distance = displacement.magnitude;

            int layerMask = ~_layerMask;
            int hitCount = Physics.SphereCastNonAlloc(currentPosition, _castRadius, direction, _hitBuffer, distance, layerMask, QueryTriggerInteraction.Ignore);

            //if (Physics.SphereCast(transform.position, _castRadius, displacement.normalized, out RaycastHit hit, displacement.magnitude, layerMask))
            if (hitCount > 0)
            {
                RaycastHit closestHit = _hitBuffer[0];
                Debug.Log("진입 전 : " + closestHit);
                for (int i = 1; i < hitCount; i++)
                {
                    if (_hitBuffer[i].distance < closestHit.distance)
                    {
                        closestHit = _hitBuffer[i];
                        Debug.Log("진입 후 변경 : " + closestHit.collider.gameObject.name);

                    }
                }

                HandleCollision(closestHit);
            }
        }

        if (displacement.sqrMagnitude > 0.0001f)
        {
            if (Runner.LagCompensation.Raycast(transform.position, displacement.normalized, 0.2f,
                    Object.InputAuthority, out var hits))
            {
                Explode();
            }
        }

        transform.position = newPosition;
    }
    private void HandleCollision(RaycastHit hit)
    {


        int hitLayer = hit.collider.gameObject.layer;

        // 폭발 체크
        if (hitLayer == 6)
        {
            Explode();
        }

        if (hitLayer == 11)
        {
            _isStopped = true;
            transform.position = hit.point + hit.normal * 0.01f;  // 지면 위에 위치 이동
        }
        else if (hitLayer == 10)
        {
            //transform.position = hit.point;  // 벽에 부딪히지 않고, 계속 이동
        }
        else
        {
            // 반사 벡터 계산
            //Vector3 reflected = Vector3.Reflect(_velocity.normalized, hit.normal);
            //_velocity = (reflected + hit.normal * 0.2f).normalized * _velocity.magnitude * 0.6f;
            _velocity = Vector3.Reflect(_velocity, hit.normal) * 0.6f;
            // 충돌 지점으로 위치 이동 (약간 띄워서 튕기기)
            transform.position = hit.point + hit.normal * 0.01f;
            newPosition = hit.point + hit.normal * 0.01f + _velocity;
        }
    }


    private void Explode()                  
    {
        explosionParticles.SetActive(true);
        GrenadeExplosion.gameObject.SetActive(true);
        GrenadeExplosion.Explosion();
        _lifeTimer = TickTimer.None;
        StopAnimation();
        Invoke(nameof(Despawn), 1f);
    }

    public void Init(Vector3 initialVelocity, Grenade grenade, Vector3 startPosition)
    {
        _lifeTimer = TickTimer.CreateFromSeconds(Runner, 2f);
        _velocity = initialVelocity;
        transform.position = startPosition;
        _time = 0f;
        obj = grenade;
    }

    //public override void Spawned()
    //{
    //    _lifeTimer = TickTimer.CreateFromSeconds(Runner, 2f);
    //}

    public void Despawn()
    {
        if (Object == null) return;

        Runner.Despawn(Object);
    }

    public void StopAnimation()
    {
        animator.SetBool(_isStopHash, true);
    }

}
