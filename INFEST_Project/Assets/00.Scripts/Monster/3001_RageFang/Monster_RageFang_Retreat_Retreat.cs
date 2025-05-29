using Fusion;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class Monster_RageFang_Retreat_Retreat : MonsterStateNetworkBehaviour<Monster_RageFang, Monster_RageFang_Phase_Retreat>
{

    public override void Enter()
    {
        base.Enter();
    }

    public override void Execute()
    {
        base.Execute();

        //if (Vector3.Distance(phase.t.position, phase.targetPosition) > 0.1f)
        //{
        //    phase.t.position = Vector3.MoveTowards(phase.t.position, phase.targetPosition, phase.speed * Time.deltaTime);
        //    if (!phase.FirstJump && Physics.Raycast(transform.position + Vector3.up * 0.5f, transform.forward, 5, phase.obstacleLayer))
        //    {
        //        phase.FirstJump = true;
        //        phase.ChangeState<Monster_RageFang_Retreat_Jump>();
        //    }
        //}
        //else
        //{
            phase.t.position = phase.targetPosition; // 정확한 위치 보정
            monster.ClearTargetList();
            monster.FSM.ChangePhase<Monster_RageFang_Phase_Wonder>();
        //}
    }

    public override void Exit()
    {
        base.Exit();
    }
}
