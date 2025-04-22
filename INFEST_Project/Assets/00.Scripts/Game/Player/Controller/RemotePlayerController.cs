using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 3인칭 프리팹에 붙어서 애니메이션 관리
/// 1인칭은 따로 관리하므로 3인칭 프리팹과 관련된 모든 것을 관리(회전, 애니메이션)
/// 플레이어의 회전
/// 
/// 3인칭 프리팹은 나는 볼 수 없으므로 비활성화 상태
/// 상대방만 볼 수 있으니까 네트워크로 보여줘야한다
/// 
/// </summary>
public class RemotePlayerController : PlayerController
{
    #region 플레이어 프리팹 관련
    [Header("Components")]

    private PlayerStatData _networkData;   //서버로부터 받을 적의 데이터
    //private DummyGunController _dummyGunController;
    private Vector3 _networkPosition;

    // 필요한가?
    private Vector3 _correctedPosition;
    private float _correctionSpeed = 20f;

    private Quaternion _networkRotation;

    private PlayerStatHandler _statHandler; // 다른 플레이어의 정보를 네트워크로 받는 값으로 갱신한다

    // 필요한가?
    [SerializeField] private Transform weaponHolder; // 손에 붙이는 슬롯
    [SerializeField] private Transform weaponFix;   //방향 조정

    [Header("이름 태그")]
    //[SerializeField] public PlayerNameTag nameTag;

    [SerializeField] private Transform model;   // 3인칭 모델
    //[SerializeField] private Transform bodyCollider;  // Player의 Capsule Collider
    [SerializeField] private LayerMask groundLayer;

    private GameObject equippedWeapon;
    #endregion

    #region 3인칭 프리팹의 상태 관련
    private float _currentYaw = 0f;

    private Vector3 _inputMove;
    private float _inputYaw;
    private float _inputPitch;
    private bool _isJumping, _isInAir;
    private float _jumpElapsed = 0f;
    private float _jumpDuration = 0.5f;
    private float _jumpHeight = 0.5f;
    private Vector3 _jumpStartPos;
    private bool _isJumpingUp = false;
    private float _lastY;
    private float _verticalVelocity;

    private Vector3 _lookDirection;

    private Queue<Vector3> _positionQueue = new Queue<Vector3>();
    private Vector3 _startPos;
    private Vector3 _targetPos;
    private float _lerpTime = 0f;
    private float _lerpDuration = 0.05f; // 한 보간 단위 시간 (ms 단위로도 설정 가능)  
    private bool _isInterpolating = false;

    private Rigidbody _rb;  // Player에 붙어있는 rigidbody

    public PlayerStatData PlayerStateData => _networkData;
    //public DummyGunController DummyGunController => _dummyGunController;

    private Vector3 _velocity = Vector3.zero;
    #endregion


    public override void Awake()
    {
        base.Awake();

        // animator이 있는 곳에 추가했다(1인칭, 3인칭 각각)

        _rb = GetComponentInParent<Rigidbody>();
        _statHandler = GetComponentInParent<PlayerStatHandler>();

        //if (_statHandler != null)
        //{
        //    _jumpHeight = _statHandler.JumpPower;
        //    _rb.interpolation = RigidbodyInterpolation.Interpolate;
        //    _rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;

        //    _statHandler.OnDeath += OnDeath;
        //}
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

    // Start is called before the first frame update
    void Start()
    {
    }

        /// <summary>
    /// 상대방의 데이터를 이용하여 애니메이션, 데미지 주고받는 연산 등을 
    /// 상태에 따른 애니메이션 변화는 로컬에서 계산
    /// Network에서 내려받은 데이터를 신뢰하므로...
    /// Update만 FixedUpdate로 바꾸면 되지 않을까?
    /// </summary>
    //public override void FixedUpdateNetwork()
    //{
    //    // 상대방의 데이터를 내려받기만 한다
    //    if (GetInput(out NetworkInputData input))
    //    {
    //        Debug.Log("RemotePlayerController FixedUpdateNetwork 진입");
    //        base.Update();
    //        // RemotePlayerController만의 로직은 아래에 추가
    //    }
    //}

    //private void OnDeath()
    //{
    //    //_statHandler.OnDeath(_networkData.networkId);
    //    Debug.Log($"RemotePlayerController OnDeath : {_networkData.id}");
    //}
    //private void HandleJump()
    //{
    //    _jumpElapsed += Time.deltaTime;
    //    float t = Mathf.Clamp01(_jumpElapsed / _jumpDuration);
    //    float height = Mathf.SmoothStep(0f, _jumpHeight, t);
    //    Vector3 pos = _jumpStartPos;
    //    pos.y += height;
    //    transform.position = pos;

    //    if (t >= 1f)
    //        _isJumpingUp = false;
    //}

    //private void ApplyCorrection()
    //{
    //    if (_isInAir || _isJumpingUp) return;

    //    float dist = Vector3.Distance(transform.position, _correctedPosition);

    //    if (dist > 0.01f)
    //    {
    //        float lerpFactor = Mathf.Clamp01(_correctionSpeed * Time.deltaTime);
    //        transform.position = Vector3.Lerp(transform.position, _correctedPosition, lerpFactor);
    //    }
    //}
}

