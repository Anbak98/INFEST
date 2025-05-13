using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grita_Phase_Wander : MonsterPhase<Monster_Grita>
{
    public TickTimer screemCooldownTickTimer;

    public override void MachineEnter()
    {
        base.MachineEnter();

        // 0초짜리 만들고 Expired를 true로 만들어서 1번 실행
        screemCooldownTickTimer = TickTimer.CreateFromSeconds(Runner, 0f);
    }

    // 플레이어가 감지범위 이내에 들어오면 Scream으로 상태를 바꿔야한다
    // Idle, Walk state 모두 적용되므로 이곳에 작성한다..?
    public override void MachineExecute()
    {


        base.MachineExecute();

        //if ()
        // FSM이 실행되면 기본 Phase의 0번 State가 실행
        // State에서 전환은 State 내에서 이루어지며
        // Phase의 전환은 어디에서 이루어지나?
        // 아무데서 하면 된다
        // State에서 다른 Phase 내의 State로 전환하고 싶다면 State내에서 호출하는 것이 맞다
       

    }

}
