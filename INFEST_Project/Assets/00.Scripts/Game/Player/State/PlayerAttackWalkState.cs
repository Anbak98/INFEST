using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackWalkState : PlayerGroundState
{
    public PlayerAttackWalkState(PlayerController controller, PlayerStateMachine stateMachine) : base(controller, stateMachine)
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

        player.animationController.MoveDirection = data.direction;
        PlayerMove(data);

        // ���
        player.animationController.isFiring = data.isFiring;
        PlayerFire(data);
        //controller.ApplyGravity();  // �߷�

        // �̵� �Է��� ������ Idle ���·�
        if (!data.isFiring)
        {
            stateMachine.ChangeState(stateMachine.IdleState);
        }

    }
}
