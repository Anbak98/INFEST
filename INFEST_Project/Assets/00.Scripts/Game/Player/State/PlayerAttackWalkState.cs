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

        player.animationController.isFiring = data.isFiring;
        if (data.isFiring)
            PlayerFire(data);

        // 이동 입력이 없으면 Attack 상태로
        if (data.direction == Vector3.zero)
        {
            stateMachine.ChangeState(stateMachine.AttackState);
        }
        if (!data.isFiring)
        {
            stateMachine.ChangeState(stateMachine.AimState);
        }
        if ((controller.IsGrounded()) && data.isJumping)
        {
            player.animationController.MoveDirection = data.direction;

            stateMachine.ChangeState(stateMachine.JumpState);
        }
    }
}
