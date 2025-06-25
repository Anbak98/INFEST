using UnityEngine;
using UnityEngine.AI;

public class Monster_RageFang_Wonder_Walk : MonsterStateNetworkBehaviour<Monster_RageFang, Monster_RageFang_Phase_Wonder>
{
    Vector3 randomPosition;

    public override void Enter()
    {
        base.Enter();
        monster.CurMovementSpeed = monster.info.SpeedMove;
    }

    public override void Execute()
    {
        base.Execute();
        if (!monster.AIPathing.pathPending)
        {
            if (monster.MoveToRandomPositionAndCheck(5f, 10f, 10f))
            {
                phase.ChangeState<Monster_RageFang_Wonder_Idle>();
            }
        }
    }

    public override void Exit()
    {
        base.Exit();
        monster.CurMovementSpeed = 0;
    }
}
