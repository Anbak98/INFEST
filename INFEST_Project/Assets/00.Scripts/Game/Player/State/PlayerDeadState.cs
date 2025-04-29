using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 앉아 있는 상태
// SitIdle, Waddle, SitAttack, SitReload
public class PlayerDeadState : PlayerGroundState
{
    public PlayerDeadState(PlayerController controller, PlayerStateMachine stateMachine) : base(controller, stateMachine)
    {
    }

    public override void Enter()
    {
        player.animationController.Die = true;
    }

    public override void Exit()
    {
    }

    public override void OnUpdate(NetworkInputData data)
    {
    }

    public override void PhysicsUpdate(NetworkInputData data)
    {
    }
}
