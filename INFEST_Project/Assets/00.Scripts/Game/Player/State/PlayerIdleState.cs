using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerIdleState : PlayerBaseState
{
    public PlayerIdleState(PlayerController controller, PlayerStateMachine stateMachine) : base(controller, stateMachine)
    {
    }

    // 가장 먼저 시작
    public override void Enter()
    {
        stateMachine.StatHandler.MoveSpeedModifier = 0;
        Debug.Log("Idle상태 진입");
        base.Enter();

        // Blend Tree의 중심 (0, 0)으로 애니메이션을 전환함
        SetAnimationFloat(stateMachine.Player.AnimationData.MoveXParameterHash, 0f);
        SetAnimationFloat(stateMachine.Player.AnimationData.MoveZParameterHash, 0f);
    }

    public override void Exit()
    {
        base.Exit();
    }
    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
    public override void Update()
    {
        base.Update();
        // 입력값이 있다면 MoveState로 전환
        if (stateMachine.InputHandler.GetMoveInput() != Vector3.zero)
        {
            stateMachine.ChangeState(stateMachine.MoveState);
            return;
        }
    }



    protected override void OnMovementCanceled(InputAction.CallbackContext context)
    {

    }
    protected override void OnRunStarted(InputAction.CallbackContext context)
    {

    }


}
