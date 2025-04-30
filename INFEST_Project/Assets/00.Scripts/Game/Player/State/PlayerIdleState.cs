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

        //player.animationController.MoveDirection = data.direction;
        //PlayerMove(data);

        //player.animationController.isFiring = data.isFiring;
        //if (data.isFiring)
        //    PlayerFire(data);

        // ������ ���¿��� �¿�ȸ��
        if (data.direction != Vector3.zero)
        {
            stateMachine.ChangeState(stateMachine.MoveState);
        }
        if ((controller.IsGrounded()) && data.isJumping)
        {
            player.animationController.MoveDirection = data.direction;

            stateMachine.ChangeState(stateMachine.JumpState);
        }
        // �ϴ� ����(isShotgunOnFiring)�� ���ۼ�
        if (stateMachine.Player.GetWeapons() != null && data.isFiring)
        {
            stateMachine.ChangeState(stateMachine.AttackState);
            //player.animationController.isFiring = data.isFiring;
            //PlayerFire(data);
        }
        if (controller.IsGrounded() && data.isZooming)
        {
            stateMachine.ChangeState(stateMachine.AimState);
        }
        if (controller.IsGrounded() && data.isSitting)
        {
            stateMachine.ChangeState(stateMachine.SitIdleState);
        }
        //if (stateMachine.Player.GetWeapons() != null && data.isReloading)
        //{
        //    stateMachine.ChangeState(stateMachine.ReloadState);
        //}
        //if (controller.IsGrounded() && data.isRunning)
        //{
        //    stateMachine.ChangeState(stateMachine.RunState);
        //}
    }
}
