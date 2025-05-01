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
        base.OnUpdate(data);

        // blend tree �ִϸ��̼ǿ����� �Է°��� ������Ʈ�ؼ� �ִϸ��̼��� �����ؾ��Ѵ�        
        player.animationController.MoveDirection = data.direction;
        PlayerMove(data);

        if (data.direction == Vector3.zero)
        {
            stateMachine.ChangeState(stateMachine.IdleState);
        }
        if (data.isRunning)
        {
            player.animationController.isRunning = data.isRunning;
            stateMachine.ChangeState(stateMachine.RunState);
        }
        if (data.isJumping)
        {
            player.animationController.isJumping = data.isJumping;
            stateMachine.ChangeState(stateMachine.JumpState);
        }


        if ((stateMachine.Player.GetWeapons() != null) && data.isFiring)
        {
            player.animationController.isFiring = data.isFiring;
            PlayerFire(data);

            stateMachine.ChangeState(stateMachine.AttackWalkState);
            //player.animationController.isFiring = data.isFiring;
            //PlayerFire(data);
        }
        if (data.isZooming)
        {
            stateMachine.ChangeState(stateMachine.AimWalkState);
        }

        //if (data.isSitting)
        //{
        //    stateMachine.ChangeState(stateMachine.SitIdleState);
        //}
    }
}
