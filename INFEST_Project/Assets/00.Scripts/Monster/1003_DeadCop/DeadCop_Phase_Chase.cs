using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadCop_Phase_Chase : MonsterPhase<Monster_DeadCop>
{
    public override void MachineEnter()
    {
        base.MachineEnter();
        monster.IsReadyForChangingState = false;

    }


    public override void MachineExecute()
    {
        base.MachineExecute();

        // 기본상태인 Run으로 들어가서 Run에서 실행하니까 굳이 필요하지 않을 것 같다

        //monster.AIPathing.SetDestination(monster.target.position);

        //// 생성되자마자 공격되는거 방지
        //if (!monster.AIPathing.pathPending)
        //{
        //    // 애니메이션이 끝나고 나서 상태 전환을 한다
        //    if (monster.IsReadyForChangingState)
        //    {
        //        float distance = monster.AIPathing.remainingDistance;
        //        monster.transform.forward = monster.target.position - monster.transform.position;

        //        if (distance <= 0.5f)
        //        {
        //            // HeadButt
        //            ChangeState<DeadCop_Attack_2_HeadButt>();
        //        }
        //        else if (distance > 0.5f && distance < 1.0f)
        //        {
        //            // normal
        //            ChangeState<DeadCop_Attack_1_Normal>();
        //        }
        //    }
        //}
    }
}
