using Fusion;
using UnityEngine;

public class GrenadeExplosion : MonoBehaviour
{
    public GrenadeProjectile grenadeProjectile;

    private int _playerLayer = 7;
    private int _monsterLayer = 12;

    private int _damage;
    private Player _player;

    public void Awake()
    {
        _damage = grenadeProjectile.obj.GetComponent<Grenade>().instance.data.Effect;
        _player = grenadeProjectile.obj.GetComponent<Grenade>().throwPoint.GetComponentInParent<Player>();
    }

    private void OnTriggerEnter(UnityEngine.Collider other)
    {
        if (other.gameObject.layer == _playerLayer)
        {
            Player _otherplayer = other.GetComponentInParent<Player>();

            _otherplayer.statHandler.TakeDamage(_damage/2);

        }

        else if (other.gameObject.layer == _monsterLayer)
        {
            MonsterNetworkBehaviour _monster = other.GetComponentInParent<MonsterNetworkBehaviour>();
            Debug.Log("몬스터 이름" + _monster);
            ApplyDamage(_monster, transform.position, (transform.position - _monster.transform.position).normalized);

        } 
    }

    private void ApplyDamage(MonsterNetworkBehaviour monster, Vector3 pos, Vector3 dir)
    {
        if (monster == null) return;

        if (monster.CurrentHealth == 0 || monster.IsDead == true) return;

        if (monster.ApplyDamage(_player.Runner.LocalPlayer, _damage, pos, dir, 0, false) == false)
            return;

    }
}