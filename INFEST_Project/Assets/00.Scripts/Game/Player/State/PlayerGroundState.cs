using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

// 땅에 붙어있는 모든 상태 공통 적용
// Idle, Move, Run, Attack, Reload
public class PlayerGroundState : PlayerBaseState
{
    float lookThreshold = 0.3f; // 얼마나 좌우로 보는지를 판단할 기준 값 (조절 가능)

    public PlayerGroundState(PlayerController controller, PlayerStateMachine stateMachine) : base(controller, stateMachine)
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
        player.animationController.lookDelta = data.lookDelta;
        if (data.lookDelta.y != 0f)
        {
            // 위 또는 아래를 내려본다
            player.animationController.playerAnimator.GetLayerIndex("Look");    // Layer를 변경한다
        }
        // lookDelta.x의 범위에 따라
        // 회전각을 -30도에서 +30도일때는 상체(UpperBody)만 회전할지
        // 그 밖의 범위에서는 
        // 몸 전체(Base)가 회전할지 Layer로 구분한다
        if (Mathf.Abs(data.lookDelta.x) < lookThreshold)
        {
            // 상체만 회전한다
            player.animationController.playerAnimator.GetLayerIndex()
        }

    }
    public override void PhysicsUpdate(NetworkInputData data)
    {
    }
}
