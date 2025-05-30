using Fusion;
using System.Collections.Generic;
using UnityEngine;

public class GoreHaul_Phase_Chase : MonsterPhase<Monster_GoreHaul>
{
    public TickTimer[] skillCoolDown = new TickTimer[4];
    [ReadOnly, SerializeField] private List<int> activatedSkilles;
    public int nextPatternIndex = 0;

    public override void MachineEnter()
    {
        base.MachineEnter();

        for (int i = 0; i < skillCoolDown.Length; i++)
        {
            skillCoolDown[i] = TickTimer.CreateFromSeconds(Runner, 0);
        }
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
            monster.FSM.ChangePhase<GoreHaul_Phase_Wonder>();
            return;
        }

        if (monster.IsReadyForChangingState && monster.target != null)
        {
           monster.MoveToTarget();
        }

        if (!monster.AIPathing.pathPending)
        {
            if (monster.IsReadyForChangingState)
            {
                CaculateAttackType(monster.AIPathing.remainingDistance);

                switch (nextPatternIndex)
                {
                    case 0:
                        ChangeState<GoreHaul_Run>(); break;
                    case 1:
                        ChangeState<GoreHaul_Punch>(); break;
                    case 2:
                        ChangeState<GoreHaul_LowAttack>(); break;
                    case 3:
                        ChangeState<GoreHaul_JumpAttack>(); break;
                }
            }
        }
    }

    public void CaculateAttackType(float distance)
    {
        activatedSkilles = new();
        monster.TrySetTarget(monster.target.transform);

        bool pattern1Ready = skillCoolDown[1].ExpiredOrNotRunning(Runner);

        for (int i = 2; i < skillCoolDown.Length; ++i)
        {
            if (skillCoolDown[i].ExpiredOrNotRunning(Runner))
            {
                if (distance <= monster.skills[2].UseRange)
                {
                    activatedSkilles.Add(i);
                }
            }
        }

        if (activatedSkilles.Count == 0 && distance <= monster.skills[1].UseRange && pattern1Ready)
        {
            activatedSkilles.Add(1);
        }

        if (activatedSkilles.Count > 0)
        {
            monster.IsReadyForChangingState = false;
            nextPatternIndex = activatedSkilles[Random.Range(0, activatedSkilles.Count)];
        }
        else
        {
            nextPatternIndex = 0;
        }
    }
}
