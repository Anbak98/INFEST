using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.UI;
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
public class PlayerInputActionHandler : MonoBehaviour
{
    /// <summary>
    /// 속성(Property)은 참조(ref)로 전달할 수 없으므로, ref 전달 하려면 필드로 바꿔라
    /// </summary>
    public Vector3 MoveInput { get; private set; } = Vector2.zero;      // 이동 방향 (Vector3)
    public Vector2 LookInput { get; private set; } = Vector2.zero;      // 시선 방향 (Vector2)

    public float RotationX { get; private set; } = 0f;       // 마우스 X 회전 (pitch)
    public float RotationY { get; private set; } = 0f;       // 마우스 Y 회전 (yaw)
    private Vector2 _isSwapVelue;

    // InputAction과 연결
    private bool _isJumping;
    private bool _isReloading;
    private bool _isFiring;
    private bool _isZooming;
    private bool _isInteracting;
    private bool _isUsingGrenad;
    private bool _isUsingHeal;
    private bool _isUsingShield;
    private bool _isRunning;
    private bool _isSitting;
    private bool _isScoreBoardPopup;
    private bool _isMenuPopup;
    private bool _isChangingCamera;

    // 내부 변수(Input Action과 연결X)
    private bool _isShotgunOnFiring;
    private bool _isOnZoom;

    [SerializeField]
    private InputManager _inputManager;

    //[SerializeField] private UIController _UIController;

    private void Start()
    {
        /// performed: 키가 눌렸을 때 호출된다
        /// canceled: 키가 뗴졌을 때 호출
        //inputManager.GetInput(EPlayerInput.move).started += SetMoveInput;
        _inputManager.MoveGetInput(EPlayerInput.move).performed += SetMoveInput; // 여러 방향키 동시입력을 위해 추가
        _inputManager.MoveGetInput(EPlayerInput.move).canceled += SetMoveInput;

        _inputManager.MoveGetInput(EPlayerInput.look).started += SetLookInput;
        _inputManager.MoveGetInput(EPlayerInput.look).canceled += SetLookInput;

        _inputManager.MoveGetInput(EPlayerInput.swap).performed += SetSwapInput;
        _inputManager.MoveGetInput(EPlayerInput.swap).canceled += SetSwapInput;


        /// started, performed // canceled 에는 bool값을 반대로 바꾸는 메서드가 들어가야한다
        /// 
        //inputManager.GetInput(EPlayerInput.jump).started += StartJumpInput;
        _inputManager.MoveGetInput(EPlayerInput.jump).performed += StartJumpInput;
        _inputManager.MoveGetInput(EPlayerInput.jump).canceled += CancelJumpInput;

        _inputManager.MoveGetInput(EPlayerInput.fire).started += StartFireState;
        _inputManager.MoveGetInput(EPlayerInput.fire).canceled += CancelFireState;

        _inputManager.MoveGetInput(EPlayerInput.zoom).started += StartZoomState;
        _inputManager.MoveGetInput(EPlayerInput.zoom).canceled += CancelZoomState;

        _inputManager.MoveGetInput(EPlayerInput.reload).started += StartReloadState;
        _inputManager.MoveGetInput(EPlayerInput.reload).canceled += CancelReloadState;

        _inputManager.GetInput(EPlayerInput.interaction).started += StartInteraction;
        //_inputManager.GetInput(EPlayerInput.interaction).canceled += CancelInteraction;

        _inputManager.MoveGetInput(EPlayerInput.grenade).started += StartGrenade;
        _inputManager.MoveGetInput(EPlayerInput.grenade).canceled += CancelGrenade;

        _inputManager.MoveGetInput(EPlayerInput.heal).started += StartHeal;
        _inputManager.MoveGetInput(EPlayerInput.heal).canceled += CancelHeal;

        _inputManager.MoveGetInput(EPlayerInput.shield).started += StartShield;
        _inputManager.MoveGetInput(EPlayerInput.shield).canceled += CancelShield;

        _inputManager.MoveGetInput(EPlayerInput.run).started += StartRunState;
        _inputManager.MoveGetInput(EPlayerInput.run).canceled += CancelRunState;

        _inputManager.MoveGetInput(EPlayerInput.sit).started += StartSitState;
        _inputManager.MoveGetInput(EPlayerInput.sit).canceled += CancelSitState;

        _inputManager.GetInput(EPlayerInput.scoreboard).started += OpenScoreboard;
        _inputManager.GetInput(EPlayerInput.scoreboard).canceled += CloseScoreboard;

        _inputManager.GetInput(EPlayerInput.menu).started += OpenMenu;
        //_inputManager.GetInput(EPlayerInput.menu).canceled += CloseMenu;

        //_inputManager.GetInput(EPlayerInput.heal).started += StartChangeCamera;
        //_inputManager.GetInput(EPlayerInput.heal).canceled += CancelChangeCamera;
    }

    private void OnDisable()
    {
        _inputManager.MoveGetInput(EPlayerInput.move).started -= SetMoveInput;
        _inputManager.MoveGetInput(EPlayerInput.move).performed -= SetMoveInput; // 추가
        _inputManager.MoveGetInput(EPlayerInput.move).canceled -= SetMoveInput;

        _inputManager.MoveGetInput(EPlayerInput.look).started -= SetLookInput;
        _inputManager.MoveGetInput(EPlayerInput.look).canceled -= SetLookInput;

        _inputManager.MoveGetInput(EPlayerInput.swap).performed -= SetSwapInput;
        _inputManager.MoveGetInput(EPlayerInput.swap).canceled -= SetSwapInput;

        _inputManager.MoveGetInput(EPlayerInput.jump).started -= StartJumpInput;
        _inputManager.MoveGetInput(EPlayerInput.jump).canceled -= CancelJumpInput;

        _inputManager.MoveGetInput(EPlayerInput.fire).started -= StartFireState;
        _inputManager.MoveGetInput(EPlayerInput.fire).canceled -= CancelFireState;

        _inputManager.MoveGetInput(EPlayerInput.zoom).started -= StartZoomState;
        _inputManager.MoveGetInput(EPlayerInput.zoom).canceled -= CancelZoomState;

        _inputManager.MoveGetInput(EPlayerInput.reload).started -= StartReloadState;
        _inputManager.MoveGetInput(EPlayerInput.reload).canceled -= CancelReloadState;

        _inputManager.GetInput(EPlayerInput.interaction).started -= StartInteraction;
        //_inputManager.GetInput(EPlayerInput.interaction).canceled -= CancelInteraction;
                      
        _inputManager.MoveGetInput(EPlayerInput.grenade).started -= StartGrenade;
        _inputManager.MoveGetInput(EPlayerInput.grenade).canceled -= CancelGrenade;
                      
        _inputManager.MoveGetInput(EPlayerInput.heal).started -= StartHeal;
        _inputManager.MoveGetInput(EPlayerInput.heal).canceled -= CancelHeal;
                      
        _inputManager.MoveGetInput(EPlayerInput.shield).started -= StartShield;
        _inputManager.MoveGetInput(EPlayerInput.shield).canceled -= CancelShield;
                      
        _inputManager.MoveGetInput(EPlayerInput.run).started -= StartRunState;
        _inputManager.MoveGetInput(EPlayerInput.run).canceled -= CancelRunState;

        _inputManager.MoveGetInput(EPlayerInput.sit).started -= StartSitState;
        _inputManager.MoveGetInput(EPlayerInput.sit).canceled -= CancelSitState;

        _inputManager.GetInput(EPlayerInput.scoreboard).started -= OpenScoreboard;
        _inputManager.GetInput(EPlayerInput.scoreboard).canceled -= CloseScoreboard;

        _inputManager.GetInput(EPlayerInput.menu).started -= OpenMenu;
        //_inputManager.GetInput(EPlayerInput.menu).canceled -= CloseMenu;

        //_inputManager.GetInput(EPlayerInput.heal).started -= StartChangeCamera;
        //_inputManager.GetInput(EPlayerInput.heal).canceled -= CancelChangeCamera;
    }

    #region Get, Set
    #region Move
    private void SetMoveInput(InputAction.CallbackContext context)
    {
        Vector2 moveInput = context.ReadValue<Vector2>();
        if (moveInput.sqrMagnitude < 0.01f)
        {
            MoveInput = Vector3.zero;
        }
        else
        {
            MoveInput = new Vector3(moveInput.x, 0f, moveInput.y);
        }


        //MoveInput = new Vector3(moveInput.x, 0f, moveInput.y);
    }
    public Vector3 GetMoveInput() => MoveInput;

    #endregion
    #region Look   
    private void SetLookInput(InputAction.CallbackContext context)
    {
        LookInput = context.ReadValue<Vector2>();
        //Debug.Log($"[Input] SetLookInput - Look: {look}, Phase: {context.phase}");
        RotationX = LookInput.x;
        RotationY = LookInput.y;
    }
    public Vector2 GetLookInput() => LookInput;
    //public float GetRotationX() => RotationX;
    //public float GetRotationY() => RotationY;
    #endregion
    #region Swap
    private void SetSwapInput(InputAction.CallbackContext context)
    {
        //Debug.Log($"[Input] SetSwapInput - Velue: {context.ReadValue<Vector2>().y}, Phase: {context.phase}");
        _isSwapVelue = context.ReadValue<Vector2>();
    }
    public Vector2 GetSwapValue() => _isSwapVelue;
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
    public bool GetIsJumping() => _isJumping;
    #endregion

    #region Fire
    private void StartFireState(InputAction.CallbackContext context)
    {
        //Debug.Log($"[Input] SetFireState - Fire started");
        _isFiring = true;   // 유지 중
        _isShotgunOnFiring = true; // 누르고 있다, 샷건의 연발 방지용(누르고 있어도 안나가야한다)
    }
    private void CancelFireState(InputAction.CallbackContext context)
    {
        //Debug.Log($"[Input] SetFireState - Fire canceled");
        _isFiring = false;
    }
    public bool GetIsFiring() => _isFiring;
    public bool GetShotgunIsOnFiring() => _isShotgunOnFiring;
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
        _isOnZoom = true;
    }
    public bool GetIsZooming() => _isZooming;
    public bool GetIsOnZoom() => _isOnZoom;
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
    public bool GetIsReloading() => _isReloading;
    #endregion
    #region Interaction
    private void StartInteraction(InputAction.CallbackContext context)
    {
        Debug.Log("[Input] TriggerInteraction - Interaction triggered");
        _isInteracting = true;
        //Invoke(nameof(CancelInteraction), 0.1f);
    }

    private void CancelInteraction(InputAction.CallbackContext context)
    {
        Debug.Log("[Input] ResetInteractionState - Interaction reset");
        _isInteracting = false;
    }
    public bool GetIsInteracting() => _isInteracting;
    #endregion
    #region Grenade
    private void StartGrenade(InputAction.CallbackContext context)
    {
        Debug.Log("[Input] TriggerUseItem - Use item triggered");
        _isUsingGrenad = true;
        //Invoke(nameof(CancelUseItem), 0.1f);
    }

    private void CancelGrenade(InputAction.CallbackContext context)
    {
        Debug.Log("[Input] ResetUseItemState - Use item reset");
        _isUsingGrenad = false;
    }
    public bool GetIsUsingGrenade() => _isUsingGrenad;
    #endregion
    #region Heal
    private void StartHeal(InputAction.CallbackContext context)
    {
        Debug.Log("[Input] TriggerUseItem - Use item triggered");
        _isUsingHeal = true;
        //Invoke(nameof(CancelUseItem), 0.1f);
    }

    private void CancelHeal(InputAction.CallbackContext context)
    {
        Debug.Log("[Input] ResetUseItemState - Use item reset");
        _isUsingHeal = false;
    }
    public bool GetIsUsingHeal() => _isUsingHeal;
    #endregion
    #region Shield
    private void StartShield(InputAction.CallbackContext context)
    {
        Debug.Log("[Input] TriggerUseItem - Use item triggered");
        _isUsingShield = true;
        //Invoke(nameof(CancelUseItem), 0.1f);
    }

    private void CancelShield(InputAction.CallbackContext context)
    {
        Debug.Log("[Input] ResetUseItemState - Use item reset");
        _isUsingShield = false;
    }
    public bool GetIsUsingShield() => _isUsingShield;
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
    public bool GetIsRunning() => _isRunning;
    #endregion
    #region Sit
    private void StartSitState(InputAction.CallbackContext context)
    {
        //Debug.Log($"[Input] StartSitState - Sitting Started");
        _isSitting = true;
    }

    private void CancelSitState(InputAction.CallbackContext context)
    {
        //Debug.Log($"[Input] CancelSitState - Sitting Canceled");
        _isSitting = false;
    }
    public bool GetIsSitting() => _isSitting;
    #endregion
    #region Scoreboard
    private void OpenScoreboard(InputAction.CallbackContext context)
    {
        //_UIController.Show<UIScoreboardView>();
        //_isScoreBoardPopup = true;
    }

    private void CloseScoreboard(InputAction.CallbackContext context)
    {
        //_UIController.Hide();
        //_isScoreBoardPopup = false;
    }
    public bool GetIsScoreBoardPopup() => _isScoreBoardPopup;
    #endregion
    #region Menu
    private void OpenMenu(InputAction.CallbackContext context)
    {
        if (!_isMenuPopup)
        {
            //_UIController.Show<UIMenuView>();
            //_isMenuPopup = true;
        }
        else
        {
            //_UIController.Hide();
            //_isMenuPopup = false;
        }
    }

    //private void CloseMenu(InputAction.CallbackContext context)
    //{
    //    _UIController.Hide();
    //    _isMenuPopup = false;
    //}
    public bool GetIsMenuPopup() => _isMenuPopup;

    #endregion
    #region ChangeCamera(SpectorMode)
    private void ChangeCamera(InputAction.CallbackContext context)
    {
        // 관전 모드 카메라 변경

    }

    private void StartChangeCamera(InputAction.CallbackContext context)
    {
        Debug.Log("[Input] ChangeCamera triggered");
        _isChangingCamera = true;
        //Invoke(nameof(CancelUseItem), 0.1f);
    }

    private void CancelChangeCamera(InputAction.CallbackContext context)
    {
        Debug.Log("[Input] ChangeCamera triggered reset");
        _isChangingCamera = false;
    }

    public bool GetIsChangingCamera() => _isChangingCamera;


    #endregion
    #endregion
    /// <summary>
    /// 네트워크 입력 만들기
    /// PlayerInputSender에서 호출하여 저장된 입력을 서버로 전송한다
    /// </summary>
    /// <returns></returns>
    public NetworkInputData? GetNetworkInput()
    {
        var data = new NetworkInputData
        {
            direction = MoveInput,
            lookDelta = new Vector2(RotationX, RotationY),


            isRunning = _isRunning,
            isJumping = _isJumping,
            isFiring = _isFiring,
            isZooming = _isZooming,
            isReloading = _isReloading,
            isInteracting = _isInteracting,
            isUsingGrenad = _isUsingGrenad,
            isUsingHeal = _isUsingHeal,
            isUsingShield = _isUsingShield,
            isSitting = _isSitting,
            isMenuPopup = _isMenuPopup,
            isScoreBoardPopup = _isScoreBoardPopup,

            isShotgunOnFiring = _isShotgunOnFiring,
            isOnZoom = _isOnZoom,

            scrollValue = _isSwapVelue
        };

        // button 상태는 여기서
        if (_isRunning) data.buttons.Set(NetworkInputData.BUTTON_RUN, true);
        if (_isJumping) data.buttons.Set(NetworkInputData.BUTTON_JUMP, true);
        if (_isFiring) data.buttons.Set(NetworkInputData.BUTTON_FIRE, true);
        if (_isZooming) data.buttons.Set(NetworkInputData.BUTTON_ZOOM, true);
        if (_isReloading) data.buttons.Set(NetworkInputData.BUTTON_RELOAD, true);
        if (_isInteracting) data.buttons.Set(NetworkInputData.BUTTON_INTERACT, true);
        if (_isUsingGrenad) data.buttons.Set(NetworkInputData.BUTTON_USEGRENAD, true);
        if (_isUsingHeal) data.buttons.Set(NetworkInputData.BUTTON_USEHEAL, true);
        if (_isUsingShield) data.buttons.Set(NetworkInputData.BUTTON_USESHIELD, true);
        if (_isSitting) data.buttons.Set(NetworkInputData.BUTTON_SIT, true);
        if (_isScoreBoardPopup) data.buttons.Set(NetworkInputData.BUTTON_SCOREBOARD, true);
        if (_isMenuPopup) data.buttons.Set(NetworkInputData.BUTTON_MENU, true);
        if (_isChangingCamera) data.buttons.Set(NetworkInputData.BUTTON_CHANGECAMERA, true);

        _isInteracting = false;
        // 샷건의 경우 누르고 있어도 발사가 되면 안되니까 막아놓았다
        if (_isShotgunOnFiring) data.buttons.Set(NetworkInputData.BUTTON_FIREPRESSED, true);   // 구조체의 변수를 바꾼거고
        _isShotgunOnFiring = false;    // PlayerInputHandler의 변수를 바꾼거다

        // Shotgun의 단발을 위해 false로 바꿔야한다
        // 위의 if문이 있을 때 다른 입력이 없을 때 우클릭을 해제하면, 아무 입력도 없게 되어 return된다.
        // _isOnZoom은 위의 if문에서 null 통과하지 않게 만들어놔서 그거 통과하려고 어쩔 수 없이 만든 변수다
        // 하지만 플레이어의 상태를 추가한다면 입력이 없어도 null을 리턴하면 안되기 때문에
        // 위의 if문을 삭제하든가 수정해야하고, 그렇게 되면 _isOnZoom은 없어도 되는 변수다
        if (_isOnZoom) data.buttons.Set(NetworkInputData.BUTTON_ZOOMPRESSED, true);
        _isOnZoom = false;

        return data;
    }
}
