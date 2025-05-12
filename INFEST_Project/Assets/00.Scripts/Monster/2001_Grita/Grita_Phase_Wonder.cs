using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grita_Phase_Wonder : MonsterPhase<Monster_Grita>
{

    // 플레이어가 감지범위 이내에 들어오면 Scream으로 상태를 바꿔야한다
    // Idle, Walk state 모두 적용되므로 이곳에 작성한다
    public override void MachineExecute()
    {
        base.MachineExecute();

        //if ()

    }

}
