using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using UnityEngine.InputSystem;

/// <summary>
/// 캐릭터 동작 처리를 한다
/// 상태 → 행동(움직임, 애니메이션 등)
/// 
/// 
/// InputAction의 이벤트메서드를 연결한다
/// 
/// 플레이어의 FSM은 네트워크에서 동기화된 입력 데이터를 기반으로 상태 전환
/// 
/// 플레이어의 동작 및 상태 관리
/// 실제 게임 내 플레이어 캐릭터의 동작을 제어하는 역할.
/// 이동, 점프, 공격 등 물리적 동작을 구현하며, 이를 네트워크 상태에 반영.
/// FixedUpdateNetwork()에서 Fusion으로부터 받은 입력 데이터를 기반으로 시뮬레이션 수행.
/// 
/// 물리 계산 및 캐릭터 상태 업데이트.
/// 네트워크로부터 받은 최종 상태를 반영하여 클라이언트 화면에 표시.
/// 애니메이션 및 시각적 효과 처리.
/// </summary>
public class PlayerController : MonoBehaviour
{
    [SerializeField] private PlayerInputHandler inputHandler;
    [SerializeField] private CharacterController characterController;
    [SerializeField] private float moveSpeed = 5f;

    // Start is called before the first frame update
    void Start()
    {
        //BindInputActions();
    }




    // InputAction과 그에 맞는 이벤트를 연결한다

    //private void BindInputActions()
    //{
    //    var actionMap = inputManager;

    //    actionMap.GetInput(EPlayerInput.move).performed += OnMovePerformed;
    //    actionMap.GetInput(EPlayerInput.move).canceled += OnMoveCanceled;

    //    actionMap.GetInput(EPlayerInput.look).performed += OnLookPerformed;
    //    actionMap.GetInput(EPlayerInput.look).canceled += OnLookCanceled;

    //    actionMap.GetInput(EPlayerInput.jump).performed += OnJump;

    //    actionMap.GetInput(EPlayerInput.run).performed += context => IsRunning = true;
    //    actionMap.GetInput(EPlayerInput.run).canceled += context => IsRunning = false;

    //    actionMap.GetInput(EPlayerInput.fire).performed += context => IsFiring = true;
    //    actionMap.GetInput(EPlayerInput.fire).canceled += context => IsFiring = false;

    //    actionMap.GetInput(EPlayerInput.zoom).performed += context => IsZooming = true;
    //    actionMap.GetInput(EPlayerInput.zoom).canceled += context => IsZooming = false;

    //    actionMap.GetInput(EPlayerInput.reload).performed += OnReload;

    //    actionMap.GetInput(EPlayerInput.sit).performed += context => IsSitting = true;
    //    actionMap.GetInput(EPlayerInput.sit).canceled += context => IsSitting = false;

    //    actionMap.GetInput(EPlayerInput.scoreboard).performed += context => IsScoreBoardPopup = true;
    //    actionMap.GetInput(EPlayerInput.scoreboard).canceled += context => IsScoreBoardPopup = false;
    //}

    //// === 각 입력 처리 메서드 ===
    //// Move
    //private void OnMovePerformed(InputAction.CallbackContext context)
    //{
    //    Vector2 input = context.ReadValue<Vector2>();
    //    MoveInput = new Vector3(input.x, 0, input.y);
    //}
    //private void OnMoveCanceled(InputAction.CallbackContext context)
    //{
    //    MoveInput = Vector3.zero;
    //}

    //// Look
    //private void OnLookPerformed(InputAction.CallbackContext context)
    //{
    //    Vector2 input = context.ReadValue<Vector2>();
    //    RotationX = input.x;
    //    RotationY = input.y;
    //}
    //private void OnLookCanceled(InputAction.CallbackContext context)
    //{
    //    RotationX = 0f;
    //    RotationY = 0f;
    //}

    //// Jump
    //private void OnJump(InputAction.CallbackContext context)
    //{
    //    IsJumping = true;
    //}

    //// Reload
    //private void OnReload(InputAction.CallbackContext context)
    //{
    //    IsReloading = true;
    //}




}
