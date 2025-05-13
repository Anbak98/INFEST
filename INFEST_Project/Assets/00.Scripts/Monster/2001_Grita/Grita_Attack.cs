using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grita_Attack : MonsterStateNetworkBehaviour<Monster_Grita, Grita_Phase_Chase>
{
    private TickTimer _tickTimer;

    public override void Enter()
    {
        base.Enter();

        if (monster.IsDead || monster.target == null)
            return;
        monster.CurMovementSpeed = 0f;
        monster.IsAttack = true;

        Debug.Log("Attack");
        //monster.targetStatHandler = monster.target.GetComponentInParent<PlayerStatHandler>();
        //monster.targetStatHandler.TakeDamage(Random.Range(monster.info.MinAtk, monster.info.MaxAtk));
        _tickTimer = TickTimer.CreateFromSeconds(Runner, 2);

    }
    public override void Execute()
    {
        base.Execute();
        if (_tickTimer.Expired(Runner))
        {
            monster.AIPathing.SetDestination(monster.target.position);
            monster.IsAttack = false;
            if (!monster.AIPathing.pathPending && !monster.IsDead)
            {
                if (monster.AIPathing.remainingDistance <= monster.AIPathing.stoppingDistance)
                {
                    phase.ChangeState<Grita_Attack>();
                }
                else if (monster.AIPathing.remainingDistance > monster.AIPathing.stoppingDistance)
                {
                    phase.ChangeState<Grita_Run>();
                }
            }
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}
