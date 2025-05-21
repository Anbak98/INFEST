using Fusion;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Monster_RageFang_Phase_AttackOne : MonsterPhase<Monster_RageFang>
{
    public TickTimer[] skillCoolDown = new TickTimer[6];
    public TickTimer patternTickTimer;
    [SerializeField] private int patternCount = 0;
    [SerializeField] private int nextPatternIndex = 0;
    [SerializeField] private List<int> activatedSkills;

    public bool IsFlexingMuscles = false;
    public TickTimer flexingMusclesBuffTimer;

    public int beforeDamage;
    public int beforeDef;

    public override void MachineEnter()
    {
        base.MachineEnter();
        monster.PhaseIndex = 1;

        for (int i = 0; i < skillCoolDown.Length; i++)
        {
            skillCoolDown[i] = TickTimer.CreateFromSeconds(Runner, 0);
        }
        patternTickTimer = TickTimer.CreateFromSeconds(Runner, 0);
        monster.IsPhaseAttackOne = true;
    }

    public override void MachineExecute()
    {
        base.MachineExecute();
        monster.AIPathing.SetDestination(monster.target.position);

        if(monster.CurHealth < monster.BaseHealth / 2)
        {
            monster.FSM.ChangePhase<Monster_RageFang_Phase_AttackTwo>();    
        }

        if(!flexingMusclesBuffTimer.ExpiredOrNotRunning(Runner))
        {
            if(!IsFlexingMuscles)
            {
                beforeDamage = monster.CurDamage;
                beforeDef = monster.CurDef;
                monster.CurDamage += (int)(monster.CurDamage / monster.skills[4].DamageCoefficient);
                monster.CurDef += (int)(monster.CurDef / monster.skills[4].DamageCoefficient);
                IsFlexingMuscles = true;
            }
        }
        else
        {
            if(IsFlexingMuscles)
            {
                monster.CurDamage = beforeDamage;
                monster.CurDef = beforeDef;
                IsFlexingMuscles = false;
            }
        }

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
                }
            }
        }
    }

    public override void MachineExit()
    {
        base.MachineExit();
        monster.IsPhaseAttackOne = false;
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
                if (distance <= 5 && monster.skills[i].Distance_1P1M > 0)
                {
                    totalProbability += monster.skills[i].Distance_1P1M;
                    activatedSkills.Add(i);
                }
                else if (distance <= 10 && monster.skills[i].Distance_1P3M > 0)
                {
                    totalProbability += monster.skills[i].Distance_1P3M;
                    activatedSkills.Add(i);
                }
                else if (distance <= 15 && monster.skills[i].Distance_1P5M > 0)
                {
                    totalProbability += monster.skills[i].Distance_1P5M;
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
            if (distance <= 5)
            {
                temp += monster.skills[index].Distance_1P1M;
            }
            else if (distance <= 10)
            {
                temp += monster.skills[index].Distance_1P3M;
            }
            else if (distance <= 15)
            {
                temp += monster.skills[index].Distance_1P5M;
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
