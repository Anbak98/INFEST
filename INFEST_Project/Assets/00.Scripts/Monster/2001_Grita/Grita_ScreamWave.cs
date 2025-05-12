using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 무작정 소리를 지르는건 없으니까 ScreamWave는 없다고 보는 것이 맞다
// Scream State에서 호출하는 함수를 구분하는 것이 맞다고 생각한다
// 매개변수로 radius를 구분하면, if같은거로 경우를 나눌 필요도 없겠지
public class Grita_ScreamWave : MonsterStateNetworkBehaviour<Monster_Grita>
{
    public override void Enter()
    {
        base.Enter();

    }
    public override void Execute()
    {
        base.Execute();

    }

    public override void Exit()
    {
        base.Exit();

    }

}
