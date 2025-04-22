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
    /// 2.State 변경을 위해 가져온다 
    /// 여기서 받아와서 
    /// 상태를 업데이트 한 후 
    /// NetworkInputData에 상태를 보낸 다음
    /// 
    /// PlayerInputSender에서 최종으로 확인 후 서버로 보낸다
    /// </summary>

    /// <summary>
    /// 상속으로 구현한 값 가져오는 메서드들이 플레이어 프리팹의 최상위 부모에 붙어야 하므로
    /// 나중에는 PlayerInputHandler에 옮겨야 한다
    /// 각각 1인칭 프리팹과 3인칭 프리팹에 붙는 것들은 animator와 statemachine만 가지고 있어야한다
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

    //public override void Update()
    //{
    //    // 상태머신 
    //    stateMachine.HandleInput();
    //    stateMachine.Update();
    //}

    public override void FixedUpdateNetwork()
    {
        //base.FixedUpdateNetwork();


        // 상태머신 
        stateMachine.HandleInput();
        stateMachine.Update();
    }


    //#region 상태 변화 조건(PlayerInputHandler의 값을 가져와서 판단)
    //// 1인칭 애니메이션은 LocalPlayerController 3인칭 애니메이션은 RemoteController에서 조작하지만
    //// 1인칭, 3인칭 공통으로 처리하는 것은 여기에서 관리    
    //public override bool HasMoveInput() => inputHandler.MoveInput.sqrMagnitude > 0.01f;
    //public override bool IsJumpInput() => inputHandler.GetIsJumping();
    //public override bool IsFiring() => inputHandler.GetIsFiring();
    ////public override bool IsGrounded() => controller.isGrounded;
    //public override bool IsShotgunFiring() => inputHandler.GetShotgunIsOnFiring();
    //public override float GetVerticalVelocity() => verticalVelocity;
    //public override void ApplyGravity() { }
    //#endregion

    // 점프 눌렸나
    public override bool IsJumpInput() => player.Input.GetIsJumping();
    public override bool IsSitInput() => player.Input.GetIsSitting();

    // 플레이어가 땅 위에 있는지?
    public override bool IsGrounded() => player.characterController.isGrounded;
    public override float GetVerticalVelocity() => verticalVelocity;



    // Remote 플레이어는 직접 이동하지 않는다 (서버로부터 받는 정보를 신뢰)

    // 플레이어의 이동(방향은 CameraHandler에서 설정) 처리
    public override void HandleMovement()
    {
        Vector3 input = player.Input.MoveInput;

        Vector3 forward = transform.forward;
        Vector3 right = transform.right;

        Vector3 move = right * input.x + forward * input.z;
        move.y = 0f; // 수직 방향 제거
        player.characterController.Move(move.normalized * player.statHandler.MoveSpeed * player.statHandler.MoveSpeedModifier * Time.deltaTime);
    }
    public override void ApplyGravity()
    {
        // TODO
        if (IsGrounded() && verticalVelocity < 0)
        {
            verticalVelocity = -2f;
        }
        else
        {
            verticalVelocity += gravity * Time.deltaTime;
        }
        Vector3 gravityMove = new Vector3(0f, verticalVelocity, 0f);
        player.characterController.Move(gravityMove * Time.deltaTime);
    }
    /// <summary>
    /// 점프 시작 시 수직 속도 계산
    /// </summary>
    public override void StartJump()
    {
        verticalVelocity = Mathf.Sqrt(player.statHandler.JumpPower * -2f * gravity);
    }

    // 앉는다
    public override void StartSit()
    {
        // collider는 상태에서 변화시키므로 여기서는 transform만 아래로
        float playerYpos = player.transform.position.y;
        playerYpos /= 2;
        player.transform.position = new Vector3(player.transform.position.x, playerYpos, player.transform.position.z);
    }
    // 일어난다
    public override void StartStand()
    {
        // collider는 상태에서 변화시키므로 여기서는 transform만 아래로
        float playerYpos = player.transform.position.y;
        playerYpos *= 2;
        player.transform.position = new Vector3(player.transform.position.x, playerYpos, player.transform.position.z);
    }


    public override void StartFire()
    {
        if (GetInput(out NetworkInputData data))
        {
            // 네트워크 객체는 StateAuthority(호스트)만 생성할 수 있기 때문에 StateAuthority에 대한 확인이 필요
            // 호스트에서만 실행되고 클라이언트에서는 예측되지 않는다
            if (HasStateAuthority && delay.ExpiredOrNotRunning(Runner))
            {
                // 마우스 좌클릭(공격)
                if (data.buttons.IsSet(NetworkInputData.BUTTON_FIRE))
                {
                    //Debug.Log("공격");
                    weapons.Fire(data.buttons.IsSet(NetworkInputData.BUTTON_FIREPRESSED));

                    delay = TickTimer.CreateFromSeconds(Runner, 0.5f);
                }
            }
        }
    }

    public override void StartReload()
    {
        // TODO
    }



}
