using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerRunState : PlayerGroundState
{
    public PlayerRunState(PlayerController controller, PlayerStateMachine stateMachine) : base(controller, stateMachine)
    {
    }

    public override void Enter()
    {
        // 달리는 중에 앉기, 조준 불가
        controller.LockState = PlayerLockState.SitLock | PlayerLockState.ZoomLock | PlayerLockState.FireLock;

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
