using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

// 서 있는 상태
// Idle, Move, Run, Attack, Reload
public class PlayerGroundState : PlayerBaseState
{
    public PlayerGroundState(PlayerController controller, PlayerStateMachine stateMachine) : base(controller, stateMachine)
    {
    }
    public override void Enter()
    {
        base.Enter();
        //StartAnimation(stateMachine.Player.AnimationData.GroundParameterHash);
    }
    public override void Exit()
    {
        base.Exit();    // 상단의 layer로 나간다
        //StopAnimation(stateMachine.Player.AnimationData.GroundParameterHash);
    }

    public override void OnUpdate(NetworkInputData data)
    {
    }
    public override void PhysicsUpdate(NetworkInputData data)
    {
    }
}
