public class Stacker_Phase_Chase : MonsterPhase<Monster_Stacker>
{
    public override void MachineEnter()
    {
        base.MachineEnter();
        monster.CurDetectorRadius = monster.info.DetectAreaWave;
        monster.AIPathing.speed = monster.info.SpeedMoveWave;

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
            monster.FSM.ChangePhase<Stacker_Phase_Wonder>();
            return;
        }

        if (monster.IsReadyForChangingState)
        {
           monster.MoveToTarget();
        }

        if (!monster.AIPathing.pathPending)
        {
            if (monster.IsReadyForChangingState)
            {
                if (monster.AIPathing.remainingDistance <= monster.commonSkill[1].UseRange)
                {
                    ChangeState<Stacker_Attack>();
                }
                else
                {
                    ChangeState<Stacker_Run>();
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
