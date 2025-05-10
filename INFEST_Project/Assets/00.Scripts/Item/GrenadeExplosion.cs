using System.Collections.Generic;
using Cinemachine;
using Fusion;
using FuzzySharp.Utils;
using UnityEngine;

public class GrenadeExplosion : MonoBehaviour
{
    public GrenadeProjectile grenadeProjectile;

    [SerializeField] private LayerMask _playerLayer = 7;
    [SerializeField] private LayerMask _monsterLayer = 12;

    private int _damage;
    private Player _player;

    public void Awake()
    {
        _damage = grenadeProjectile.obj.GetComponent<Grenade>().instance.data.Effect;
        _player = grenadeProjectile.obj.GetComponent<Grenade>().throwPoint.GetComponentInParent<Player>();
    }

    public void Explosion()
    {
        int layerMask = (1 << _playerLayer);

        UnityEngine.Collider[] colliders = Physics.OverlapSphere(transform.position, 5f, layerMask);

        foreach (UnityEngine.Collider other in colliders)
        {
            if (other.gameObject.layer == _playerLayer)
            {
                Player _otherplayer = other.GetComponentInParent<Player>();

                _otherplayer.statHandler.TakeDamage(_damage / 100);

            }

            //if (other.gameObject.layer == _monsterLayer)
            //{
            //    MonsterNetworkBehaviour _monster = other.GetComponentInParent<MonsterNetworkBehaviour>();
            //    Debug.Log("몬스터 이름" + _monster);
            //    ApplyDamage(_monster, transform.position, (transform.position - _monster.transform.position).normalized);

            //}
        }
        List<LagCompensatedHit> hits = new List<LagCompensatedHit>();

        _player.Runner.LagCompensation.OverlapSphere(
            origin: transform.position,
            radius: 5f,
            hits: hits,
            layerMask: -1,
            queryTriggerInteraction: QueryTriggerInteraction.Ignore,
            player: _player.Runner.LocalPlayer
        );

        foreach (var hit in hits)
        {
            if (hit.Hitbox != null && hit.Hitbox.name == "mixamorig5:Head")
            {
                var _monster = hit.Hitbox.Root.GetComponent<MonsterNetworkBehaviour>();
                if (_monster != null)
                {
                    ApplyDamage(_monster, transform.position, (transform.position - _monster.transform.position).normalized);
                    Debug.Log($"몬스터 {_monster.name} 에게 피해");
                }
            }
        }
    }

    private void ApplyDamage(MonsterNetworkBehaviour _monster, Vector3 pos, Vector3 dir)
    {
        if (_monster == null) return;

        if (_monster.CurrentHealth == 0 || _monster.IsDead == true) return;

        if (_monster.ApplyDamage(_player.Runner.LocalPlayer, _damage, pos, dir, 0, false) == false)
            return;

    }
}