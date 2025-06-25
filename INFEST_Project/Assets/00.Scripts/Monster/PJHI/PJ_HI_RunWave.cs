using UnityEngine;

public class PJ_HI_RunWave : MonsterStateNetworkBehaviour<Monster_PJ_HI, PJ_HI_Phase_Wave>
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

            //if (monster.IsTargetInRange() <= monster.AIPathing.stoppingDistance)
            //{
            //    Debug.Log(monster.target.position + "  " + transform.position);
            //    Debug.Log(monster.IsTargetInRange() + "  " + monster.AIPathing.stoppingDistance);
            //    phase.ChangeState<PJ_HI_AttackWave>();
            //}
            //else if (monster.IsTargetInRange() > 10f)
            //{
            //    monster.FSM.ChangePhase<PJ_HI_WonderPhase>();
            //}
        }
    }

    public override void Exit()
    {
        base.Exit();
        monster.CurMovementSpeed = 0;

    }
}