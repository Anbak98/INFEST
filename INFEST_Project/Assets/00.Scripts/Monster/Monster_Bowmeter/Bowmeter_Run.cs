using UnityEngine;

public class Bowmeter_Run : MonsterStateNetworkBehaviour<Monster_Bowmeter, Bowmeter_Phase_Chase>
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

        if(!monster.AIPathing.pathPending && monster.AIPathing.remainingDistance <= 5f)
        {
            monster.IsReadyForChangingState = true;
        }
        //float distance = monster.AIPathing.remainingDistance;

        //if (distance <= 5f)
        //{
        //    // Chase phase의 공격 계산 함수 호출
        //    phase.CaculateAttackType(distance);

        //    switch (phase.nextPatternIndex)
        //    {
        //        case 0:
        //            phase.ChangeState<Bowmeter_Run>(); break;
        //        case 1:
        //            phase.ChangeState<Bowmeter_Pattern1>(); break;
        //        case 2:
        //            phase.ChangeState<Bowmeter_Pattern2>(); break;
        //        case 3:
        //            phase.ChangeState<Bowmeter_Pattern3>(); break;
        //    }
        //}
    }

    public override void Exit()
    {
        base.Exit();
        monster.IsRun = false;
    }
}
