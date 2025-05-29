using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ThirdPersonRoot�� Aim �ִϸ��̼��� ����
// FirstPersonRoot�� ������ ������, Sit�� Stand�� ������ ����
public class PlayerAimState : PlayerGroundState
{
    public PlayerAimState(PlayerController controller, PlayerStateMachine stateMachine) : base(controller, stateMachine)
    {
    }

    public override void Enter()
    {
        // �����߿��� �޸���, ���� �Ұ�
        controller.LockState = PlayerLockState.RunLock | PlayerLockState.JumpLock;

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

        //player.animationController.isFiring = data.isFiring;
        //if (data.isFiring)
        //    PlayerFire(data);

        /*
        // Aim ���¿����� Idle, AimWalk, AimAttack���� ��ȯ ����
        if (data.direction != Vector3.zero)
        {
            stateMachine.ChangeState(stateMachine.AimWalkState);
        }
        if (data.isFiring)
        {
            player.animationController.isFiring = data.isFiring;
            PlayerFire(data);

            stateMachine.ChangeState(stateMachine.AimAttackState);
        }
        if (!data.isZooming)
        {
            stateMachine.ChangeState(stateMachine.IdleState);
        }
        */
    }
}
