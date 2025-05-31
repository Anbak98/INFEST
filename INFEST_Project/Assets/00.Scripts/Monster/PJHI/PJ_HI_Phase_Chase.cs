
using UnityEngine;
using UnityEngine.AI;

public class PJ_HI_Phase_Chase : MonsterPhase<Monster_PJ_HI>
{
    public override void MachineEnter()
    {
        base.MachineEnter();
        monster.CurDetectorRadius = monster.info.DetectAreaWave;
        monster.CurMovementSpeed = monster.info.SpeedMoveWave;

        monster.IsReadyForChangingState = true;
        monster.IsChasePhase = true;
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
            monster.FSM.ChangePhase<PJ_HI_Phase_Wonder>();
            return;
        }

       monster.MoveToTarget();

        if(!monster.AIPathing.pathPending)
        {
            if (monster.IsReadyForChangingState)
            {
                if (monster.IsTargetInRange(monster.CommonSkillTable[1].UseRange))
                {
                    ChangeState<PJ_HI_Attack>();
                }
                else
                {
                    ChangeState<PJ_HI_Run>();
                }
            }
        }
    }

    public override void MachineExit()
    {
        base.MachineExit();
        monster.IsChasePhase = false;
    }
}
