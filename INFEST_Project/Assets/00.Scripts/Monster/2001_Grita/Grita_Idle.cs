using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grita_Idle : MonsterStateNetworkBehaviour
{
    public TickTimer _tickTimer;

    public override void Enter()
    {
        base.Enter();
        monster.MovementSpeed = 0f;
        _tickTimer = TickTimer.CreateFromSeconds(Runner, 7);
    }
    public override void Execute()
    {
        base.Execute();

        if (_tickTimer.Expired(Runner)) // 일정 시간 초과하면 true 리턴
        {
            phase.ChangeState<Grita_Walk>(); // state 교체

            //// 예시
            //monster.FSM.ChangePhase<PJ_HI_II_DeadPhase>(); //phase 교체
            //monster.MovementSpeed = monster.info.SpeedMove; //기획서에 있는 달리기 속도
            //monster.AIPathing.SetDestination(monster.target.transform.position); // 추격
            //// Monsterinfo에서 불러옴



        }
    }
}
