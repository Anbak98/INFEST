using UnityEngine;

public class Stacker_RunWave : MonsterStateNetworkBehaviour<Monster_Stacker, Stacker_Phase_Wave>
{
    public override void Enter()
    {
        base.Enter();
        monster.CurMovementSpeed = monster.info.SpeedMoveWave;
    }

    public override void Execute()
    {
        base.Execute();

       monster.MoveToTarget();

        // 아직 경로가 계산되지 않았거나 도착한 경우
        if (monster.AIPathing.enabled && !monster.AIPathing.pathPending)
        {

            if (monster.IsTargetInRange(1f))
            {
                phase.ChangeState<Stacker_AttackWave>();
            }
            //else if (monster.IsTargetInRange() > 10f)
            //{
            //    monster.FSM.ChangePhase<Stacker_Phase_Wonder>();
            //}
        }
    }

    public override void Exit()
    {
        base.Exit();
        monster.CurMovementSpeed = 0;
    }
}
