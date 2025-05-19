using Fusion;
using UnityEngine;

public class TargetableFromMonster : NetworkBehaviour
{
    [SerializeField] private PlayerStatHandler playerStatHandler;

    public int health;

    public virtual void ApplyDamage(MonsterNetworkBehaviour attacker, int damage)
    {
        playerStatHandler.TakeDamage(damage);
        health = playerStatHandler.CurHealth;

        if(playerStatHandler.CurHealth <= 0 )
        {
            attacker.TryRemoveTarget(transform);
        }
    }
}
