using UnityEngine;
using UnityEngine.AI;

public class Monster_RageFang_Attack_Run : MonsterStateNetworkBehaviour<Monster_RageFang, Monster_RageFang_Phase_Attack>
{
    public override void Enter()
    {
        base.Enter();
        monster.CurMovementSpeed = monster.info.SpeedMoveWave;
    }

    public override void Execute()
    {
        base.Execute();
    }

    public override void Exit()
    {
        base.Exit();
    }
}
