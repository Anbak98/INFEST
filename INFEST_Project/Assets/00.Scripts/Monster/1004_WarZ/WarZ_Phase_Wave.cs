using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarZ_Phase_Wave : MonsterPhase<Monster_WarZ>
{
    [SerializeField] private int patternCount = 0;
    [SerializeField] private int nextPatternIndex = 0;

    public override void MachineEnter()
    {
        base.MachineEnter();
        monster.animator.Play("Wave.WarZ_Run");
        monster.PhaseIndex = 1;
        nextPatternIndex = 0;
        ChangeState<WarZ_Wave_Run>(); // currentState를 강제로 1번 변경

        // 플레이어의 인식범위 늘린다
        monster.PlayerDetectorCollider.radius = monster.info.DetectAreaWave;
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
                    CaculateAttackType(monster.AIPathing.remainingDistance);

                switch (nextPatternIndex)
                {
                    case 0:
                        ChangeState<WarZ_Wave_Run>(); break;
                    case 1:
                        ChangeState<WarZ_Wave_DropKick>(); break;
                    case 2:
                        ChangeState<WarZ_Wave_Punch>(); break;
                    case 3:
                        monster.FSM.ChangePhase<WarZ_Phase_Wander>(); break;
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
        // 너무 멀거나 Wander로 돌아가야한다       
        if (distance > 10f)
        {
            /// Wander -> Idle
            monster.TryRemoveTarget(monster.target);    // Wander에서 이동할때는 target이 아니라 randomPosition으로 이동하니까 null문제 발생하지 않는다
            nextPatternIndex = 3;
            return;
        }

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
        /// Wave 시간 간격 초과하면 원래 상태로 돌아간다
    }
}
