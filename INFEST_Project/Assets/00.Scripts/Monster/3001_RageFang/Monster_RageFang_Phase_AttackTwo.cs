using Fusion;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Monster_RageFang_Phase_AttackTwo : MonsterPhase<Monster_RageFang>
{
    public TickTimer[] skillCoolDown = new TickTimer[10];
    public int nextPatternIndex = 0;
    [SerializeField] private List<int> activatedSkills;
    [SerializeField] private List<int> activatedSkillsPro;
    [SerializeField] private int patternCount = 0;

    public override void MachineEnter()
    {
        base.MachineEnter();
        monster.CurMovementSpeed = 0;
        monster.IsReadyForChangingState = false;

        monster.IsRoaring = true;

        monster.CurDamage += monster.CurDamage / 20;
        monster.CurDef += monster.CurDef / 20;
    }

    public override void MachineExecute()
    {
        base.MachineExecute();
        monster.AIPathing.SetDestination(monster.target.position);

        if (!monster.AIPathing.pathPending)
        {
            if (monster.IsReadyForChangingState)
            {
                CaculateAttackType(monster.AIPathing.remainingDistance);
                monster.transform.forward = monster.target.position - monster.transform.position;

                switch (nextPatternIndex)
                {
                    case 0:
                        ChangeState<Monster_RageFang_AttackTwo_Run>(); break;
                    case 1:
                        ChangeState<Monster_RageFang_AttackTwo_RightPunch>(); break;
                    case 2:
                        ChangeState<Monster_RageFang_AttackTwo_LeftSwip>(); break;
                    case 3:
                        ChangeState<Monster_RageFang_AttackTwo_Jumping>(); break;
                    case 4:
                        ChangeState<Monster_RageFang_AttackTwo_FlexingMuscles>(); break;
                    case 5:
                        ChangeState<Monster_RageFang_AttackTwo_Rush>(); break;
                    case 6:
                        ChangeState<Monster_RageFang_AttackTwo_JumpAttack>(); break;
                    case 7:
                        ChangeState<Monster_RageFang_AttackTwo_ContinuousPunch>(); break;
                    case 8:
                        ChangeState<Monster_RageFang_AttackTwo_QuickRollToRUN>(); break;
                    case 9:
                        ChangeState<Monster_RageFang_AttackTwo_TurnAttack>(); break;
                }
            }
        }
    }


    public void CaculateAttackType(float distance)
    {
        activatedSkills = new();
        activatedSkillsPro = new();
        int totalProbability = 0;

        if (distance > 15)
        {
            monster.IsReadyForChangingState = true;
            nextPatternIndex = 0;
            return;
        }

        for (int i = 1; i < skillCoolDown.Length; ++i)
        {
            if (skillCoolDown[i].ExpiredOrNotRunning(Runner))
            {
                if (distance <= 5 && monster.skills[i].Distance_2P1M > 0)
                {
                    totalProbability += monster.skills[i].Distance_2P1M;
                    activatedSkills.Add(i);
                    activatedSkillsPro.Add(monster.skills[i].Distance_2P1M);
                }
                else if (distance <= 10 && monster.skills[i].Distance_2P3M > 0)
                {
                    totalProbability += monster.skills[i].Distance_2P3M;
                    activatedSkills.Add(i);
                    activatedSkillsPro.Add(monster.skills[i].Distance_2P3M);
                }
                else if (distance <= 15 && monster.skills[i].Distance_2P5M > 0)
                {
                    totalProbability += monster.skills[i].Distance_2P5M;
                    activatedSkills.Add(i);
                    activatedSkillsPro.Add(monster.skills[i].Distance_2P5M);
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
                ChangeState<Monster_RageFang_AttackTwo_Run>();
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
                temp += monster.skills[index].Distance_2P1M;
            }
            else if (distance <= 10)
            {
                temp += monster.skills[index].Distance_2P3M;
            }
            else if (distance <= 15)
            {
                temp += monster.skills[index].Distance_2P5M;
            }

            if (rand < temp)
            {
                nextPatternIndex = index;

                patternCount++;

                if (patternCount > 1)
                {
                    monster.SetTargetRandomly();
                    patternCount = 0;
                }

                return;
            }
        }

        ChangeState<Monster_RageFang_AttackTwo_Run>();
    }
}
