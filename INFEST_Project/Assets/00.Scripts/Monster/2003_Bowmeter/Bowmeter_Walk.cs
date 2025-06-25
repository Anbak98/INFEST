using UnityEngine;
using UnityEngine.AI;

public class Bowmeter_Walk : MonsterStateNetworkBehaviour<Monster_Bowmeter, Bowmeter_Phase_Wonder>
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
        base.Execute();// ���� ��ΰ� ������ �ʾҰų� ������ ���
        if (!monster.AIPathing.pathPending)
        {
            if (monster.MoveToRandomPositionAndCheck(5, 10, 10))
            {
                phase.ChangeState<Bowmeter_Idle>();
            }
        }
    }

    public override void Exit()
    {
        base.Exit();
        monster.IsWalk = false;
    }
}
