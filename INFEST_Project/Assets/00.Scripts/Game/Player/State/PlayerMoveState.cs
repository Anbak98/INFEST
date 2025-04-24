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
        // �ϴ� ���ڴ���. ���߿� PlayStatData.WalkSpeedModifier ���� �߰��ؼ� ��,������ �ٲ۴�
        stateMachine.StatHandler.MoveSpeedModifier = 4;
        Debug.Log("Move���� ����");
        base.Enter();
    }
    public override void Exit()
    {
        base.Exit();
    }

    public override void OnUpdate(NetworkInputData data)
    {
        // blend tree �ִϸ��̼ǿ����� �Է°��� ������Ʈ�ؼ� �ִϸ��̼��� �����ؾ��Ѵ�        
        player.animationController.MoveDirection = data.direction;


        PlayerMove(data);
        //controller.ApplyGravity();  // �߷�

        if (controller.IsGrounded() && data.direction == Vector3.zero)
        {
            stateMachine.ChangeState(stateMachine.IdleState);
        }
        else if (controller.IsGrounded() && data.isRunning)
        {
            stateMachine.ChangeState(stateMachine.RunState);
        }
        else if (controller.IsGrounded() && (stateMachine.Player.GetWeapons() != null) && data.isFiring)
        {
            stateMachine.ChangeState(stateMachine.AttackState);
        }
        else if (controller.IsGrounded() && (stateMachine.Player.GetWeapons() != null) && data.isReloading)
        {
            stateMachine.ChangeState(stateMachine.ReloadState);
        }
        else if ((controller.IsGrounded()) && data.isJumping)
        {
            stateMachine.ChangeState(stateMachine.JumpState);
        }
        else if (controller.IsGrounded() && data.isSitting)
        {
            stateMachine.ChangeState(stateMachine.SitIdleState);
        }
    }
}
