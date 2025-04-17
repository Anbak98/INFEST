using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerRunState : PlayerBaseState
{
    public PlayerRunState(PlayerController controller, PlayerStateMachine stateMachine) : base(controller, stateMachine)
    {
    }

    public override void Enter()
    {
        // 일단 숫자대입. 나중에 PlayStatData.RunSpeedModifier 변수 추가해서 그,값으로 바꾼다
        stateMachine.StatHandler.MoveSpeedModifier = 4;
        Debug.Log("Run상태 진입");
        base.Enter();
        StartAnimation(stateMachine.Player.AnimationData.IdleParameterHash);
    }
    public override void Exit()
    {
        base.Exit();
        StopAnimation(stateMachine.Player.AnimationData.IdleParameterHash);
    }

    public override void Update()
    {

    }
}
