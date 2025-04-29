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

        // 사격
        player.animationController.isFiring = data.isFiring;
        PlayerFire(data);
        //controller.ApplyGravity();  // 중력

        // 이동 입력이 없으면 Idle 상태로
        if (!data.isFiring)
        {
            stateMachine.ChangeState(stateMachine.IdleState);
        }

    }
}
