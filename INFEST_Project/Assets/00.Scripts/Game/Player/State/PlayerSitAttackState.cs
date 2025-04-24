using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSitAttackState : PlayerSitState
{
    public PlayerSitAttackState(PlayerController controller, PlayerStateMachine stateMachine) : base(controller, stateMachine)
    {
    }

    public override void Enter()
    {
        Debug.Log("SitAttack상태 진입");
        base.Enter();

        // Sit && Attack
        //StartAnimation(stateMachine.Player.AnimationData.AttackParameterHash);
    }
    public override void Exit()
    {
        base.Exit();    // 상단의 layer로 나간다
        //StopAnimation(stateMachine.Player.AnimationData.AttackParameterHash);
    }
    public override void OnUpdate(NetworkInputData data)
    {
        bool isFire = data.isFiring;

        // 사격
        PlayerSitFire(data);
        controller.ApplyGravity();  // 중력

        // 이동 입력이 없으면 Idle 상태로
        if (!data.isFiring)
        {
            stateMachine.ChangeState(stateMachine.SitIdleState);
            return;
        }

    }
    public override void PhysicsUpdate(NetworkInputData data)
    {
    }
}
