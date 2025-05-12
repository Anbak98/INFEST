using UnityEngine;

public class PJ_HI_II_RunWave : MonsterStateNetworkBehaviour<Monster_PJ_HI_II>
{
    public override void Enter()
    {
        base.Enter();
        monster.MovementSpeed = monster.info.SpeedMoveWave;

    }

    public override void Execute()
    {
        base.Execute();

        monster.AIPathing.SetDestination(monster.target.position);

        // 아직 경로가 계산되지 않았거나 도착한 경우
        if (monster.AIPathing.enabled && !monster.AIPathing.pathPending)
        {

            if (monster.AIPathing.remainingDistance <= monster.AIPathing.stoppingDistance)
            {
                Debug.Log(monster.target.position + "  " + transform.position);
                Debug.Log(monster.AIPathing.remainingDistance + "  " + monster.AIPathing.stoppingDistance);
                phase.ChangeState<PJ_HI_II_AttackWave>();
            }
            //else if (monster.AIPathing.remainingDistance > 10f)
            //{
            //    monster.FSM.ChangePhase<PJ_HI_WonderPhase>();
            //}
        }
    }

    public override void Exit()
    {
        base.Exit();
        monster.MovementSpeed = 0;

    }
}
