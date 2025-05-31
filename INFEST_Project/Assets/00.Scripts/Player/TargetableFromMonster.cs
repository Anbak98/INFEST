using Fusion;
using INFEST.Game;
using UnityEngine;

public class TargetableFromMonster : NetworkBehaviour
{
    [SerializeField] private PlayerStatHandler playerStatHandler;
    [SerializeField] private Player player;

    public IState CurState => player.controller.stateMachine.currentState;

    public int CurHealth = 9999;

    public virtual void ApplyDamage(MonsterNetworkBehaviour attacker, int damage)
    {
        playerStatHandler.TakeDamage(attacker, damage);
        CurHealth = playerStatHandler.CurHealth;
    }
}
