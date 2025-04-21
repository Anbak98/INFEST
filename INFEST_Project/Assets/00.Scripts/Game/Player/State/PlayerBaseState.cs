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
    protected PlayerController controller;    // PlayerController, 다른 npc의 controller
    protected PlayerStateMachine stateMachine;    // PlayerStateMachine
    protected readonly PlayerStatHandler statHandler;   // PlayerStatHandler에 있는 데이터를 읽기만 한다
    protected Player player;
    // 지금은 플레이어의 카메라만 있으니 그냥 하지만
    // 나중에는 다른 카메라(조준에 사용할)를 포함하여 CameraHandler를 통해 카메라에 접근할 수 있음

    public Transform MainCameraTransform { get; set; }


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

        MainCameraTransform = Camera.main.transform;
    }

    public virtual void Enter()
    {
        AddInputActionsCallbacks(); // BaseState에 들어올 때 이벤트 등록
    }

    public virtual void Exit()
    {
        RemoveInputActionsCallbacks();  // 이벤트 해제
    }

    // 입력 값을 확인
    public virtual void HandleInput()
    {
        // StateMachine에서 호출하는 HandleInput는
        // BaseState의  ReadMovementInput를 호출하는데
        // ReadMovementInput는 여기에서만 호출한다
        // 이거 Controller의 HandleMovement가 될 수 없냐는거지
        // 중력도 마찬가지 ApplyGravity

        ReadMovementInput();    // 이동
        // 달리기
        // 점프
        // 사격
        // 앉기

    }
    public virtual void Update()
    {
    }
    public virtual void PhysicsUpdate()
    {
    }

    #region 상태 관련 이벤트#
    // 모든 상태에 공통인 메서드
    // 각각 클래스에서 사용할 이벤트를 상태 진입 시에 추가
    protected virtual void AddInputActionsCallbacks()
    {
        // 상태에 진입했을 때 추가한다
        //inputManager.GetInput(EPlayerInput.move).started += OnMovementCanceled; // 키를 해제했을 때
        //inputManager.GetInput(EPlayerInput.move).started += OnMoveStarted; // 키를 해제했을 때
        //inputManager.GetInput(EPlayerInput.run).started += OnRunStarted;    // 키를 눌렀을 때
        //inputManager.GetInput(EPlayerInput.look).started += OnLookStarted;

        //inputManager.GetInput(EPlayerInput.fire).started += OnAttack;
        //inputManager.GetInput(EPlayerInput.reload).started += OnReload;

        //inputManager.GetInput(EPlayerInput.sit).started += OnSitStarted;

        //inputManager.GetInput(EPlayerInput.jump).started += OnJumpStarted;
    }
    // 이벤트 해제
    protected virtual void RemoveInputActionsCallbacks()
    {
        // 상태를 빠져나갈 때 추가한다
        //inputManager.GetInput(EPlayerInput.move).canceled -= OnMoveStarted;
        //inputManager.GetInput(EPlayerInput.run).canceled -= OnRunStarted;

        //inputManager.GetInput(EPlayerInput.look).canceled += OnLookStarted;

        //inputManager.GetInput(EPlayerInput.fire).canceled += OnAttack;
        //inputManager.GetInput(EPlayerInput.reload).canceled += OnReload;

        //inputManager.GetInput(EPlayerInput.sit).canceled += OnSitStarted;

        //inputManager.GetInput(EPlayerInput.jump).canceled += OnJumpStarted;
    }

    // 각각 다음 상태로 가능한 경우에 구현한다
    protected virtual void OnMovementCanceled(InputAction.CallbackContext context)
    {
        // 방향키 이동을 해제했다면 Idle로 바꾼다
    }
    protected virtual void OnMoveStarted(InputAction.CallbackContext context)
    {
    }

    protected virtual void OnRunStarted(InputAction.CallbackContext context)
    {
    }
    //protected virtual void OnLookStarted(InputAction.CallbackContext context)
    //{
    //    //Vector2 lookDelta = context.ReadValue<Vector2>();
    //}
    protected virtual void OnAttack(InputAction.CallbackContext context)
    {
    }
    protected virtual void OnReload(InputAction.CallbackContext context)
    {
    }
    protected virtual void OnJumpStarted(InputAction.CallbackContext context)
    {
    }
    protected virtual void OnSitStarted(InputAction.CallbackContext context)
    {
    }
    protected virtual void OnWaddleStarted(InputAction.CallbackContext context)
    {
    }
    protected virtual void OnSitReloadStarted(InputAction.CallbackContext context)
    {
    }
    protected virtual void OnSitAttackStarted(InputAction.CallbackContext context)
    {
    }

    #endregion
    #region 애니메이션 교체
    // bool 파라미터
    protected void StartAnimation(int animatorHash)
    {
        stateMachine.Player.playerAnimator.SetBool(animatorHash, true);
    }
    protected void StopAnimation(int animatorHash)
    {
        stateMachine.Player.playerAnimator.SetBool(animatorHash, false);
    }
    // Trigger 파리미터
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

    #region 입력값 읽기
    // 모든 상태는 입력값을 받는다
    // WASD 입력
    // 이건 BaseState에서만 호출하고있다
    private void ReadMovementInput()
    {
        // Input
        stateMachine.InputHandler.GetMoveInput();
    }
    private void ReadBoolInput()
    {
        stateMachine.InputHandler.GetIsJumping();
        // 이런식으로 다 들고와야할까
    }

    #endregion
    #region 애니메이션이 있는 것들(이동, 달리기, 사격, 점프, 앉기, 앉아서 이동, 조준)의 실제 동작
    protected void PlayerMove()
    {
        // 카메라의 회전방향(CameraHandler의 Update에서 실시간으로 업데이트)으로 이동한다
        controller.HandleMovement();    // 이동
        controller.ApplyGravity();  // 중력
    }
    

    protected void PlayerFire()
    {
        Debug.Log("Fire");
        // 발사로직 PlayerController에 옮길것
    }
    protected void PlayerRun()
    {
        Debug.Log("Run");
        // 카메라의 회전방향(CameraHandler의 Update에서 실시간으로 업데이트)으로 이동한다
        controller.HandleMovement();    // 이동
        controller.ApplyGravity();  // 중력

    }
    protected void PlayerJump()
    {
        Debug.Log("Jump");

        // Junp 키입력하면 내부에서 1번만 y축 힘받고 그 외는 땅에 닿을 때까지 중력만 받을것이다
        controller.StartJump();
    }
    protected void PlayerSit()
    {
        Debug.Log("Sit");

    }
    // 앉아서 걷기
    protected void PlayerWaddle()
    {
        Debug.Log("Waddle");
        // 카메라의 회전방향(CameraHandler의 Update에서 실시간으로 업데이트)으로 이동한다
        controller.HandleMovement();    // 이동
        controller.ApplyGravity();  // 중력

    }
    // 조준(애니메이션은 바꾸고, 카메라를 따로 조작)
    protected void PlayerZoom()
    {
        Debug.Log("Zoom");

    }
    #endregion
}
