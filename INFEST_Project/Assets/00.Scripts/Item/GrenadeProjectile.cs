using Fusion;
using UnityEngine;

public class GrenadeProjectile : NetworkBehaviour
{
    [Networked] TickTimer _lifeTimer { get; set; }
    public GrenadeExplosion GrenadeExplosion;
    private float _time;
    private Vector3 _gravity = new Vector3(0, -9.81f, 0);
    public Transform throwPoint;
    private Vector3 _startPosition;
    private Vector3 _velocity;
    public GameObject obj;
    private float castRadius = 0.2f;

    private float _radius = 1f;

    private bool _isStopped = false;
    [SerializeField] private LayerMask aimLayerMask;


    public override void FixedUpdateNetwork()
    {
        if (_lifeTimer.Expired(Runner))
        {
            if (!GrenadeExplosion.gameObject.activeSelf)
            {
                GrenadeExplosion.gameObject.SetActive(true);
                Invoke("Despawn", 1f);
            }
            return;
        }

        if (_isStopped) return;

        _time += Time.deltaTime;

        Vector3 displacement = _velocity * _time + 0.5f * _gravity * _time * _time;
        transform.position = _startPosition + displacement;
      
        if (Physics.SphereCast(transform.position, castRadius, displacement.normalized, out RaycastHit hit, displacement.magnitude))
        {
            if (hit.collider.gameObject.layer == 11)
            {
                _isStopped = true;
                _gravity = _velocity = Vector3.zero; // 이동속도 초기화
                transform.position = hit.point + hit.normal * 0.01f; //지면 위에 위치 이동
            }
            else
            {
                // 반사 벡터 계산
                Vector3 reflected = Vector3.Reflect(_velocity.normalized, hit.normal);
                _velocity = reflected * _velocity.magnitude * 0.6f; // 약간 감쇠

                // 충돌 지점으로 위치 이동 (약간 띄워서 튕기기)
                transform.position = hit.point + hit.normal * 0.01f;
            }
            _startPosition = transform.position;
        }


    }


    public void Init(Vector3 initialVelocity, GameObject grenade)
    {
        _velocity = initialVelocity;
        _startPosition = transform.position;
        _time = 0f;
        obj = grenade;
    }

    public override void Spawned()
    {
        _lifeTimer = TickTimer.CreateFromSeconds(Runner, 5f);
    }

    public void Despawn()
    {
        if (Object == null) return;

        Runner.Despawn(Object);
    }

}
