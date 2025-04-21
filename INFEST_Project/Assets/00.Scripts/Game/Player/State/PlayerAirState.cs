using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 지면에 발이 떨어진 상태
// Jump, Fall
public class PlayerAirState : PlayerBaseState
{
    public PlayerAirState(PlayerController controller, PlayerStateMachine stateMachine) : base(controller, stateMachine)
    {
    }

    public override void Enter()
    {
        Debug.Log("Air상태 진입");
        base.Enter();

        StartAnimation(stateMachine.Player.AnimationData.AimParameterHash);
    }
    public override void Exit()
    {
        base.Exit();    // 상단의 layer로 나간다
        StopAnimation(stateMachine.Player.AnimationData.AimParameterHash);
    }

    public override void Update()
    {
    }
    public override void PhysicsUpdate()
    {
    }
}
