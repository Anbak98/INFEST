using System.Collections.Generic;
using Fusion;
using UnityEngine;

public class Rocket : NetworkBehaviour
{
    public Weapon weapon;
    public GameObject explosion;
    public GameObject render;
    TickTimer _explosionTime;
    [SerializeField] private int _playerLayer = 7;
    [SerializeField] private int _monsterLayer = 14;
    [SerializeField] private LayerMask _layerMask = 1 << 12 | 1<< 16 | 1<< 10;

    private float _castRadius = 0.2f;
    private int _damage;
    private Player _player;
    private Vector3 displacement;
    private Vector3 newPosition;
    private RaycastHit[] _hitBuffer = new RaycastHit[5];

    private void Start()
    {
        if (!Object.HasStateAuthority) return;
        _damage = weapon.instance.data.Atk;
        _explosionTime = TickTimer.CreateFromSeconds(Runner, 8f);
    }

    public override void FixedUpdateNetwork()
    {
        if (!HasStateAuthority) return;

        if (_explosionTime.Expired(Runner))
        {
            if (!explosion.activeSelf)
            {
                RPC_Explode(transform.position);
                _explosionTime = TickTimer.None;
            }
            return;
        }

        if (displacement.sqrMagnitude > 0.0001f)
        {
            if (Runner.LagCompensation.Raycast(transform.position, displacement.normalized, 10f,
                    Object.InputAuthority, out var hits))
            {
                //transform.position = hits.GameObject.transform.root.position + new Vector3(0, 0.01f, 0);

                RPC_Explode(transform.position);
                _explosionTime = TickTimer.None;
                return;
            }
        }

        displacement = transform.forward.normalized * 20f * Runner.DeltaTime;
        newPosition = displacement + transform.position;
        if (displacement.sqrMagnitude > 0.0001f)
        {
            Vector3 direction = displacement.normalized;
            float distance = displacement.magnitude;

            int layerMask = ~_layerMask;
            int hitCount = Physics.SphereCastNonAlloc(transform.position, _castRadius, direction, _hitBuffer, distance, layerMask, QueryTriggerInteraction.Ignore);

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

                //transform.position = closestHit.transform.position + new Vector3(0, 0.01f, 0); ;
                RPC_Explode(transform.position);
                _explosionTime = TickTimer.None;
                return;
            }
        }

        transform.position = newPosition;
    }



    public void Explosion()
    {
        if (!HasStateAuthority) return;

        Invoke(nameof(Despawn), 0.8f);

        UnityEngine.Collider[] colliders = Physics.OverlapSphere(transform.position, weapon.instance.data.Splash, 1 << _playerLayer);

        foreach (UnityEngine.Collider other in colliders)
        {
            Player _otherplayer = other.GetComponentInParent<Player>();
            _otherplayer.statHandler.TakeDamage(null, _damage/4);
        }

        List<LagCompensatedHit> hits = new List<LagCompensatedHit>();
        if (hits != null)
        {
            Runner.LagCompensation.OverlapSphere(
            origin: transform.position,
            radius: weapon.instance.data.Splash,
            hits: hits,
            layerMask: 1 << _monsterLayer,
            queryTriggerInteraction: QueryTriggerInteraction.Ignore,
            player: Object.StateAuthority
        );

            foreach (var hit in hits)
            {
                if (hit.Hitbox != null && hit.GameObject.layer == _monsterLayer)
                {
                    var _monster = hit.Hitbox.Root.GetComponent<MonsterNetworkBehaviour>();
                    if (_monster != null)
                    {
                        ApplyDamage(_monster, transform.position, (transform.position - _monster.transform.position).normalized);
                    }
                }
            }
        }
    }

    private void ApplyDamage(MonsterNetworkBehaviour _monster, Vector3 pos, Vector3 dir)
    {
        if (_monster == null) return;

        if (_monster.CurHealth == 0 || _monster.IsDead == true) return;

        if (_monster.ApplyDamage(Runner.LocalPlayer, _damage, Vector3.zero, Vector3.zero, 0, false) == false)
            return;

    }

    private void Despawn()
    {
        if (Object == null) return;

        Runner.Despawn(Object);
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    private void RPC_Explode(Vector3 pos)
    {
        transform.position = pos;
        explosion.SetActive(true);
        render.SetActive(false);
        Explosion();
    }


}
