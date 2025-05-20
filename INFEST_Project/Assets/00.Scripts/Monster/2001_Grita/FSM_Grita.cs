using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSM_Grita : MonsterFSM<Monster_Grita>
{
    [SerializeField] private int patternCount = 0;
    [SerializeField] private int nextPatternIndex = 0;

    public TickTimer screamDealy;



    private void OnTriggerEnter(UnityEngine.Collider other)
    {
        if (other.gameObject.layer == 7)
        {
            monster.TryAddTarget(other.transform);
            monster.SetTarget(other.transform);
            monster.SetTargetRandomly();

            /// 타겟으로 플레이어가 들어오면
            /// 여기에서 Scream 횟수, Scream의 쿨타임 판단하고
            /// 다음 패턴의 인덱스를 구분하여 단순히 Chase할 것인지
            /// Scream을 할 것인지 결정한다

            if (screamDealy.ExpiredOrNotRunning(Runner))
            {
                screamDealy = TickTimer.CreateFromSeconds(Runner, 50f);
                monster.FSM.ChangePhase<Grita_Phase_Scream>();
            }
        }
    }


    /// 여기에서 다음 상태를 판단해야한다



}
