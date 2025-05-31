using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerIdleState : PlayerGroundState
{
    
    public PlayerIdleState(PlayerController controller, PlayerStateMachine stateMachine) : base(controller, stateMachine)
    {
    }

    // ���� ���� ����
    public override void Enter()
    {
        // idle ���¿����� �Է¿� ������ ����
        controller.LockState = PlayerLockState.Free;

        base.Enter();   // Ground ����
        player.animationController.Die = false;
    }

    public override void OnUpdate(NetworkInputData data)
    {
        base.OnUpdate(data);
        player.animationController.isSitting = data.isSitting;
        player.animationController.isRunning = data.isRunning;
        player.Weapons.OffSprint();

    }
    public override void Exit()
    {
        base.Exit();
        //StopAnimation(stateMachine.Player.AnimationData.IdleParameterHash);
    }
}
