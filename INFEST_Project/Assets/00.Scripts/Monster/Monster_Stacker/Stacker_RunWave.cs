using UnityEngine;

public class Stacker_RunWave : MonsterStateNetworkBehaviour<Monster_Stacker, Stacker_Phase_Wave>
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
                phase.ChangeState<Stacker_AttackWave>();
            }
            //else if (monster.AIPathing.remainingDistance > 10f)
            //{
            //    monster.FSM.ChangePhase<Stacker_Phase_Wonder>();
            //}
        }
    }

    public override void Exit()
    {
        base.Exit();
        monster.MovementSpeed = 0;
    }
}
