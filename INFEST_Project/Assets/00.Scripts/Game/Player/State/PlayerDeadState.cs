using Cinemachine;
using Fusion;
using INFEST.Game;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeadState : PlayerBaseState
{
    public PlayerDeadState(PlayerController controller, PlayerStateMachine stateMachine) : base(controller, stateMachine)
    {
    }

    public override void Enter()
    {
        player.animationController.Die = true;
        stateMachine.IsDead = true;

        Debug.Log("DeadState");
    }

    public override void Exit()
    {
    }

    public override void OnUpdate(NetworkInputData data)
    {
    }

    public override void PhysicsUpdate(NetworkInputData data)
    {
    }

}
