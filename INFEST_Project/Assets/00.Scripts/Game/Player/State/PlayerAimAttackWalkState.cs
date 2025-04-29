using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAimAttackWalkState : PlayerGroundState
{
    public PlayerAimAttackWalkState(PlayerController controller, PlayerStateMachine stateMachine) : base(controller, stateMachine)
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
        player.animationController.MoveDirection = data.direction;
        PlayerMove(data);

        // AimAttackWalk 상태에서는 Aim, Move, AimWalk, 
        if (data.direction == Vector3.zero)
        {
            stateMachine.ChangeState(stateMachine.AimAttackState);
        }
        if (!data.isZooming)
        {
            stateMachine.ChangeState(stateMachine.MoveState);
        }
        if (!data.isFiring)
        {
            stateMachine.ChangeState(stateMachine.AimWalkState);
        }
    }
}
