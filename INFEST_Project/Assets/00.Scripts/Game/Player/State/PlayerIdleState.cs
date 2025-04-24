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
        // �ϴ� ���ڴ���. ���߿� PlayStatData.WalkSpeedModifier ���� �߰��ؼ� ��,������ �ٲ۴�
        stateMachine.StatHandler.MoveSpeedModifier = 0;

        base.Enter();   // Ground ����
        //Debug.Log("Idle���� ����");

        // Ground && Idle
        //StartAnimation(stateMachine.Player.AnimationData.IdleParameterHash);
    }

    public override void Exit()
    {
        base.Exit();
        //StopAnimation(stateMachine.Player.AnimationData.IdleParameterHash);
    }

    public override void PhysicsUpdate(NetworkInputData data)
    {
        base.PhysicsUpdate(data);
    }

    public override void OnUpdate(NetworkInputData data)
    {
        base.OnUpdate(data);
        //controller.ApplyGravity();  // �߷�

        // Idle ���� X, Z �������� ȸ���� �� �־����
        player.animationController.LookDirection = data.lookDelta;



        if (controller.IsGrounded() && data.direction != Vector3.zero)
        {
            stateMachine.ChangeState(stateMachine.MoveState);
        }
        else if ((controller.IsGrounded()) && data.isJumping)
        {
            stateMachine.ChangeState(stateMachine.JumpState);
        }
        else if (controller.IsGrounded() && data.isReloading)
        {
            stateMachine.ChangeState(stateMachine.ReloadState);
        }
        // �ϴ� ����(isShotgunOnFiring)�� ���ۼ�
        else if (controller.IsGrounded() && (stateMachine.Player.GetWeapons() != null) && data.isFiring)
        {
            stateMachine.ChangeState(stateMachine.AttackState);
        }
        else if (controller.IsGrounded() && (stateMachine.Player.GetWeapons() != null) && data.isReloading)
        {
            stateMachine.ChangeState(stateMachine.ReloadState);
        }
        else if (controller.IsGrounded() && data.isRunning)
        {
            stateMachine.ChangeState(stateMachine.RunState);
        }
        else if (controller.IsGrounded() && data.isSitting)
        {
            stateMachine.ChangeState(stateMachine.SitIdleState);
        }
    }
}
