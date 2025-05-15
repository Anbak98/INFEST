using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarZ_Chase_DropKick : MonsterStateNetworkBehaviour<Monster_WarZ, WarZ_Phase_Chase>
{
    public override void Enter()
    {
        base.Enter();

        monster.CurMovementSpeed = 0f;
        monster.IsDropKick = true;

        // 애니메이션이 끝나기 전에는 상태가 안바뀐다
        monster.IsReadyForChangingState = false;
    }

    //public override void Execute()
    //{
    //    base.Execute();

    //    //monster.AIPathing.SetDestination(monster.target.position);
    //    //if (monster.animTickTimer.Expired(Runner))
    //    //{
    //    //    if (!monster.AIPathing.pathPending /*&& !monster.IsDead*/)
    //    //    {
    //    //        if (monster.AIPathing.remainingDistance <= 0.5f)
    //    //        {
    //    //            phase.ChangeState<WarZ_Chase_DropKick>();
    //    //        }
    //    //        else
    //    //        {
    //    //            monster.IsAttack = false;
    //    //            phase.ChangeState<WarZ_Chase_Run>();
    //    //        }
    //    //    }
    //    //}
    //}

    public override void Exit()
    {
        base.Exit();
        monster.IsDropKick = false;
    }

    public override void Attack()
    {
        base.Attack();
        monster.TryAttackTarget((int)(monster.CurDamage /** monster.skills[1].DamageCoefficient*/));
    }

}
