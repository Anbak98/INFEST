using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAimAttackState : PlayerGroundState
{
    public PlayerAimAttackState(PlayerController controller, PlayerStateMachine stateMachine) : base(controller, stateMachine)
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
        //if (data.isFiring)
        PlayerFire(data);

        // AimAttack 상태에서는 Aim, Attack, AimAttackWalk, 
        if (data.direction != Vector3.zero)
        {
            stateMachine.ChangeState(stateMachine.AimAttackWalkState);
        }
        if (!data.isZooming)
        {
            stateMachine.ChangeState(stateMachine.AttackState);
        }
        if (!data.isFiring)
        {
            stateMachine.ChangeState(stateMachine.AimState);
        }
    }
}
