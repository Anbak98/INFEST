using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSitIdleState : PlayerSitState
{
    public PlayerSitIdleState(PlayerController controller, PlayerStateMachine stateMachine) : base(controller, stateMachine)
    {
    }
    public override void Enter()
    {
        //stateMachine.StatHandler.MoveSpeedModifier = 0;

        base.Enter();
        //Debug.Log("SitIdle상태 진입");

        // Sit && Idle
        //StartAnimation(stateMachine.Player.AnimationData.IdleParameterHash);
    }
    public override void Exit()
    {
        base.Exit();
        //StopAnimation(stateMachine.Player.AnimationData.IdleParameterHash);
    }

    public override void OnUpdate(NetworkInputData data)
    {
        base.OnUpdate(data);

        controller.ApplyGravity();  // 중력

        if (data.direction != Vector3.zero)
        {
            stateMachine.ChangeState(stateMachine.MoveState);
        }
        if (!data.isSitting)
        {
            stateMachine.ChangeState(stateMachine.IdleState);
        }
        // isSitting && isFiring
        if ((stateMachine.Player.GetWeapons() != null) && data.isFiring)
        {
            stateMachine.ChangeState(stateMachine.SitAttackState);
        }
        // isSitting && isReloading
        if ((stateMachine.Player.GetWeapons() != null) && data.isReloading)
        {
            stateMachine.ChangeState(stateMachine.SitReloadState);
        }
    }
    public override void PhysicsUpdate(NetworkInputData data)
    {
    }
}
