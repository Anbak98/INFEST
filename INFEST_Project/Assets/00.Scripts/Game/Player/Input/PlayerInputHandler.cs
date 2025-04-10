using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.EventSystems.StandaloneInputModule;


/// <summary>
/// 입력 감지 및 저장
/// InputSystem → 상태 저장
/// (참고: 수신하는 곳에서 INetworkRunnerCallbacks의 OnInput을 구현해야하며, 
/// 
/// 각 플레이어의 입력을 개별적으로 처리하는 역할
/// InputManager에서 전달받은 데이터를 플레이어 컨텍스트에 맞게 변환
/// 플레이어의 입력을 받아서 저장
/// 
/// 플레이어별 입력 데이터 변환 및 네트워크와 로컬 동기화 
/// 
/// PlayerInputHandler.cs (개별 플레이어 입력 핸들링)​
/// 각 버튼/이동/시선 입력을 받아서 내부 상태 변수로 저장.
/// GetNetworkInput()으로 Fusion 입력 구조체 생성.
/// SetFusionNetworkRunner()를 통해 Fusion 네트워크 전달.
/// 장점: 네트워크 데이터 구조와 잘 연동, 깔끔하게 분리됨.
/// </summary>
public class PlayerInputHandler : NetworkBehaviour
{
    /// <summary>
    /// **속성(Property)**는 참조(ref)로 전달할 수 없으므로, ref 전달 하려면 필드로 바꿔라
    /// </summary>
    public Vector3 MoveInput { get; private set; } = Vector3.zero;      // 이동 방향 (Vector3)

    public float RotationX { get; private set; } = 0f;       // 마우스 X 회전 (pitch)
    public float RotationY { get; private set; } = 0f;       // 마우스 Y 회전 (yaw)

    //public bool IsJumping { get; private set; } = false;    // jump
    //public bool IsReloading { get; private set; } = false;  // reload
    //public bool IsFiring { get; private set; } = false;     // fire
    //public bool IsZooming { get; private set; } = false;    // zoom
    //public bool IsInteracting { get; private set; } = false;    // interaction
    //public bool IsUsingItem { get; private set; } = false;  // useItem
    //public bool IsRunning { get; private set; } = false;    // run
    //public bool IsSitting { get; private set; } = false;    // sit
    //public bool IsScoreBoardPopup { get; private set; } = false;    // scoreboard

    private bool _isJumping;
    private bool _isReloading;
    private bool _isFiring;
    private bool _isZooming;
    private bool _isInteracting;
    private bool _isUsingItem;
    private bool _isRunning;
    private bool _isSitting;
    private bool _isScoreBoardPopup;

    // 읽기 전용 속성
    public bool IsJumping => _isJumping; 
    public bool IsReloading => _isReloading;
    public bool IsFiring => _isFiring;
    public bool IsZooming => _isZooming;
    public bool IsInteracting => _isInteracting;
    public bool IsUsingItem => _isUsingItem;
    public bool IsRunning => _isRunning;
    public bool IsSitting => _isSitting;
    public bool IsScoreBoardPopup => _isScoreBoardPopup;


    // 임시(동적연결)
    public InputManager inputManager;

    private void Start()
    {
        inputManager = FindAnyObjectByType<InputManager>();

        if (inputManager == null)
        {
            Debug.LogError("InputManager not assigned.");
            return;
        }
        // InputManager에서 각 입력 액션을 구독
        SubscribeToInputs();
    }
    /// <summary>
    /// 네트워크 입력 만들기
    /// PlayerInputSender에서 호출하여 저장된 입력을 서버로 전송한다
    /// </summary>
    /// <returns></returns>
    public NetworkInputData? GetNetworkInput()
    {
        // 입력 없으면 null 반환
        if (MoveInput == Vector3.zero && !_isJumping && !_isFiring && !_isReloading && !_isZooming && !_isRunning)
            return null;

        var data = new NetworkInputData
        {
            direction = MoveInput,
            lookDelta = new Vector2(RotationX, RotationY),
        };

        if (_isRunning) data.buttons.Set(NetworkInputData.BUTTON_RUN, true);
        if (_isJumping) data.buttons.Set(NetworkInputData.BUTTON_JUMP, true);
        if (_isFiring) data.buttons.Set(NetworkInputData.BUTTON_FIRE, true);
        if (_isZooming) data.buttons.Set(NetworkInputData.BUTTON_ZOOM, true);
        if (_isReloading) data.buttons.Set(NetworkInputData.BUTTON_RELOAD, true);
        if (_isInteracting) data.buttons.Set(NetworkInputData.BUTTON_INTERACT, true);
        if (_isUsingItem) data.buttons.Set(NetworkInputData.BUTTON_USEITEM, true);
        if (_isSitting) data.buttons.Set(NetworkInputData.BUTTON_SIT, true);
        if (_isScoreBoardPopup) data.buttons.Set(NetworkInputData.BUTTON_SCOREBOARD, true);

        return data;
    }
    #region Input Subscription
    /// <summary>
    /// 입력 이벤트를 구독합니다.
    /// </summary>
    private void SubscribeToInputs()
    {
        inputManager.GetInput(EPlayerInput.move).performed += InputMove;
        inputManager.GetInput(EPlayerInput.move).canceled += InputMove;

        inputManager.GetInput(EPlayerInput.look).performed += InputLook;

        inputManager.GetInput(EPlayerInput.jump).started += ctx => SetBool(ref _isJumping, true);
        inputManager.GetInput(EPlayerInput.jump).canceled += ctx => SetBool(ref _isJumping, false);

        inputManager.GetInput(EPlayerInput.reload).started += ctx => SetBool(ref _isReloading, true);
        inputManager.GetInput(EPlayerInput.reload).canceled += ctx => SetBool(ref _isReloading, false);

        inputManager.GetInput(EPlayerInput.fire).started += ctx => SetBool(ref _isFiring, true);
        inputManager.GetInput(EPlayerInput.fire).canceled += ctx => SetBool(ref _isFiring, false);

        inputManager.GetInput(EPlayerInput.zoom).started += ctx => SetBool(ref _isZooming, true);
        inputManager.GetInput(EPlayerInput.zoom).canceled += ctx => SetBool(ref _isZooming, false);

        inputManager.GetInput(EPlayerInput.interaction).started += ctx => SetBool(ref _isInteracting, true);
        inputManager.GetInput(EPlayerInput.interaction).canceled += ctx => SetBool(ref _isInteracting, false);

        inputManager.GetInput(EPlayerInput.useItem).started += ctx => SetBool(ref _isUsingItem, true);
        inputManager.GetInput(EPlayerInput.useItem).canceled += ctx => SetBool(ref _isUsingItem, false);

        inputManager.GetInput(EPlayerInput.run).started += ctx => SetBool(ref _isRunning, true);
        inputManager.GetInput(EPlayerInput.run).canceled += ctx => SetBool(ref _isRunning, false);

        inputManager.GetInput(EPlayerInput.sit).started += ctx => SetBool(ref _isSitting, true);
        inputManager.GetInput(EPlayerInput.sit).canceled += ctx => SetBool(ref _isSitting, false);

        inputManager.GetInput(EPlayerInput.scoreboard).started += ctx => SetBool(ref _isScoreBoardPopup, true);
        inputManager.GetInput(EPlayerInput.scoreboard).canceled += ctx => SetBool(ref _isScoreBoardPopup, false);
    }

    /// <summary>
    /// 이동 입력 처리
    /// </summary>
    private void InputMove(InputAction.CallbackContext context)
    {
        Vector2 moveValue = context.ReadValue<Vector2>();
        MoveInput = new Vector3(moveValue.x, 0f, moveValue.y);
    }

    /// <summary>
    /// 시선 입력 처리
    /// </summary>
    private void InputLook(InputAction.CallbackContext context)
    {
        Vector2 lookValue = context.ReadValue<Vector2>();
        RotationX = lookValue.x;
        RotationY = lookValue.y;
    }
    /// <summary>
    /// 상태 값을 설정
    /// </summary>
    private void SetBool(ref bool field, bool value)
    {
        field = value;
    }
    #endregion
}
