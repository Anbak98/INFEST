using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadCop_Wave_Run : MonsterStateNetworkBehaviour<Monster_DeadCop, DeadCop_Phase_Wave>
{
    Transform _target;

    public override void Enter()
    {
        base.Enter();
        monster.CurMovementSpeed = monster.info.SpeedMoveWave;
    }

    public override void Execute()
    {
        base.Execute();

        monster.AIPathing.SetDestination(monster.target.position);

        // 아직 경로가 계산되지 않았거나 도착한 경우
        if (monster.AIPathing.enabled && !monster.AIPathing.pathPending)
        {
            monster.AIPathing.SetDestination(_target.position);

            if (monster.AIPathing.remainingDistance <= 0.5f)
            {
                phase.ChangeState<DeadCop_Wave_HeadButt>();
            }
            else if (monster.AIPathing.remainingDistance > 0.5f && monster.AIPathing.remainingDistance < 1.0f)
            {
                phase.ChangeState<DeadCop_Wave_Punch>();
            }
            // wave 해제조건은 공통으로 몬스터 숫자를 다 잡으면 IsWave를 false로 만들 것이다
            // FSM_DeadCop에서 작성
        }
    }

    public override void Exit()
    {
        base.Exit();
        monster.CurMovementSpeed = 0;
    }
}
