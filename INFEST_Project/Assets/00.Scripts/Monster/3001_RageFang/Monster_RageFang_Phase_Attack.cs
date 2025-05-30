using Fusion;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Monster_RageFang_Phase_Attack  : MonsterPhase<Monster_RageFang>
{
    public TickTimer[] skillCoolDown = new TickTimer[10];
    public TickTimer patternTickTimer;
    [SerializeField] private int patternCount = 0;
    [SerializeField] private int nextPatternIndex = 0;
    [SerializeField] private List<int> activatedSkills;

    public override void MachineEnter()
    {
        base.MachineEnter();
        monster.IsValidRetreat = true;
        monster.RetreatTimer = TickTimer.CreateFromSeconds(Runner, 120f);
        monster.PhaseIndex = 1;

        for (int i = 0; i < skillCoolDown.Length; i++)
        {
            skillCoolDown[i] = TickTimer.CreateFromSeconds(Runner, 0);
        }
        patternTickTimer = TickTimer.CreateFromSeconds(Runner, 0);
        monster.IsPhaseAttack = true;
    }

    public override void MachineExecute()
    {
        base.MachineExecute();

       monster.MoveToTarget();

        if (!monster.AIPathing.pathPending)
        {
            if (monster.IsReadyForChangingState)
            {
                CaculateAttackType(monster.AIPathing.remainingDistance);

                switch (nextPatternIndex)
                {
                    case 0:
                        ChangeState<Monster_RageFang_Attack_Run>(); break;
                    case 1:
                        ChangeState<Monster_RageFang_Attack_RightPunch>(); break;
                    case 2:
                        ChangeState<Monster_RageFang_Attack_LeftSwip>(); break;
                    case 3:
                        ChangeState<Monster_RageFang_Attack_Jumping>(); break;
                    case 4:
                        ChangeState<Monster_RageFang_Attack_FlexingMuscles>(); break;
                    case 5:
                        ChangeState<Monster_RageFang_Attack_Rush>(); break;
                    case 6:
                        ChangeState<Monster_RageFang_Attack_JumpAttack>(); break;
                    case 7:
                        ChangeState<Monster_RageFang_Attack_ContinuousPunch>(); break;
                    case 8:
                        ChangeState<Monster_RageFang_Attack_QuickRollToRUN>(); break;
                    case 9:
                        ChangeState<Monster_RageFang_Attack_TurnAttack>(); break;
                }
            }
        }
    }

    public override void MachineExit()
    {
        base.MachineExit();
        monster.IsPhaseAttack = false;
    }

    public void CaculateAttackType(float distance)
    {
        activatedSkills = new();
        int totalProbability = 0;

        if (distance > 15)
        {
            monster.IsReadyForChangingState = true;
            nextPatternIndex = 0;
            return;
        }

        for (int i = 1; i < skillCoolDown.Length; ++i)
        {
            if (skillCoolDown[i].Expired(Runner))
            {
                int thr = 0;
                int fiv = 0;
                int sev = 0;
                switch (monster.regionIndex)
                {
                    case 2:
                        thr = monster.skills[i].RF_1P3M;
                        fiv = monster.skills[i].RF_1P5M;
                        sev = monster.skills[i].RF_1P7M;
                        break;
                    case 3:
                        thr = monster.skills[i].RF_2P3M;
                        fiv = monster.skills[i].RF_2P5M;
                        sev = monster.skills[i].RF_2P7M;
                        break;
                    case 4:
                        thr = monster.skills[i].RF_3P3M;
                        fiv = monster.skills[i].RF_3P5M;
                        sev = monster.skills[i].RF_3P7M;
                        break;
                    case 5:
                        thr = monster.skills[i].RF_4P3M;
                        fiv = monster.skills[i].RF_4P5M;
                        sev = monster.skills[i].RF_4P7M;
                        break;
                }

                if (distance <= 3 && thr > 0)
                {
                    totalProbability += thr;
                    activatedSkills.Add(i);
                }
                else if (distance <= 5 && fiv > 0)
                {
                    totalProbability += fiv;
                    activatedSkills.Add(i);
                }
                else if (distance <= 7 && sev> 0)
                {
                    totalProbability += sev;
                    activatedSkills.Add(i);
                }
            }
        }


        if (activatedSkills.Count == 0)
        {
            if (distance <= 5)
            {
                nextPatternIndex = 1;
            }
            else
            {
                ChangeState<Monster_RageFang_Attack_Run>();
                nextPatternIndex = 0;
            }

            return;
        }

        int rand = UnityEngine.Random.Range(0, totalProbability);
        int temp = 0;

        foreach (var index in activatedSkills)
        {
            int thr = 0;
            int fiv = 0;
            int sev = 0;
            switch (monster.regionIndex)
            {
                case 2:
                    thr = monster.skills[index].RF_1P3M;
                    fiv = monster.skills[index].RF_1P5M;
                    sev = monster.skills[index].RF_1P7M;
                    break;               
                case 3:                  
                    thr = monster.skills[index].RF_2P3M;
                    fiv = monster.skills[index].RF_2P5M;
                    sev = monster.skills[index].RF_2P7M;
                    break;               
                case 4:                  
                    thr = monster.skills[index].RF_3P3M;
                    fiv = monster.skills[index].RF_3P5M;
                    sev = monster.skills[index].RF_3P7M;
                    break;               
                case 5:                  
                    thr = monster.skills[index].RF_4P3M;
                    fiv = monster.skills[index].RF_4P5M;
                    sev = monster.skills[index].RF_4P7M;
                    break;
            }

            if (distance <= 3)
            {
                temp += thr;
            }
            else if (distance <= 5)
            {
                temp += fiv;
            }
            else if (distance <= 7)
            {
                temp += sev;
            }

            if (rand < temp)
            {
                nextPatternIndex = index;

                patternCount++;

                if (patternCount > 1)
                {
                    monster.SetTargetRandomly();
                }

                return;
            }
        }

        ChangeState<Monster_RageFang_Attack_Run>();
    }
}
