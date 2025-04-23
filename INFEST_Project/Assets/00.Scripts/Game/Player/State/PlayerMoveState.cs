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
        //stateMachine.StatHandler.MoveSpeedModifier = 2;
        Debug.Log("Move���� ����");
        base.Enter();
    }
    public override void Exit()
    {
        base.Exit();
        // ���� �ʱ�ȭ
        SetAnimationFloat(stateMachine.Player.AnimationData.MoveXParameterHash, 0f);
        SetAnimationFloat(stateMachine.Player.AnimationData.MoveZParameterHash, 0f);
    }

    public override void OnUpdate(NetworkInputData data)
    {
        // blend tree �ִϸ��̼ǿ����� �Է°��� ������Ʈ�ؼ� �ִϸ��̼��� �����ؾ��Ѵ�        
        player.animationController.MoveDirection = data.direction;

        // ���������� Blend Tree �Ķ���� ������Ʈ
        SetAnimationFloat(stateMachine.Player.AnimationData.MoveXParameterHash, moveInput.x);
        SetAnimationFloat(stateMachine.Player.AnimationData.MoveZParameterHash, moveInput.y);

        PlayerMove(data);
        controller.ApplyGravity();  // �߷�

        if (data.direction == Vector3.zero)
        {
            stateMachine.ChangeState(stateMachine.IdleState);
        }
        if (data.isRunning)
        {
            stateMachine.ChangeState(stateMachine.RunState);
        }
        if ((stateMachine.Player.GetWeapons() != null) && data.isFiring)
        {
            stateMachine.ChangeState(stateMachine.AttackState);
        }
        if ((stateMachine.Player.GetWeapons() != null) && data.isReloading)
        {
            stateMachine.ChangeState(stateMachine.ReloadState);
        }
        if (data.isJumping)
        {
            //if (controller.IsGrounded())
            //{
            stateMachine.ChangeState(stateMachine.JumpState);
            //}
        }
        if (data.isSitting)
        {
            stateMachine.ChangeState(stateMachine.SitIdleState);
        }
    }
}
