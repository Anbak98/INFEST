using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAimWalkState : PlayerGroundState
{
    public PlayerAimWalkState(PlayerController controller, PlayerStateMachine stateMachine) : base(controller, stateMachine)
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

        // AimWalk ���¿����� Aim, Walk, AimAttackWalk ���·� ���� ����
        if (data.direction == Vector3.zero)
        {
            stateMachine.ChangeState(stateMachine.AimState);
        }
        if (!data.isZooming)
        {
            stateMachine.ChangeState(stateMachine.MoveState);
        }
        if (data.isFiring)
        {
            stateMachine.ChangeState(stateMachine.AimAttackWalkState);
        }
    }
}
