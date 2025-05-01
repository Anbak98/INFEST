using UnityEngine;

public class PJ_HI_II_Run : MonsterStateNetworkBehaviour
{
    Transform target;

    public override void Enter()
    {
        base.Enter();
        target = monster.target;
        monster.MovementSpeed = 5f;
    }

    public override void Execute()
    {
        base.Execute();

        // 아직 경로가 계산되지 않았거나 도착한 경우
        if (monster.AIPathing.enabled && !monster.AIPathing.pathPending)
        {
            monster.AIPathing.SetDestination(target.position);

            if (monster.AIPathing.remainingDistance <= monster.AIPathing.stoppingDistance)
            {
                phase.ChangeState<PJ_HI_II_Attack>();
            }
            //else if (monster.AIPathing.remainingDistance > 10f)
            //{
            //    monster.FSM.ChangePhase<PJ_HI_WonderPhase>();
            //}
        }
    }
}
