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
        Debug.Log("Attack상태 진입");
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
        // blend tree 애니메이션에서는 입력값을 업데이트해서 애니메이션을 변경해야한다

        // 사격
        PlayerFire(data);
        controller.ApplyGravity();  // 중력

        // 이동 입력이 없으면 Idle 상태로
        if (!data.isFiring)
        {
            stateMachine.ChangeState(stateMachine.IdleState);
        }
    }
}
