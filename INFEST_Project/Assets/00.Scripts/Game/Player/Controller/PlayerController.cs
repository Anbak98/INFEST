using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using UnityEngine.InputSystem;
using UnityEngine.Playables;
using UnityEngine.InputSystem.XR;
using UnityEngine.Windows;
//using static UnityEditor.Experimental.GraphView.GraphView;


/// <summary>
/// 캐릭터 동작 처리를 한다
/// 
/// InputAction의 이벤트메서드를 연결한다
/// 플레이어의 FSM은 네트워크에서 동기화된 입력 데이터를 기반으로 상태 전환
/// 
/// 플레이어의 동작 및 상태 관리
/// FixedUpdateNetwork()에서 Fusion으로부터 받은 입력 데이터를 기반으로 시뮬레이션 수행.
/// </summary>
public abstract class PlayerController : BaseController
{
    // 동적 연결되는 변수 숨기기
    [HideInInspector]
    public Player player;

    public Weapons weapons;

    /// <summary>
    /// 플레이어가 입력을 받으면 다음 2가지 로직을 수행(2가지 동작은 별개의 동작이다)
    /// 1.서버로 보내야 하니 PlayerInputHandler에 저장
    /// 2.PlayerInputSender에서 최종으로 확인 후 서버로 보낸다
    /// 3.FixedUpdateNetwork에서 값을 받아서 사용한다
    /// </summary>

    //public PlayerInputHandler inputHandler;
    public PlayerCameraHandler cameraHandler;
    //public Transform MainCameraTransform { get; set; }

    //protected CharacterController controller;

    // FSM 상태 머신 인스턴스
    protected float verticalVelocity;
    protected float gravity = -9.81f;

    public string playerId;
    protected bool hitSuccess;
    protected string hitTatgetId;

    public override void Awake()
    {
        player = GetComponentInParent<Player>();    // 플레이어 먼저 생성

        weapons = player.GetWeapons();

        //inputHandler = player.Input;
        // inputManager에 더 잘 연결하는 방법을 생각해보자
        //if (inputManager == null)
        //    inputManager = FindObjectOfType<InputManager>();

        //controller = GetComponentInParent<CharacterController>();

        stateMachine = new PlayerStateMachine(player, this);

        //MainCameraTransform = Camera.main.transform;
    }

    //public override void FixedUpdateNetwork()
    //{
    //    //base.FixedUpdateNetwork();

    //    if (GetInput(out NetworkInputData data))
    //    {
    //        // 상태머신 
    //        //stateMachine.HandleInput();
    //        stateMachine.OnUpdate();
    //    }
    //}

    //// 점프 눌렸나
    //public override bool IsJumpInput() => player.Input.GetIsJumping();
    //public override bool IsSitInput() => player.Input.GetIsSitting();

    //// 플레이어가 땅 위에 있는지?
    ////public override bool IsGrounded() => player.characterController.isGrounded;
    //public override bool IsGrounded() => player.networkCharacterController.Grounded;
    //public override float GetVerticalVelocity() => verticalVelocity;

    //// 플레이어의 이동(방향은 CameraHandler에서 설정) 처리. 그 방향이 transform.forward로 이미 설정되었다
    //public override void HandleMovement()
    //{
    //    //Vector3 input = player.Input.MoveInput;
    //    //Vector3 forward = transform.forward;
    //    //Vector3 right = transform.right;

    //    //Vector3 move = right * input.x + forward * input.z;
    //    //move.y = 0f; // 수직 방향 제거
    //    //player.characterController.Move(move.normalized * player.statHandler.MoveSpeed * player.statHandler.MoveSpeedModifier * Time.deltaTime);


    //    if (GetInput(out NetworkInputData data))
    //    {
    //        Vector3 input = data.direction;

    //        // ❗ 입력 없으면 아무 것도 하지 않음
    //        if (input.sqrMagnitude < 0.01f) return;

    //        // 카메라 기준 방향 가져오기
    //        Vector3 camForward = player.cameraHandler.GetCameraForwardOnXZ();
    //        Vector3 camRight = player.cameraHandler.GetCameraRightOnXZ();

    //        Vector3 moveDir = (camRight * input.x + camForward * input.z).normalized;
    //        moveDir.y = 0f; // 수직 방향 제거

    //        //player.networkCharacterController.Move(camForward * player.statHandler.MoveSpeed * player.statHandler.MoveSpeedModifier * Time.deltaTime);

    //        // 회전은 막고, 이동만 한다
    //        player.networkCharacterController.Move(
    //            moveDir * player.statHandler.MoveSpeed * player.statHandler.MoveSpeedModifier * Time.deltaTime
    //        );

    //        // 회전 강제 고정: 카메라가 지정한 forward로
    //        player.transform.forward = camForward;
    //    }
    //}
    //public override void ApplyGravity()
    //{
    //    // TODO
    //    if (IsGrounded() && verticalVelocity < 0)
    //    {
    //        verticalVelocity = -2f;
    //    }
    //    else
    //    {
    //        verticalVelocity += gravity * Time.deltaTime;
    //    }
    //    Vector3 gravityMove = new Vector3(0f, verticalVelocity, 0f);
    //    player.characterController.Move(gravityMove * Time.deltaTime);
    //}
    ///// <summary>
    ///// 점프 시작 시 수직 속도 계산
    ///// </summary>
    //public override void StartJump()
    //{
    //    verticalVelocity = Mathf.Sqrt(player.statHandler.JumpPower * -2f * gravity);
    //}

    //// 앉는다
    //public override void StartSit()
    //{
    //    // collider는 상태에서 변화시키므로 여기서는 transform만 아래로
    //    float playerYpos = player.transform.position.y;
    //    playerYpos /= 2;
    //    player.transform.position = new Vector3(player.transform.position.x, playerYpos, player.transform.position.z);
    //}
    //// 일어난다
    //public override void StartStand()
    //{
    //    // collider는 상태에서 변화시키므로 여기서는 transform만 아래로
    //    float playerYpos = player.transform.position.y;
    //    playerYpos *= 2;
    //    player.transform.position = new Vector3(player.transform.position.x, playerYpos, player.transform.position.z);
    //}


    //public override void StartFire()
    //{
    //    if (GetInput(out NetworkInputData data))
    //    {
    //        // 네트워크 객체는 StateAuthority(호스트)만 생성할 수 있기 때문에 StateAuthority에 대한 확인이 필요
    //        // 호스트에서만 실행되고 클라이언트에서는 예측되지 않는다
    //        if (HasStateAuthority && delay.ExpiredOrNotRunning(Runner))
    //        {
    //            // 마우스 좌클릭(공격)
    //            if (data.buttons.IsSet(NetworkInputData.BUTTON_FIRE))
    //            {
    //                //Debug.Log("공격");
    //                weapons.Fire(data.buttons.IsSet(NetworkInputData.BUTTON_FIREPRESSED));

    //                delay = TickTimer.CreateFromSeconds(Runner, 0.5f);
    //            }
    //        }
    //    }
    //}

    //public override void StartReload()
    //{
    //    // TODO
    //}



}
