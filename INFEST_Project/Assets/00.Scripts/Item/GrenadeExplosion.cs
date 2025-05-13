using System.Collections.Generic;
using Fusion;
using UnityEngine;

public class GrenadeExplosion : NetworkBehaviour
{
    public GrenadeProjectile grenadeProjectile;

    [SerializeField] private int _playerLayer = 6;
    [SerializeField] private int _monsterLayer = 14;


    private int _damage;
    private Player _player;

    private int _ice = 10702;
    private int _emp = 10703;
    private float _iceDebuff = 0.55f;
    private float _debuffTime = 5f;

    private void Awake()
    {
        if (!Object.HasStateAuthority) return;
        _damage = grenadeProjectile.obj.instance.data.Effect/100;
        _player = grenadeProjectile.obj.GetComponent<Grenade>()._player;
    }
    public void Explosion()
    {
        if (!Object.HasStateAuthority) return;

        int layerMask = 1 << _playerLayer;

        UnityEngine.Collider[] colliders = Physics.OverlapSphere(transform.position, 5f, layerMask);

        foreach (UnityEngine.Collider other in colliders)
        {
                Player _otherplayer = other.GetComponentInParent<Player>();
                _otherplayer.TakeDamage(_damage);
        }

        List<LagCompensatedHit> hits = new List<LagCompensatedHit>();
        if( hits != null)
        {
            _player.Runner.LagCompensation.OverlapSphere(
            origin: transform.position,
            radius: 5f,
            hits: hits,
            layerMask: 1 << _monsterLayer,
            queryTriggerInteraction: QueryTriggerInteraction.Ignore,
            player: _player.Object.StateAuthority
        );

            foreach (var hit in hits)
            {
                if (hit.Hitbox != null && hit.GameObject.layer == _monsterLayer)
                {
                    var _monster = hit.Hitbox.Root.GetComponent<MonsterNetworkBehaviour>();
                    if (_monster != null)
                    {
                        ApplyDamage(_monster, transform.position, (transform.position - _monster.transform.position).normalized);
                        if (grenadeProjectile.obj?.key == _ice)
                            FreezEeffect(_monster);
                        if (grenadeProjectile.obj?.key == _emp)
                            EmpEeffect(_monster);
                    }
                }
            }
        }
        
    }

    private void ApplyDamage(MonsterNetworkBehaviour _monster, Vector3 pos, Vector3 dir)
    {
        if (_monster == null) return;

        if (_monster.CurrentHealth == 0 || _monster.IsDead == true) return;

        if (_monster.ApplyDamage(_player.Runner.LocalPlayer, _damage, Vector3.zero, Vector3.zero, 0, false) == false)
            return;

    }

    private void FreezEeffect(MonsterNetworkBehaviour _monster)
    {
        //if(_monster is Monster_PJ_HI pj)
        //{
        //    pj.FSM.ChangePhase<PJ_HI_Phase_Wonder>();
        //    pj.FSM.ChangeState<PJ_HI_Idle>();

        //    pj.FSM.ChangePhase<PJ_HI_Phase_Chase>();
        //    pj.FSM.ChangeState<PJ_HI_Run>();
        //}
        StartCoroutine(_monster.Slow(0, _debuffTime));

    }

    private void EmpEeffect(MonsterNetworkBehaviour _monster)
    {
        // _monster.FSM.ChangeState<PJ_HI_Idle>();
        StartCoroutine(_monster.Slow(_monster.MovementSpeed * _iceDebuff, _debuffTime));

    }

}