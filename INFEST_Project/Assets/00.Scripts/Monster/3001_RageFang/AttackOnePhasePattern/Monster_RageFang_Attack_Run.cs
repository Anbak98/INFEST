using UnityEngine;
using UnityEngine.AI;

public class Monster_RageFang_Attack_Run : MonsterStateNetworkBehaviour<Monster_RageFang, Monster_RageFang_Phase_AttackOne>
{
    public override void Enter()
    {
        base.Enter();
        monster.MovementSpeed = monster.info.SpeedMoveWave;
    }

    public override void Execute()
    {
        base.Execute();
        Debug.Log(monster.target);
        monster.AIPathing.SetDestination(monster.target.position);
    }

    public override void Exit()
    {
        base.Exit();
    }
}
