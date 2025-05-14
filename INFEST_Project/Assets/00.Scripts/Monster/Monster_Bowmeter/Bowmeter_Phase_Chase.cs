using System.Collections.Generic;
using Fusion;
using Unity.VisualScripting;
using UnityEngine;

public class Bowmeter_Phase_Chase : MonsterPhase<Monster_Bowmeter>
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
        monster.AIPathing.SetDestination(monster.target.position);

        if (!monster.AIPathing.pathPending)
        {
            if (monster.IsReadyForChangingState)
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
    }

    public void CaculateAttackType(float distance)
    {
        //List<int> activatedSkills = new();
        Debug.Log($"[AttackType] Dist: {distance}, Cool2: {skillCoolDown[2].Expired(Runner)}, Cool3: {skillCoolDown[3].Expired(Runner)}");
        activatedSkilles = new();

        for (int i = 1; i < skillCoolDown.Length; ++i)
        {
            if (skillCoolDown[i].ExpiredOrNotRunning(Runner)) 
            { 
                if(distance <= 3f)
                {
                    activatedSkilles.Add(i);
                }
                else if (distance <= 5f)
                {
                    if(i != 1)
                        activatedSkilles.Add(i);
                }
            }
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
