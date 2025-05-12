using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Attack과 AttackWave차이
// AttackWave는 Wave가 시작될 때 공격으로
// 기획에 따라 Attack과 다른 동작을 할 수 있어서 분리했다
public class Grita_AttackWave : MonsterStateNetworkBehaviour<Monster_Grita>
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
