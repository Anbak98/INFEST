using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class PlayerMoveState : PlayerGroundState
{
    public PlayerMoveState(PlayerController controller, PlayerStateMachine stateMachine) : base(controller, stateMachine)
    {
    }
    public override void Enter()
    {
        // 일단 숫자대입. 나중에 PlayStatData.WalkSpeedModifier 변수 추가해서 그,값으로 바꾼다
        stateMachine.StatHandler.MoveSpeedModifier = 2;
        Debug.Log("Move상태 진입");
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
        PlayerMove();
        controller.ApplyGravity();  // 중력


        //// 이동 입력이 없으면 Idle 상태로
        //if (moveInput == Vector2.zero)
        //{
        //    stateMachine.ChangeState(stateMachine.IdleState);
        //}
        //// run 눌렀다면 달리기(지금은 1번 눌러서 bool값을 설정하면 다시 누르기 전까지 계속 달리기 모드)
        //else if (stateMachine.InputHandler.GetIsRunning())
        //{
        //    stateMachine.ChangeState(stateMachine.RunState);
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
        //// jump 눌렀다면 점프
        //else if (stateMachine.InputHandler.GetIsJumping())
        //{
        //    stateMachine.ChangeState(stateMachine.JumpState);
        //}
        //// 앉기로 전환
        //else if (stateMachine.InputHandler.GetIsSitting())
        //{
        //    stateMachine.ChangeState(stateMachine.SitIdleState);
        //}
    }

    // Move에서 사용할 이벤트는 
    protected override void AddInputActionsCallbacks()
    {
        inputManager.GetInput(EPlayerInput.run).started += OnRunStarted;    
        inputManager.GetInput(EPlayerInput.fire).started += OnAttack;
        inputManager.GetInput(EPlayerInput.reload).started += OnReload;
        inputManager.GetInput(EPlayerInput.sit).started += OnSitStarted;
        inputManager.GetInput(EPlayerInput.jump).started += OnJumpStarted;
    }
    protected override void RemoveInputActionsCallbacks()
    {
        //inputManager.GetInput(EPlayerInput.run).canceled += OnRunStarted;
        //inputManager.GetInput(EPlayerInput.fire).canceled += OnAttack;
        //inputManager.GetInput(EPlayerInput.reload).canceled += OnReload;
        //inputManager.GetInput(EPlayerInput.sit).canceled += OnSitStarted;
        //inputManager.GetInput(EPlayerInput.jump).canceled += OnJumpStarted;
        inputManager.GetInput(EPlayerInput.run).started -= OnRunStarted;
        inputManager.GetInput(EPlayerInput.fire).started -= OnAttack;
        inputManager.GetInput(EPlayerInput.reload).started -= OnReload;
        inputManager.GetInput(EPlayerInput.sit).started -= OnSitStarted;
        inputManager.GetInput(EPlayerInput.jump).started -= OnJumpStarted;
    }





    // Move에서는 Idle, Run, Attack, Reload, Jump, Sit으로 상태전환 가능하다
    protected override void OnRunStarted(InputAction.CallbackContext context)
    {
        base.OnRunStarted(context);
        stateMachine.ChangeState(stateMachine.RunState);
    }
    protected override void OnAttack(InputAction.CallbackContext context)
    {
        base.OnAttack(context);
        stateMachine.ChangeState(stateMachine.AttackState);
    }
    protected override void OnReload(InputAction.CallbackContext context)
    {
        base.OnReload(context);
        stateMachine.ChangeState(stateMachine.ReloadState);
    }
    protected override void OnJumpStarted(InputAction.CallbackContext context)
    {
        base.OnJumpStarted(context);
        stateMachine.ChangeState(stateMachine.JumpState);
    }
    protected override void OnSitStarted(InputAction.CallbackContext context)
    {
        base.OnSitStarted(context);
        stateMachine.ChangeState(stateMachine.SitIdleState);
    }

}
