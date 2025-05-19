using System.Collections.Generic;
using Fusion;
using UnityEngine;

public class Rocket : NetworkBehaviour
{
    public Weapon weapon;
    public GameObject explosion;
    public GameObject render;
    TickTimer _explosionTime;
    [SerializeField] private int _playerLayer = 6;
    [SerializeField] private int _monsterLayer = 14;

    private int _damage;
    private Player _player;

    private void Awake()
    {
        if (!Object.HasStateAuthority) return;
        _damage = weapon.instance.data.Atk;
        _explosionTime = TickTimer.CreateFromSeconds(Runner, 8f);
    }

    private void Update()
    {
        if (_explosionTime.ExpiredOrNotRunning(Runner))
            Explosion();
    }

    public void Explosion()
    {
        if (!HasStateAuthority) return; 

        Invoke(nameof(Despawn), 0.8f);

        UnityEngine.Collider[] colliders = Physics.OverlapSphere(transform.position, weapon.instance.data.Splash, 1 << _playerLayer);

        foreach (UnityEngine.Collider other in colliders)
        {
            Player _otherplayer = other.GetComponentInParent<Player>();
            _otherplayer.statHandler.TakeDamage(_damage);
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
}
