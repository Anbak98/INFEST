using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSM_WarZ : MonsterFSM<Monster_WarZ>
{
    private void OnTriggerEnter(UnityEngine.Collider other)
    {
        if (other.gameObject.layer == 7)
        {
            // 타겟의 체력이 0이 아니라면 추가, 0이면 추가하지 않는다면?           
            // target이 null이라도 wander의 walk에서는 randomPosition으로 SetDestination하니까 괜찮다

            monster.TryAddTarget(other.transform);

            monster.SetTargetRandomly();
            monster.FSM.ChangePhase<WarZ_Phase_Chase>();
        }
    }
}
