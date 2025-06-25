using System.Collections.Generic;
using Fusion;
using UnityEngine;

public class GrenadeProjectile : NetworkBehaviour
{
    public Rigidbody rb;
    TickTimer _activeTimer;
    TickTimer _lifeTimer;
    public GrenadeExplosion GrenadeExplosion;
    public GameObject explosionParticles;
    public Grenade obj;
    public GameObject rendering;
    private float _castRadius = 0.2f;

    private float _boundCnt = 0;
    private bool _isBound = false;
    private bool _isStopped = false;
    [SerializeField] private LayerMask _layerMask;

    private Animator animator;

    private int _isStopHash = Animator.StringToHash("IsStop");
    private List<LagCompensatedHit> hits = new();

    private void Awake()
    {
        animator = GetComponent<Animator>();
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
                RPC_Explode();
                _lifeTimer = TickTimer.None;
            }
            return;
        }

        if (_isStopped) return;

        Runner.LagCompensation.OverlapSphere(transform.position, _castRadius, Object.InputAuthority, hits);

        foreach (var hit in hits)
        {
            RPC_Explode();
            _lifeTimer = TickTimer.None;
            return;
        }
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    private void RPC_Explode()
    {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.useGravity = false;
        _isStopped = true;
        rendering.SetActive(false);
        explosionParticles.SetActive(true);
        GrenadeExplosion.gameObject.SetActive(true);
        GrenadeExplosion.Explosion();
        SetAnimation(true);
        _activeTimer = TickTimer.CreateFromSeconds(Runner, 0.8f);
    }

    public void Init(Vector3 velocity)
    {
        _lifeTimer = TickTimer.CreateFromSeconds(Runner, 3f);
        rb.AddForce(velocity, ForceMode.VelocityChange);
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

    public void GetGrenade(Grenade grenade)
    {
        obj = grenade;
    }

    private void OnCollisionEnter(Collision collision)
    {
        int hitLayer = collision.gameObject.layer;

        // Æø¹ß Ã¼Å©
        if (hitLayer == 6)
        {
            RPC_Explode();
            _lifeTimer = TickTimer.None;
        }
        else if (hitLayer == 11)
        {
            if (!_isBound)
            {
                _boundCnt++;
                _isBound = _boundCnt == 3 ? true : false;
            }
            else
            {
                _isStopped = true;
                rb.angularVelocity = Vector3.zero;
                rb.velocity = Vector3.zero;
            }
        }
    }
}
