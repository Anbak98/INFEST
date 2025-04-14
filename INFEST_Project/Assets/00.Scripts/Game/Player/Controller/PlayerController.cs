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
public class PlayerController : BaseController
{
    //private PlayerStateMachine stateMachine;
    public Player player;

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
    public InputManager inputManager;

    public PlayerInputHandler inputHandler;

    protected CharacterController characterController;
    

    private void Awake()
    {
        // inputManager에 더 잘 연결하는 방법을 생각해보자
        if (inputManager == null)
            inputManager = FindObjectOfType<InputManager>();
    }

    // Start is called before the first frame update
    void Start()
    {
        //BindInputActions();
    }
    /// <summary>
    /// 오직 여기에서만 네트워크로부터 플레이어의 업데이트를 받는다
    /// </summary>
    public override void FixedUpdateNetwork()
    {
        Debug.Log("FixedUpdateNetwork 진입");
        if (GetInput(out NetworkInputData input))
        {
            // 테스트용으로 플레이어를 이동시킨다

            //// 입력을 현재 상태에 전달
            //stateMachine.currentState.HandleInput(input);

            //// 상태 업데이트
            //stateMachine.currentState.UpdateLogic();
        }
    }

    // 네트워크에서 받은 상태를 반영한다
    public void ApplyNetworkState()
    {

    }

    #region 상태 변화 조건(PlayerInputHandler의 값을 가져와서 판단)
    // 1인칭 애니메이션은 LocalPlayerController 3인칭 애니메이션은 RemoteController에서 조작하지만
    // 같은 종류의 애니메이션이어야 하므로 같은 변수를 공유한다
    // 이동
    public bool HasMoveInput() => inputHandler.MoveInput.sqrMagnitude > 0.01f;
    // 점프
    public bool IsJumpInput() => inputHandler.GetIsJumping();
    // 사격
    public bool IsFiring() => inputHandler.GetIsFiring();

    //현재 캐릭터가 땅 위에 있는지
    //public bool IsGrounded() => 

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
