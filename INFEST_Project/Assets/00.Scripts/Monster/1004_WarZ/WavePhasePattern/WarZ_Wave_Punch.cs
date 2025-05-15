using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarZ_Wave_Punch : MonsterStateNetworkBehaviour<Monster_WarZ, WarZ_Phase_Wave>
{
    private TickTimer _tickTimer;

    public override void Enter()
    {
        base.Enter();

        if (monster.IsDead || monster.target == null)
            return;

        monster.CurMovementSpeed = 0f;
        monster.IsAttack = true;

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
            if (!monster.AIPathing.pathPending && !monster.IsDead)
            {
                if (monster.AIPathing.remainingDistance > 0.5f && monster.AIPathing.remainingDistance < 1.0f)
                {
                    phase.ChangeState<WarZ_Wave_Punch>();
                }
                else
                {
                    monster.IsAttack = false;
                    phase.ChangeState<WarZ_Wave_Run>();
                }
            }
        }
    }
    public override void Exit()
    {
        base.Exit();
    }
}
