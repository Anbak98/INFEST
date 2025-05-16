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
        if (monster.target == null)
            monster.FSM.ChangePhase<WarZ_Phase_Wander>(); 

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
        // Run에서는 모든 상태로 변환이 가능하다
        // 타겟인 플레이어가 없거나(?) 애초에 타겟인 플레이어가 있어서 Chase에 들어온거니까

        // 너무 멀거나, 타겟의 체력이 0이거나(bool값으로 0처리) 타겟을 null로 하고 Wander로 돌아가야한다       
        if (distance > 10f /*|| 타겟의 체력 0 */)
        {
            /// Wander -> Idle
            monster.TryRemoveTarget(monster.target);    // Wander에서 이동할때는 target이 아니라 randomPosition으로 이동하니까 null문제 발생하지 않는다
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
            //ChangeState<WarZ_Chase_Run>();
            nextPatternIndex = 0;
        }

        // 타겟을 바꿔주는 로직
        // 타겟의 체력0 체크
        // 타겟에 다른 몬스터가 있다면
        // 없으면 알아서 idle이 되겠지
        //monster.SetTargetRandomly();

    }
}
