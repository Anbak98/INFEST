using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWaddleState : PlayerSitState
{
    public PlayerWaddleState(PlayerController controller, PlayerStateMachine stateMachine) : base(controller, stateMachine)
    {
    }
    public override void Enter()
    {
        // 일단 숫자대입. 나중에 PlayStatData.WalkSpeedModifier 변수 추가해서 그,값으로 바꾼다
        stateMachine.StatHandler.MoveSpeedModifier = 1; // 걷는 속도의 0.5배
        Debug.Log("Waddle상태 진입");
        base.Enter();


        /// blend tree 애니메이션에 적용
        SetAnimationFloat(stateMachine.Player.AnimationData.MoveXParameterHash, stateMachine.InputHandler.MoveInput.x);
        SetAnimationFloat(stateMachine.Player.AnimationData.MoveZParameterHash, stateMachine.InputHandler.MoveInput.y);
    }
    public override void Exit()
    {
        base.Exit();
        // 방향 초기화
        SetAnimationFloat(stateMachine.Player.AnimationData.MoveXParameterHash, 0f);
        SetAnimationFloat(stateMachine.Player.AnimationData.MoveZParameterHash, 0f);
    }

    public override void Update()
    {
        // blend tree 애니메이션에서는 입력값을 업데이트해서 애니메이션을 변경해야한다
        Vector2 moveInput = stateMachine.InputHandler.MoveInput;

        // 지속적으로 Blend Tree 파라미터 업데이트
        SetAnimationFloat(stateMachine.Player.AnimationData.MoveXParameterHash, moveInput.x);
        SetAnimationFloat(stateMachine.Player.AnimationData.MoveZParameterHash, moveInput.y);

        // 플레이어 이동
        PlayerWaddle();
        controller.ApplyGravity();  // 중력


        if (!stateMachine.InputHandler.GetIsSitting())
        {
            stateMachine.ChangeState(stateMachine.IdleState);
        }

        //// 이동 입력이 없으면 Sit 상태로
        //if (moveInput == Vector2.zero)
        //{
        //    stateMachine.ChangeState(stateMachine.SitIdleState);
        //}
        //// 무기를 가진 상태에서 공격버튼 눌렀다면 공격상태
        //else if ((stateMachine.Player.GetWeapon() != null) && stateMachine.InputHandler.GetIsFiring())
        //{

        //}
        //// 무기를 가진 상태에서 입력이 있으면 Reload 상태로
        //else if ((stateMachine.Player.GetWeapon() != null) && stateMachine.InputHandler.GetIsReloading())
        //{
        //    stateMachine.ChangeState(stateMachine.RunState);
        //}
        //// Idle로 전환
        //else if (!stateMachine.InputHandler.GetIsSitting())
        //{
        //    stateMachine.ChangeState(stateMachine.IdleState);
        //}

    }

    protected override void AddInputActionsCallbacks()
    {
    }
    protected override void RemoveInputActionsCallbacks()
    {
    }
}
