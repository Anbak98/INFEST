using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerFallState : PlayerAirState
{
    public PlayerFallState(PlayerController controller, PlayerStateMachine stateMachine, InputManager inputManager) : base(controller, stateMachine, inputManager)
    {
    }



    public override void Enter()
    {
        // 지면에서 떨어졌다 && y방향 속도가 0보다 작거나 같으면 Fall


        base.Enter();
    }
    public override void OnUpdate(NetworkInputData data)
    {
        // 땅에 닿을 때까지(IsGrounded == true) 중력을 받는다

    }

    public override void Exit()
    {
        base.Exit();
    }
}
