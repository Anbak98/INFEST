using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class PlayerMoveState : PlayerGroundState
{
    public PlayerMoveState(PlayerController controller, PlayerStateMachine stateMachine) : base(controller, stateMachine)
    {
    }

    public override void Enter()
    {
        // °È´Â Áß¿¡´Â ¾É±â ºÒ°¡
        controller.LockState = PlayerLockState.SitLock;

        base.Enter();
    }

    public override void OnUpdate(NetworkInputData data)
    {
        base.OnUpdate(data);
        player.animationController.isRunning = data.isRunning;
    }
    public override void Exit()
    {
        base.Exit();
    }

}
