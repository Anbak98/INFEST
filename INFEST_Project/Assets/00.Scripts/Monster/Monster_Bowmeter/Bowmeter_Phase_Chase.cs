using System.Collections.Generic;
using Fusion;

public class Bowmeter_Phase_Chase : MonsterPhase<Monster_Bowmeter>
{
    public TickTimer[] skillCoolDown = new TickTimer[3];
    public TickTimer patternTickTimer;
    private int patternCount = 0;
    private int nextPatternIndex = 0;

    //public override void MachineEnter()
    //{
    //    base.MachineEnter();
    //    monster.PhaseIndex = 1;

    //    for (int i = 0; i < skillCoolDown.Length; i++)
    //    {
    //        skillCoolDown[i] = TickTimer.CreateFromSeconds(Runner, 0);
    //    }
    //    patternTickTimer = TickTimer.CreateFromSeconds(Runner, 0);
    //}

    //public override void MachineExecute()
    //{
    //    base.MachineExecute();
    //    monster.AIPathing.SetDestination(monster.target.position);

    //    if (patternTickTimer.Expired(Runner))
    //    {
    //        CaculateAttackType(monster.AIPathing.remainingDistance);

    //        switch (nextPatternIndex)
    //        {
    //            case 0:
    //                ChangeState<Monster_RageFang_Attack_Run>(); break;
    //            case 1:
    //                ChangeState<Monster_RageFang_Attack_RightPunch>(); break;
    //            case 2:
    //                ChangeState<Monster_RageFang_Attack_LeftSwip>(); break;
    //            case 3:
    //                ChangeState<Monster_RageFang_Attack_Jumping>(); break;
    //            case 4:
    //                ChangeState<Monster_RageFang_Attack_FlexingMuscles>(); break;
    //            case 5:
    //                ChangeState<Monster_RageFang_Attack_Rush>(); break;
    //        }
    //    }
    //}

    //public void CaculateAttackType(float distance)
    //{
    //    List<int> activatedSkills = new();
    //    int totalProbability = 0;

    //    if (distance > 15)
    //    {
    //        ChangeState<Monster_RageFang_Attack_Run>();
    //        return;
    //    }

    //    for (int i = 1; i < skillCoolDown.Length; ++i)
    //    {
    //        if (skillCoolDown[i].Expired(Runner))
    //        {
    //            if (distance <= 5 && monster.skills[i].Distance_1P1M > 0)
    //            {
    //                totalProbability += monster.skills[i].Distance_1P1M;
    //                activatedSkills.Add(i);
    //            }
    //            else if (distance <= 10 && monster.skills[i].Distance_1P3M > 0)
    //            {
    //                totalProbability += monster.skills[i].Distance_1P3M;
    //                activatedSkills.Add(i);
    //            }
    //            else if (distance <= 15 && monster.skills[i].Distance_1P3M > 0)
    //            {
    //                totalProbability += monster.skills[i].Distance_1P5M;
    //                activatedSkills.Add(i);
    //            }
    //        }
    //    }

    //    Debug.Log(activatedSkills.Count);

    //    if (activatedSkills.Count == 0)
    //    {
    //        Debug.Log(distance);
    //        if (distance <= 5)
    //        {
    //            nextPatternIndex = 1;
    //        }
    //        else
    //        {
    //            nextPatternIndex = 0;
    //        }

    //        return;
    //    }

    //    int rand = UnityEngine.Random.Range(0, totalProbability);
    //    int temp = 0;

    //    foreach (var index in activatedSkills)
    //    {
    //        if (distance <= 5)
    //        {
    //            temp += monster.skills[index].Distance_1P1M;
    //        }
    //        else if (distance <= 10)
    //        {
    //            temp += monster.skills[index].Distance_1P3M;
    //        }
    //        else if (distance <= 15)
    //        {
    //            temp += monster.skills[index].Distance_1P5M;
    //        }

    //        if (rand < temp)
    //        {
    //            Debug.Log("Activate " + index + " " + monster.skills[index].Name);
    //            nextPatternIndex = index;

    //            patternCount++;

    //            if (patternCount > 1)
    //            {
    //                monster.SetTargetRandomly();
    //            }

    //            return;
    //        }
    //    }

    //    ChangeState<Monster_RageFang_Attack_Run>();
    //}
}
