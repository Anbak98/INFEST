using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class DeadCop_Phase_Wave : MonsterPhase<Monster_DeadCop>
{
    [SerializeField] private int patternCount = 0;
    [SerializeField] private int nextPatternIndex = 0;

    public override void MachineEnter()
    {
        base.MachineEnter();
        monster.animator.Play("Wave.DeadCop_Run");
        // 플레이어의 인식범위 늘린다
        monster.PlayerDetectorCollider.radius = monster.info.DetectAreaWave;

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
                        ChangeState<DeadCop_Wave_Run>(); break;
                    case 1:
                        ChangeState<DeadCop_Wave_HeadButt>(); break;
                    case 2:
                        ChangeState<DeadCop_Wave_Punch>(); break;
                    case 3:
                        monster.FSM.ChangePhase<DeadCop_Phase_Wander>(); break;
                }
            }
        }
    }

    public override void MachineExit()
    {
        base.MachineExit();
        monster.IsWave = false;
        // 플레이어의 인식범위 줄인다
        monster.PlayerDetectorCollider.radius = monster.info.DetectAreaNormal;
    }

    public void CaculateAttackType(float distance)
    {
        // 달릴때는 모든 상태로 변환이 가능하다
        // 느리면 Wander로 돌아가고, 
        // 빠른 경우에는 가까울 때 DropKick, 멀면 Punch 하면 된다
        if (distance <= 0.5)
        {
            // DropKick
            nextPatternIndex = 2;
        }
        else if (distance > 0.5f && distance < 1.0f)
        {
            // Punch
            nextPatternIndex = 1;
        }
        else if (distance > 1.0f && distance < 10f)
        {
            // Run
            nextPatternIndex = 0;
        }
        /// 아마 Wave 시간 간격 초과였던거 같은데...?
        else if (distance > 10f)
        {

            // Wander -> Idle
            nextPatternIndex = 3;
        }
    }
}
