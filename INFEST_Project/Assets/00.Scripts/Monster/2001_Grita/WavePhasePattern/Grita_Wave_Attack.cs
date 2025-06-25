using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grita_Wave_Attack : MonsterStateNetworkBehaviour<Monster_Grita, Grita_Phase_Wave>
{
    private TickTimer _tickTimer;

    public override void Enter()
    {
        base.Enter();

        if (monster.IsDead || monster.target == null)
            return;

        monster.CurMovementSpeed = 0f;
        monster.IsAttack = true;

        // 공격은 1번
        Debug.Log("Attack Wave");
        //monster.targetStatHandler = monster.target.GetComponentInParent<PlayerStatHandler>();
        //monster.targetStatHandler.TakeDamage(Random.Range(monster.info.MinAtk, monster.info.MaxAtk));

        _tickTimer = TickTimer.CreateFromSeconds(Runner, 2);
    }
    public override void Execute()
    {
        base.Execute();

        if (_tickTimer.Expired(Runner))
        {
           monster.MoveToTarget();
            monster.IsAttack = false;
            if (!monster.AIPathing.pathPending && !monster.IsDead)
            {
                if (monster.IsTargetInRange(1f))
                {
                    phase.ChangeState<Grita_Wave_Attack>();
                }
                else
                {
                    phase.ChangeState<Grita_Wave_Run>();
                }
            }
        }

    }

    public override void Exit()
    {
        base.Exit();

    }

}
