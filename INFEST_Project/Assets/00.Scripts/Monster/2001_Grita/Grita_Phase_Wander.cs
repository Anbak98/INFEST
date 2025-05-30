using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grita_Phase_Wander : MonsterPhase<Monster_Grita>
{
    //public TickTimer screemCooldownTickTimer;
    public override void MachineEnter()
    {
        base.MachineEnter();
        monster.IsWonderPhase = true;

        // 0초짜리 만들고 Expired를 true로 만들어서 1번 실행
        //screemCooldownTickTimer = TickTimer.CreateFromSeconds(Runner, 0f);
    }

    // 플레이어가 감지범위 이내에 들어오면 Scream으로 상태를 바꿔야한다
    // Idle, Walk state 모두 적용되므로 이곳에 작성한다..?


    public override void MachineExecute()
    {
        base.MachineExecute();

        if (monster.IsFindPlayer() && !monster.IsDead)
        {
            monster.FSM.ChangePhase<Grita_Phase_Chase>();
        }
    }

    public override void MachineExit()
    {
        base.MachineExit();
        monster.IsWonderPhase = false;
    }
}
