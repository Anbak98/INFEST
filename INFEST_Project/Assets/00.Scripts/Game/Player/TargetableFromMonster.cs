using Fusion;
using UnityEngine;

public class TargetableFromMonster : NetworkBehaviour
{
    [SerializeField] private PlayerStatHandler playerStatHandler;

    public int CurHealth = 9999;

    public virtual void ApplyDamage(MonsterNetworkBehaviour attacker, int damage)
    {
        playerStatHandler.TakeDamage(damage);
        CurHealth = playerStatHandler.CurHealth;

        if(playerStatHandler.CurHealth <= 0 )
        {
            attacker.TryRemoveTarget(transform);
        }
    }
}
