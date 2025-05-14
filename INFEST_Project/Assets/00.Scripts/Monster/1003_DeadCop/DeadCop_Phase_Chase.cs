using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadCop_Phase_Chase : MonsterPhase<Monster_DeadCop>
{
    public override void MachineEnter()
    {
        base.MachineEnter();
        //monster.IsReadyForChangingState = false;

        monster.animator.Play("Chase.DeadCop_Run");

    }


    public override void MachineExecute()
    {
        base.MachineExecute();


        monster.AIPathing.SetDestination(monster.target.position);

        // 생성되자마자 공격되는거 방지
        if (!monster.AIPathing.pathPending)
        {
            // 애니메이션이 끝나고 나서 상태 전환을 한다
            if (monster.IsReadyForChangingState)
            {
                float distance = monster.AIPathing.remainingDistance;
                monster.transform.forward = monster.target.position - monster.transform.position;

                if (distance <= 0.5f)
                {
                    // HeadButt
                    ChangeState<DeadCop_Chase_Attack_2_HeadButt>();
                }
                else if (distance > 0.5f && distance < 1.0f)
                {
                    // normal
                    ChangeState<DeadCop_Chase_Attack_1_Normal>();
                }
            }
        }
    }
}
