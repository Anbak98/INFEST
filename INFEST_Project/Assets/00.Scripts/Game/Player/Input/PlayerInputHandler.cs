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
    /// 속성(Property)은 참조(ref)로 전달할 수 없으므로, ref 전달 하려면 필드로 바꿔라
    /// </summary>
    public Vector3 MoveInput { get; private set; } = Vector3.zero;      // 이동 방향 (Vector3)

    public float RotationX { get; private set; } = 0f;       // 마우스 X 회전 (pitch)
    public float RotationY { get; private set; } = 0f;       // 마우스 Y 회전 (yaw)

    private bool _isJumping;
    private bool _isReloading;
    private bool _isFiring;
    private bool _isZooming;
    private bool _isInteracting;
    private bool _isUsingItem;
    private bool _isRunning;
    private bool _isSitting;
    private bool _isScoreBoardPopup;
    //
    private bool _isOnFiring;
    private bool _isOnZoom;

    // 임시(동적연결)
    public InputManager inputManager;

    private void Awake()
    {
        if (inputManager == null)
            inputManager = FindObjectOfType<InputManager>();
    }


    private void OnEnable()
    {
        /// performed: 키가 눌렸을 때 호출된다
        /// canceled: 키가 뗴졌을 때 호출
        inputManager.GetInput(EPlayerInput.move).started += SetMoveInput;
        inputManager.GetInput(EPlayerInput.move).canceled += SetMoveInput;

        inputManager.GetInput(EPlayerInput.look).started += SetLookInput;
        inputManager.GetInput(EPlayerInput.look).canceled += SetLookInput;

        // 계속 누르고 있는 경우 
        //_isOnFiring = !inputManager.GetInput(EPlayerInput.fire).IsPressed();


        /// started, performed // canceled 에는 bool값을 반대로 바꾸는 메서드가 들어가야한다
        /// 
        inputManager.GetInput(EPlayerInput.jump).started += StartJumpInput;
        inputManager.GetInput(EPlayerInput.jump).canceled += CancelJumpInput;

        inputManager.GetInput(EPlayerInput.fire).started += StartFireState;
        inputManager.GetInput(EPlayerInput.fire).canceled += CancelFireState;

        inputManager.GetInput(EPlayerInput.zoom).started += StartZoomState;
        inputManager.GetInput(EPlayerInput.zoom).canceled += CancelZoomState;

        inputManager.GetInput(EPlayerInput.reload).started += StartReloadState;
        inputManager.GetInput(EPlayerInput.reload).canceled += CancelReloadState;

        inputManager.GetInput(EPlayerInput.interaction).started += StartInteraction;
        inputManager.GetInput(EPlayerInput.interaction).canceled += CancelInteraction;

        inputManager.GetInput(EPlayerInput.useItem).started += StartUseItem;
        inputManager.GetInput(EPlayerInput.useItem).canceled += CancelUseItem;

        inputManager.GetInput(EPlayerInput.run).started += StartRunState;
        inputManager.GetInput(EPlayerInput.run).canceled += CancelRunState;

        inputManager.GetInput(EPlayerInput.sit).started += StartSitState;
        inputManager.GetInput(EPlayerInput.sit).canceled += CancelSitState;

        inputManager.GetInput(EPlayerInput.scoreboard).started += OpenScoreboard;
        inputManager.GetInput(EPlayerInput.scoreboard).canceled += CloseScoreboard;

    }

    private void OnDisable()
    {
        inputManager.GetInput(EPlayerInput.move).started -= SetMoveInput;
        inputManager.GetInput(EPlayerInput.move).canceled -= SetMoveInput;

        inputManager.GetInput(EPlayerInput.look).started -= SetLookInput;
        inputManager.GetInput(EPlayerInput.look).canceled -= SetLookInput;

        inputManager.GetInput(EPlayerInput.jump).started -= StartJumpInput;
        inputManager.GetInput(EPlayerInput.jump).canceled -= CancelJumpInput;

        inputManager.GetInput(EPlayerInput.fire).started -= StartFireState;
        inputManager.GetInput(EPlayerInput.fire).canceled -= CancelFireState;

        inputManager.GetInput(EPlayerInput.zoom).started -= StartZoomState;
        inputManager.GetInput(EPlayerInput.zoom).canceled -= CancelZoomState;

        inputManager.GetInput(EPlayerInput.reload).started -= StartReloadState;
        inputManager.GetInput(EPlayerInput.reload).canceled -= CancelReloadState;

        inputManager.GetInput(EPlayerInput.interaction).started -= StartInteraction;
        inputManager.GetInput(EPlayerInput.interaction).canceled -= CancelInteraction;

        inputManager.GetInput(EPlayerInput.useItem).started -= StartUseItem;
        inputManager.GetInput(EPlayerInput.useItem).canceled -= CancelUseItem;

        inputManager.GetInput(EPlayerInput.run).started -= StartRunState;
        inputManager.GetInput(EPlayerInput.run).canceled -= CancelRunState;

        inputManager.GetInput(EPlayerInput.sit).started -= StartSitState;
        inputManager.GetInput(EPlayerInput.sit).canceled -= CancelSitState;

        inputManager.GetInput(EPlayerInput.scoreboard).started -= OpenScoreboard;
        inputManager.GetInput(EPlayerInput.scoreboard).canceled -= CloseScoreboard;
    }

    // 외부에서 호출하는 저장용 메서드들
    /// SetBool
    /// 상태 지속형 동작에 적합(예: 달리기/앉기/조준 상태)
    /// 명확한 상태 전환 표현(true/false 직접 설정)
    /// 애니메이터 파라미터와 1:1 매칭 가능
    /// 
    /// Trigger      
    /// 단발성 이벤트에 적합(예: 재장전/아이템 사용)
    /// 일회성 신호 전달
    /// 목적애니메이션 트리거와 연동 시 유용 
    /// 
    /// <summary>
    /// SetXXXInput → 입력값을 저장하는 경우
    /// UpdateXXXState / SetXXXState → Boolean 상태 토글
    /// TriggerXXX / FireXXX → 단발성 이벤트(ex.reload, use item)
    /// </summary>
    /// <param name="context"></param>
    
    #region Move
    private void SetMoveInput(InputAction.CallbackContext context)
    {
        Debug.Log($"[Input] SetMoveInput - Move: {context.ReadValue<Vector2>()}, Phase: {context.phase}");
        Vector2 moveInput = context.ReadValue<Vector2>();
        MoveInput = new Vector3(moveInput.x, 0f, moveInput.y);
    }
    #endregion

    #region Look   
    private void SetLookInput(InputAction.CallbackContext context)
    {
        Vector2 look = context.ReadValue<Vector2>();
        Debug.Log($"[Input] SetLookInput - Look: {look}, Phase: {context.phase}");
        RotationX = look.x;
        RotationY = look.y;
    }
    #endregion

    #region Jump
    private void StartJumpInput(InputAction.CallbackContext context)
    {
        Debug.Log("[Input] StartJumpInput - Jump started");
        _isJumping = true;
    }   
    private void CancelJumpInput(InputAction.CallbackContext context)
    {
        Debug.Log("[Input] CancelJumpInput - Jump canceled");
        _isJumping = false;
    }
    #endregion
    #region Fire
    private void StartFireState(InputAction.CallbackContext context)
    {
        Debug.Log($"[Input] SetFireState - Fire started");
        _isFiring = true;
    }
    private void CancelFireState(InputAction.CallbackContext context)
    {
        Debug.Log($"[Input] SetFireState - Fire canceled");
        _isFiring = false;
    }
    #endregion
    #region Zoom
    private void StartZoomState(InputAction.CallbackContext context)
    {
        Debug.Log($"[Input] StartZoomState - Zoom Started");
        _isZooming = true;
    }

    private void CancelZoomState(InputAction.CallbackContext context)
    {
        Debug.Log($"[Input] CancelZoomState - Zoom Canceled");
        _isZooming = false;
    }
    #endregion
    #region Reload
    private void StartReloadState(InputAction.CallbackContext context)
    {
        Debug.Log("[Input] TriggerReload - Reload triggered");
        _isReloading = true;
        /// 방식 참고해서 나중에 State Machine에서 구현한다
        //Invoke(nameof(CancelReloadState), 0.1f);    // CancelReloadState를 0.1초 뒤에 호출하는 방식
    }
    private void CancelReloadState(InputAction.CallbackContext context)
    {
        Debug.Log("[Input] ResetReloadState - Reload reset");
        _isReloading = false;
    }
    #endregion
    #region Interaction
    private void StartInteraction(InputAction.CallbackContext context)
    {
        Debug.Log("[Input] TriggerInteraction - Interaction triggered");
        _isInteracting = true;
        Invoke(nameof(CancelInteraction), 0.1f);
    }

    private void CancelInteraction(InputAction.CallbackContext context)
    {
        Debug.Log("[Input] ResetInteractionState - Interaction reset");
        _isInteracting = false;
    }
    #endregion
    #region UseItem
    private void StartUseItem(InputAction.CallbackContext context)
    {
        Debug.Log("[Input] TriggerUseItem - Use item triggered");
        _isUsingItem = true;
        //Invoke(nameof(CancelUseItem), 0.1f);
    }

    private void CancelUseItem(InputAction.CallbackContext context)
    {
        Debug.Log("[Input] ResetUseItemState - Use item reset");
        _isUsingItem = false;
    }
    #endregion
    #region Run
    private void StartRunState(InputAction.CallbackContext context)
    {
        Debug.Log($"[Input] StartRunState - Running Started");
        _isRunning = true;
    }
    private void CancelRunState(InputAction.CallbackContext context)
    {
        Debug.Log($"[Input] CancelRunState - Running Canceled");
        _isRunning = false;
    }
    #endregion
    #region Sit
    private void StartSitState(InputAction.CallbackContext context)
    {
        Debug.Log($"[Input] StartSitState - Sitting Started");
        _isSitting = true;
    }

    private void CancelSitState(InputAction.CallbackContext context)
    {
        Debug.Log($"[Input] CancelSitState - Sitting Canceled");
        _isSitting = false;
    }
    #endregion
    #region Scoreboard
    private void OpenScoreboard(InputAction.CallbackContext context)
    {
        Debug.Log("[Input] OpenScoreboard - Scoreboard opened");
        _isScoreBoardPopup = true;
    }

    private void CloseScoreboard(InputAction.CallbackContext context)
    {
        Debug.Log("[Input] CloseScoreboard - Scoreboard closed");
        _isScoreBoardPopup = false;
    }
    #endregion

    /// <summary>
    /// 네트워크 입력 만들기
    /// PlayerInputSender에서 호출하여 저장된 입력을 서버로 전송한다
    /// </summary>
    /// <returns></returns>
    public NetworkInputData? GetNetworkInput()
    {
        if (MoveInput == Vector3.zero &&
            !_isJumping && !_isFiring && !_isReloading &&
            !_isZooming && !_isInteracting && !_isUsingItem &&
            !_isRunning && !_isSitting && !_isScoreBoardPopup)
        {
            return null;
        }
        //Debug.Log("입력 받았다");
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
        if (_isScoreBoardPopup) data.buttons.Set(NetworkInputData.BUTTON_SCOREBOARD, true);
        //
        if (_isOnFiring) data.buttons.Set(NetworkInputData.BUTTON_FIREPRESSED, true);
        if (_isOnZoom) data.buttons.Set(NetworkInputData.BUTTON_ZOOMPRESSED, true);
        _isOnFiring = false;
        _isOnZoom = false;

        return data;
    }
}
