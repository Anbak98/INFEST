using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarZ_Phase_Chase : MonsterPhase<Monster_WarZ>
{
    [SerializeField] private int nextPatternIndex = 0;

    public override void MachineEnter()
    {
        base.MachineEnter();
        //monster.IsReadyForChangingState = false;
        monster.PhaseIndex = 1;

        Debug.Log("Chase");
        monster.animator.Play("Chase.WarZ_Run");
    }


    public override void MachineExecute()
    {
        base.MachineExecute();
        monster.AIPathing.SetDestination(monster.target.position);

        // 생성되자마자 공격되는거 방지
        if (!monster.AIPathing.pathPending)
        {
            // 애니메이션이 끝나고 나서 상태 전환을 한다
            {
                if (monster.IsReadyForChangingState)
                    CaculateAttackType(monster.AIPathing.remainingDistance);

                switch (nextPatternIndex)
                {
                    case 0:
                        ChangeState<WarZ_Chase_Run>(); break;
                    case 1:
                        ChangeState<WarZ_Chase_DropKick>(); break;
                    case 2:
                        ChangeState<WarZ_Chase_Punch>(); break;
                    case 3:
                        monster.FSM.ChangePhase<WarZ_Phase_Wander>(); break;
                }
            }
        }
    }

    public void CaculateAttackType(float distance)
    {
        // 달릴때는 모든 상태로 변환이 가능하다
        // 너무 멀거나 느리면 Wander로 돌아가고
        if (distance > 10f /*|| monster.CurMovementSpeed <= 1*/)
        {
            /// Wander -> Idle
            nextPatternIndex = 3;
            return;
        }

        // 가까울 때 DropKick, 멀면 Punch 하면 된다
        if (distance <= 0.5)
        {
            // DropKick
            nextPatternIndex = 2;
            return;
        }
        else if (distance > 0.5f && distance < 1.0f)
        {
            // Punch
            nextPatternIndex = 1;
            return;
        }
        else
        {
            // Run
            nextPatternIndex = 0;
            ChangeState<WarZ_Chase_Run>();
        }
    }
}
