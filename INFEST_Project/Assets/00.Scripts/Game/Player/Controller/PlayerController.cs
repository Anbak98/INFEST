using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using UnityEngine.InputSystem;
using UnityEngine.Playables;
using UnityEngine.InputSystem.XR;
using UnityEngine.Windows;


/// <summary>
/// 캐릭터 동작 처리를 한다
/// 상태 → 행동(움직임, 애니메이션 등)
/// 
/// InputAction의 이벤트메서드를 연결한다
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
/// 
/// 카메라도 여기에서 업데이트(?)
/// 
/// 플레이어가 StateMachine으로 상태를 바꾸는 것을 Controller에서 
/// 또한 상태를 서버에 보내는 것도 여기에서 따로 
/// 
/// BaseController의 상속을 받는 방식으로 바꾸었다
/// </summary>
public abstract class PlayerController : BaseController
{
    //private PlayerStateMachine stateMachine;

    /// <summary>
    /// 플레이어가 입력을 받으면 다음 2가지 로직을 수행(2가지 동작은 별개의 동작이다)
    /// 1.서버로 보내야 하니 PlayerInputHandler에 저장
    /// 2.State 변경을 위해 가져온다 
    /// 여기서 받아와서 
    /// 상태를 업데이트 한 후 
    /// NetworkInputData에 상태를 보낸 다음
    /// 
    /// PlayerInputSender에서 최종으로 확인 후 서버로 보낸다
    /// </summary>
    //public InputManager inputManager;

    /// <summary>
    /// 상속으로 구현한 값 가져오는 메서드들이 플레이어 프리팹의 최상위 부모에 붙어야 하므로
    /// 나중에는 PlayerInputHandler에 옮겨야 한다
    /// 각각 1인칭 프리팹과 3인칭 프리팹에 붙는 것들은 animator와 statemachine만 가지고 있어야한다
    /// </summary>
    public PlayerInputHandler inputHandler;

    protected CharacterController controller;

    // FSM 상태 머신 인스턴스
    protected float verticalVelocity;
    protected float gravity = -9.81f;

    public string playerId;
    protected bool hitSuccess;
    protected string hitTatgetId;

    protected override void Awake()
    {
        // inputManager에 더 잘 연결하는 방법을 생각해보자
        //if (inputManager == null)
        //    inputManager = FindObjectOfType<InputManager>();

        controller = GetComponentInParent<CharacterController>();
    }

    // 네트워크에서 받은 상태를 반영한다
    public override void ApplyNetworkState(PlayerStatData data)
    {

    }

    #region 상태 변화 조건(PlayerInputHandler의 값을 가져와서 판단)
    // 1인칭 애니메이션은 LocalPlayerController 3인칭 애니메이션은 RemoteController에서 조작하지만
    // 1인칭, 3인칭 공통으로 처리하는 것은 여기에서 관리    
    public override bool HasMoveInput() => inputHandler.MoveInput.sqrMagnitude > 0.01f;
    public override bool IsJumpInput() => inputHandler.GetIsJumping();
    public override bool IsFiring() => inputHandler.GetIsFiring();
    public override bool IsGrounded() => controller.isGrounded;
    public override bool IsShotgunFiring() => inputHandler.GetShotgunIsOnFiring();
    public override float GetVerticalVelocity() => verticalVelocity;    
    public override void ApplyGravity() { } 
    #endregion

    // 네트워크에서 받은 값에 따라 행동하는 메서드 만든다
    public void OnMove()
    {
        //Vector3 input = _input.MoveInput;

        ///*Vector3 move = head.right * input.x + head.forward * input.z;
        //_controller.Move(move * _statHandler.MoveSpeed * Time.deltaTime);*/

        //Vector3 forward = transform.forward;
        //Vector3 right = transform.right;

        //Vector3 move = right * input.x + forward * input.z;
        //move.y = 0f; // 수직 방향 제거
        //_controller.Move(move.normalized * _statHandler.MoveSpeed * Time.deltaTime);
    }


}
