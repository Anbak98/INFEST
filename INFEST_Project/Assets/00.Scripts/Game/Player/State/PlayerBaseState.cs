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
        AddInputActionsCallbacks(); // 이벤트 등록
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

        inputManager.GetInput(EPlayerInput.look).started += OnLookStarted;

        inputManager.GetInput(EPlayerInput.fire).started += OnAttack;
        inputManager.GetInput(EPlayerInput.reload).started += OnReload;

        inputManager.GetInput(EPlayerInput.sit).started += OnSitStarted;

        inputManager.GetInput(EPlayerInput.jump).started += OnJumpStarted;
    }
    // 이벤트 해제
    protected virtual void RemoveInputActionsCallbacks()
    {
        // 상태를 빠져나갈 때 추가한다
        inputManager.GetInput(EPlayerInput.move).canceled -= OnMovementCanceled;
        inputManager.GetInput(EPlayerInput.run).canceled -= OnRunStarted;

        inputManager.GetInput(EPlayerInput.look).canceled += OnLookStarted;

        inputManager.GetInput(EPlayerInput.fire).canceled += OnAttack;
        inputManager.GetInput(EPlayerInput.reload).canceled += OnReload;

        inputManager.GetInput(EPlayerInput.sit).canceled += OnSitStarted;

        inputManager.GetInput(EPlayerInput.jump).canceled += OnJumpStarted;
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
    protected virtual void OnLookStarted(InputAction.CallbackContext context)
    {
        Vector2 lookDelta = context.ReadValue<Vector2>();

        //MainCameraTransform.UpdateRotate(lookDelta.x, lookDelta.y);
    }
    protected virtual void OnAttack(InputAction.CallbackContext context)
    {

    }
    protected virtual void OnReload(InputAction.CallbackContext context)
    {

    }
    // 점프 키 입력
    protected virtual void OnJumpStarted(InputAction.CallbackContext context)
    {

    }


    protected virtual void OnSitStarted(InputAction.CallbackContext context)
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

    // 모든 상태는 입력값을 받는다
    // WASD 입력
    // 이건 BaseState에서만 호출하고있다
    private void ReadMovementInput()
    {
        // Input
        stateMachine.InputHandler.GetMoveInput();
    }

    #region 이동
    protected void PlayerMove()
    {
        //Vector3 movementDir = GetMovementDir();
        //Move(movementDir);
        //Rotate(movementDir);

        // 카메라의 회전방향(CameraHandler의 Update에서 실시간으로 업데이트)으로 이동한다
        controller.HandleMovement();
        controller.ApplyGravity();
    }
    //private Vector3 GetMovementDir()
    //{
    //    // 메인 카메라가 바라보는 방향 = 플레이어가 바라보는 방향
    //    Transform mainCameraTr = MainCameraTransform.transform;
    //    Vector3 forward = mainCameraTr.forward;
    //    Vector3 right = mainCameraTr.right;

    //    forward.y = 0;
    //    right.y = 0;

    //    forward.Normalize();
    //    right.Normalize();

    //    return forward * inputHandler.GetMoveInput().y + right * inputHandler.GetMoveInput().x;
    //}
    //private void Move(Vector3 dir)
    //{
    //    int movementSpeed = GetMovementSpeed();
    //    player.characterController.Move((dir * movementSpeed) * Time.deltaTime);
    //}
    //// 이동속도 가져오기  
    //private int GetMovementSpeed()
    //{
    //    player.stateMachine.StatHandler.MoveSpeedModifier = 2;  // 나중에 리팩토링때 제거

    //    //int moveSpeed = GetStateMachine<PlayerStateMachine>().StatHandler.MoveSpeed * GetStateMachine<PlayerStateMachine>().StatHandler.MoveSpeedModifier;
    //    int moveSpeed = player.stateMachine.StatHandler.MoveSpeed * player.stateMachine.StatHandler.MoveSpeedModifier;
    //    return moveSpeed;
    //}
    #endregion
    //#region 회전
    //private void Rotate(Vector3 dir)
    //{
    //    if (dir != Vector3.zero)
    //    {
    //        Transform playerTr = player.transform;
    //        Quaternion targetRotation = Quaternion.LookRotation(dir);
    //        playerTr.rotation = Quaternion.Slerp(playerTr.rotation, targetRotation, player.statHandler.RotationDamping * Time.deltaTime);
    //    }
    //}
    //#endregion

}
