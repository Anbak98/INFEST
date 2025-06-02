using UnityEngine;
using UnityEngine.AI;

public class Stacker_Walk : MonsterStateNetworkBehaviour<Monster_Stacker, Stacker_Phase_Wonder>
{
    Vector3 randomPosition;

    public override void Enter()
    {
        base.Enter();
        monster.IsWalk = true;
        monster.CurMovementSpeed = monster.info.SpeedMove;
    }

    public override void Execute()
    {
        base.Execute();// 아직 경로가 계산되지 않았거나 도착한 경우
        if (!monster.AIPathing.pathPending)
        {
            if (monster.MoveToRandomPositionAndCheck(5, 10, 10))
            {
                phase.ChangeState<Stacker_Idle>();
            }
        }
    }

    public override void Exit()
    {
        base.Exit();
        monster.IsWalk = false;
    }
}
