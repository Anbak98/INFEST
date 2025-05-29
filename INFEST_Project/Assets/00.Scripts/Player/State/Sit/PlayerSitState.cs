using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 앉아 있는 상태
// SitIdle, Waddle, SitAttack, SitReload
public class PlayerSitState : PlayerBaseState
{
    
    public PlayerSitState(PlayerController controller, PlayerStateMachine stateMachine) : base(controller, stateMachine)
    {
    }

    public override void Enter()
    {
        // 앉아 있는 모든 상태에서 달리기 불가
        controller.LockState = PlayerLockState.RunLock;

        base.Enter();

        // 앉는다
        controller.StartSit();
    }
    public override void Exit()
    {
        base.Exit();
        // 일어난다
        controller.StartStand();
    }

    public override void OnUpdate(NetworkInputData data)
    {
        base.OnUpdate(data);
        player.animationController.isSitting = data.isSitting;

    }
    public override void PhysicsUpdate(NetworkInputData data)
    {
    }
}
