using System.Collections.Generic;
using Fusion;
using Unity.VisualScripting;
using UnityEngine;

public class Bowmeter_Phase_Chase : MonsterPhase<Monster_Bowmeter>
{
    public TickTimer[] skillCoolDown = new TickTimer[4];
    public TickTimer patternTickTimer;
    public int nextPatternIndex = 0;

    public override void MachineEnter()
    {
        base.MachineEnter();

        for (int i = 0; i < skillCoolDown.Length; i++)
        {
            skillCoolDown[i] = TickTimer.CreateFromSeconds(Runner, 0);
        }
        patternTickTimer = TickTimer.CreateFromSeconds(Runner, 0);
    }

    public override void MachineExecute()
    {
        base.MachineExecute();
        monster.AIPathing.SetDestination(monster.target.position);

        if (patternTickTimer.Expired(Runner))
        {
            CaculateAttackType(monster.AIPathing.remainingDistance);

            switch (nextPatternIndex)
            {
                case 0:
                    ChangeState<Bowmeter_Run>(); break;
                case 1:
                    ChangeState<Bowmeter_Pattern1>(); break;
                case 2:
                    ChangeState<Bowmeter_Pattern2>(); break;
                case 3:
                    ChangeState<Bowmeter_Pattern3>(); break;
            }
        }
    }

    public void CaculateAttackType(float distance)
    {
        //List<int> activatedSkills = new();
        Debug.Log($"[AttackType] Dist: {distance}, Cool2: {skillCoolDown[2].Expired(Runner)}, Cool3: {skillCoolDown[3].Expired(Runner)}");

        if (distance <= 5f)
        {
            if (skillCoolDown[2].Expired(Runner))
            {
                Debug.Log("패턴2 선택");
                skillCoolDown[2] = TickTimer.CreateFromSeconds(Runner, 7f);
                nextPatternIndex = 2;
            }
            else if (skillCoolDown[3].Expired(Runner))
            {
                Debug.Log("패턴3 선택");
                skillCoolDown[3] = TickTimer.CreateFromSeconds(Runner, 10f);
                nextPatternIndex = 3;
            }
            else if (distance <= 3f)
            {
                Debug.Log("패턴1 선택");
                nextPatternIndex = 1;
            }
        }
        else
        {
            nextPatternIndex = 0; // Run
        }

        patternTickTimer = TickTimer.CreateFromSeconds(Runner, 3f);

        //for (int i = 1; i < skillCoolDown.Length; ++i)
        //{
        //    if (skillCoolDown[i].Expired(Runner))
        //    {
        //        if (distance <= 3)
        //        {
        //            activatedSkills.Add(i);
        //        }
        //        else if (distance <= 5)
        //        {
        //            activatedSkills.Add(i);
        //        }
        //    }
        //}

        //Debug.Log(activatedSkills.Count);

        //if (activatedSkills.Count == 0)
        //{
        //    Debug.Log(distance);
        //    if (distance <= 5)
        //    {
        //        nextPatternIndex = 1;
        //    }
        //    else
        //    {
        //        nextPatternIndex = 0;
        //    }

        //    return;
        //}        

        //ChangeState<Bowmeter_Run>();
    }
}
