using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public interface IState
{
    public void Enter();    // 상태 진입
    public void Exit();     // 상태 끝
    public void HandleInput();  // 새로운 입력값이 들어오면 이벤트를 추가, 삭제
    public void Update();   // 상태 업데이트
    public void PhysicsUpdate();    // 물리 업데이트(중력 관련) 
}

public abstract class PlayerBaseState : IState
{
    protected BaseController controller;    // PlayerController, 다른 npc의 controller
    protected PlayerStateMachine stateMachine;    // PlayerStateMachine
    protected readonly PlayerStatHandler statHandler;   // PlayerStatHandler에 있는 데이터를 읽기만 한다
    protected Player player;

    // 임시(동적연결)
    public InputManager inputManager;

    public PlayerBaseState(PlayerController controller, PlayerStateMachine stateMachine)
    {
        this.controller = controller;
        this.stateMachine = stateMachine;
        statHandler = stateMachine.Player.statHandler;
        player = stateMachine.Player;
        inputManager = player.Input.GetInputManager();
        stateMachine.Player.playerAnimator = player.playerAnimator;
    }

    public virtual void Enter() 
    {
        AddInputActionsCallbacks(); // 이벤트 등록
    }

    public virtual void Exit() 
    {
        RemoveInputActionsCallbacks();
    }

    public virtual void HandleInput() 
    {
        ReadMovementInput();    // 입력 값을 계속 확인
    }
    public virtual void Update()
    {
    }
    public virtual void PhysicsUpdate()
    {
    }

    #region 상태 관련 이벤트#
    // 모든 상태에 공통인 메서드
    // 상태 진입 시에 이벤트 추가
    protected virtual void AddInputActionsCallbacks()
    {
        // 상태에 진입했을 때 추가한다
        inputManager.GetInput(EPlayerInput.move).started += OnMovementCanceled;
        inputManager.GetInput(EPlayerInput.run).started += OnRunStarted;
    }
    // 이벤트 해제
    protected virtual void RemoveInputActionsCallbacks()
    {
        // 상태를 빠져나갈 때 추가한다
        inputManager.GetInput(EPlayerInput.move).canceled -= OnMovementCanceled;
        inputManager.GetInput(EPlayerInput.run).canceled -= OnRunStarted;

    }
    // 키를 떼었을 때 
    protected virtual void OnMovementCanceled(InputAction.CallbackContext context)
    {
        // 각각 상태에서 구현
    }
    // 키를 입력했을 때
    protected virtual void OnRunStarted(InputAction.CallbackContext context)
    {
        // 각각 상태에서 구현
    }
    #endregion

    #region 애니메이션 교체
    // 애니메이션 교체
    // bool 애니메이션 설정
    protected void StartAnimation(int animatorHash)
    {
        stateMachine.Player.playerAnimator.SetBool(animatorHash, true);
    }
    protected void StopAnimation(int animatorHash)
    {
        stateMachine.Player.playerAnimator.SetBool(animatorHash, false);
    }
    // Trigger 애니메이션 설정
    protected void SetTriggerAnimation(int animatorHash)
    {
        stateMachine.Player.playerAnimator.SetTrigger(animatorHash);
    }
    // MoveX, MoveZ
    protected void SetAnimationFloat(int animatorHash, float value)
    {
        stateMachine.Player.playerAnimator.SetFloat(animatorHash, value);
    }

    #endregion
    // 모든 상태는 입력값을 받는다
    // WASD 입력
    private void ReadMovementInput()
    {
        // Input
        stateMachine.InputHandler.GetMoveInput();
    }
}
