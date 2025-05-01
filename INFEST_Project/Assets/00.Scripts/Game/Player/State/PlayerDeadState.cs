using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 앉아 있는 상태
// SitIdle, Waddle, SitAttack, SitReload
public class PlayerDeadState : PlayerBaseState
{
    public PlayerDeadState(PlayerController controller, PlayerStateMachine stateMachine) : base(controller, stateMachine)
    {
    }

    public override void Enter()
    {
        player.animationController.Die = true;
        player.stateMachine.IsDead = true;
        Debug.Log("????");
    }

    public override void Exit()
    {
    }

    public override void OnUpdate(NetworkInputData data)
    {
        // Jump, Fall 상태에서 죽었다면 바닥에 닿을 때까지 떨어져야한다
        controller.ApplyGravity();  

        // 키 입력을 통한 관전(다른 플레이어의 카메라로)

    }

    public override void PhysicsUpdate(NetworkInputData data)
    {
    }
}
