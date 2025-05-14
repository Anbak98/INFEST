using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSM_WarZ : MonsterFSM<Monster_WarZ>
{
    private void OnTriggerEnter(UnityEngine.Collider other)
    {
        if (other.gameObject.layer == 7)
        {
            /// 테스트를 위해 MonsterNetworkBehaviour에 임시로 추가한 ClearTargetList함수
            /// private이라서 리스트를 지울 수가 없다
            // 몬스터 Spawner를 지워야한다
            //monster.ClearTargetList();

            /// 플레이어의 Detector가 PlayerMethodFromMonster를 가지고 있지 않아서 false
            /// 플레이어가 타겟으로 들어오지않는다
            monster.TryAddTarget(other.transform);

            monster.SetTargetRandomly();
            monster.FSM.ChangePhase<WarZ_Phase_Chase>();
        }
    }
}
