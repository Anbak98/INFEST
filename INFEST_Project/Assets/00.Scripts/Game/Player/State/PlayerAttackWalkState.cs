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

        //StartAnimation(stateMachine.Player.AnimationData.AttackParameterHash);
    }
    public override void Exit()
    {
        base.Exit();
        //StopAnimation(stateMachine.Player.AnimationData.AttackParameterHash);
    }

    public override void OnUpdate(NetworkInputData data)
    {
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
        if (data.isReloading)
        {
            stateMachine.ChangeState(stateMachine.ReloadState);
        }

    }

}
