using UnityEngine;

public class Stacker_Run : MonsterStateNetworkBehaviour<Monster_Stacker, Stacker_Phase_Chase>
{
    //Transform _target;

    public override void Enter()
    {
        base.Enter();
        monster.IsRun = true;
        //_target = monster.target;
        monster.CurMovementSpeed = monster.info.SpeedMoveWave;
    }

    public override void Execute()
    {
        base.Execute();

        if (!monster.AIPathing.pathPending && monster.AIPathing.remainingDistance <= 5f)
        {
            monster.IsReadyForChangingState = true;
        }

        // 아직 경로가 계산되지 않았거나 도착한 경우
        //if (monster.AIPathing.enabled && !monster.AIPathing.pathPending)
        //{
        //    monster.AIPathing.SetDestination(_target.position);

        //    if (monster.AIPathing.remainingDistance <= monster.AIPathing.stoppingDistance)
        //    {
        //        monster.IsReadyForChangingState = true;
        //        //phase.ChangeState<Stacker_Attack>();
        //    }
        //}
    }

    public override void Exit()
    {
        base.Exit();
        monster.IsRun = false;
        //monster.CurMovementSpeed = 0;
    }
}
