using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarZ_Phase_Chase : MonsterPhase<Monster_WarZ>
{
    [SerializeField] private int nextPatternIndex = 0;

    public override void MachineEnter()
    {
        base.MachineEnter();

        monster.animator.Play("Chase.WarZ_Run");
        monster.PhaseIndex = 1;
        nextPatternIndex = 0;
        ChangeState<WarZ_Chase_Run>(); // currentState를 강제로 1번 변경
    }


    public override void MachineExecute()
    {
        base.MachineExecute();
        if (monster.IsTargetDead())   
        {
            monster.target = null;
            // 새로운 목표를 설정한다
            monster.SetTargetRandomly();
            // 몬스터 리스트에 플레이어가 있다면 타겟이 설정되고, 없으면 주변에 플레이어가 없으니 null이다
        }
        if (monster.target == null)
        {
            monster.FSM.ChangePhase<WarZ_Phase_Wander>();
            return;
        }

       monster.MoveToTarget();

        // 생성되자마자 공격되는거 방지
        if (!monster.AIPathing.pathPending)
        {
            // 애니메이션이 끝나고 나서 상태 전환을 한다
            {
                if (monster.IsReadyForChangingState)
                {
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
    }

    public void CaculateAttackType(float distance)
    {

        // 가까울 때 DropKick, 멀면 Punch 하면 된다
        if (distance <= 1.5)
        {
            // DropKick
            nextPatternIndex = 2;
            return;
        }
        else if (distance < 2.0f)
        {
            // Punch
            nextPatternIndex = 1;
            return;
        }
        else
        {
            // Run
            nextPatternIndex = 0;
        }
    }
}
