using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttackState : PlayerGroundState
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
        player.animationController.isFiring = data.isFiring;
        PlayerFire(data);

        if (!data.isFiring)
        {
            stateMachine.ChangeState(stateMachine.IdleState);
        }

    }
}
