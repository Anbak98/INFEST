using UnityEngine;
using UnityEngine.AI;

public class Monster_RageFang_AttackTwo_Run : MonsterStateNetworkBehaviour<Monster_RageFang, Monster_RageFang_Phase_AttackTwo>
{
    public override void Enter()
    {
        base.Enter();
        monster.CurMovementSpeed = monster.info.SpeedMoveWave;
    }

    public override void Execute()
    {
        base.Execute();
        monster.AIPathing.SetDestination(monster.target.position);
    }

    public override void Exit()
    {
        base.Exit();
    }
}
