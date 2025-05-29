using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

// ThirdPersonRoot�� Attack �ִϸ��̼��� ����
// FirstPersonRoot�� �ִϸ��̼��� ������ ������ 
// ��� ���¿��� Attack�� �����ϴϱ� DeadState�� ���������� ���� ó���Ѵ�
public class PlayerAttackState : PlayerBaseState
{
    public PlayerAttackState(PlayerController controller, PlayerStateMachine stateMachine) : base(controller, stateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }
    public override void Exit()
    {
        base.Exit();
    }

    public override void OnUpdate(NetworkInputData data)
    {
        base.OnUpdate(data);

        //player.animationController.MoveDirection = data.direction;
        //PlayerMove(data);

        player.animationController.isFiring = data.isFiring;
        //PlayerFire(data);

        /*
        //if (data.direction != Vector3.zero)
        //{
        //    stateMachine.ChangeState(stateMachine.AttackWalkState);
        //}
        if (!data.isFiring)
        {
            stateMachine.ChangeState(stateMachine.IdleState);
        }
        if ((controller.IsGrounded()) && data.isJumping)
        {
            player.animationController.MoveDirection = data.direction;

            stateMachine.ChangeState(stateMachine.JumpState);
        }
        */
    }
}
