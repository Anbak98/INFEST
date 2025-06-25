using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadCop_Phase_Chase : MonsterPhase<Monster_DeadCop>
{
    [SerializeField] private int nextPatternIndex = 0;

    public TickTimer[] CoolDowns = new TickTimer[3];

    public override void MachineEnter()
    {
        base.MachineEnter();
        monster.animator.Play("Chase.DeadCop_Run");
        monster.PhaseIndex = 1;
        nextPatternIndex = 0;
        ChangeState<DeadCop_Chase_Run>(); // currentState를 강제로 1번 변경
    }


    public override void MachineExecute()
    {
        base.MachineExecute();
        /// target의 체력이 0이면 null로 만든다
        if (monster.IsTargetDead())
        {
            monster.TryRemoveTarget(monster.target);
            // 새로운 목표를 설정한다
            monster.SetTargetRandomly();
            // 몬스터 리스트에 플레이어가 있다면 타겟이 설정되고, 없으면 주변에 플레이어가 없으니 null이다
        }
        if (monster.target == null)
        {
            monster.FSM.ChangePhase<DeadCop_Phase_Wander>();
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
                    CaculateAttackType();
                }

                switch (nextPatternIndex)
                {
                    case 0:
                        ChangeState<DeadCop_Chase_Run>(); break;
                    case 1:
                        ChangeState<DeadCop_Chase_Punch>(); break;
                    case 2:
                        ChangeState<DeadCop_Chase_HeadButt>(); break;
                    case 3:
                        monster.FSM.ChangePhase<DeadCop_Phase_Wander>(); break;
                }
            }
        }
    }
    public void CaculateAttackType()
    {
        // 너무 멀거나 Wander로 돌아가야한다       
        //if (distance > 10f)
        //{
        //    /// Wander -> Idle
        //    monster.TryRemoveTarget(monster.target);    // Wander에서 이동할때는 target이 아니라 randomPosition으로 이동하니까 null문제 발생하지 않는다
        //    nextPatternIndex = 3;
        //    return;
        //}

        if (CoolDowns[2].ExpiredOrNotRunning(Runner))
        {
            if (monster.IsTargetInRange(monster.CommonSkillTable[2].UseRange))
            {
                nextPatternIndex = 2;
            }
            else
            {
                nextPatternIndex = 0;
            }
        }
        else if(CoolDowns[1].ExpiredOrNotRunning(Runner))
        {
            if (monster.IsTargetInRange(monster.CommonSkillTable[1].UseRange))
            {
                nextPatternIndex = 1;
            }
            else
            {
                nextPatternIndex = 0;
            }
        }
        else
        {
            // Run
            nextPatternIndex = 0;
        }
    }
}
