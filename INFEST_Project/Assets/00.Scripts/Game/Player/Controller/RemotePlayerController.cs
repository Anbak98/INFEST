using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 3인칭 프리팹에 붙어서 애니메이션 관리
/// 1인칭은 따로 관리하므로 3인칭 프리팹과 관련된 모든 것을 관리(이동, 회전, 애니메이션)
/// </summary>
public class RemotePlayerController : PlayerController
{
    #region 플레이어 프리팹 관련
    [Header("Components")]
    public Player player;

    // 3인칭은 플레이어의 애니메이션만 교환한다
    public Animator animator;
    // collider는 CharacterController에 내재되어있다
    //public CharacterController controller;    // 부모인 PlayerController에 선언되어있다

    private PlayerStatData _networkData;   //서버로부터 받을 적의 데이터
    //private DummyGunController _dummyGunController;
    private Vector3 _networkPosition;

    // 필요한가?
    private Vector3 _correctedPosition;
    private float _correctionSpeed = 20f;

    private Quaternion _networkRotation;

    private PlayerStatHandler _statHandler;

    // 필요한가?
    [SerializeField] private Transform weaponHolder; // 손에 붙이는 슬롯
    [SerializeField] private Transform weaponFix;   //방향 조정

    [Header("이름 태그")]
    //[SerializeField] public PlayerNameTag nameTag;

    [SerializeField] private Transform model;   // 3인칭 모델
    [SerializeField] private Transform bodyCollider;  // Rigidbody + CapsuleCollider
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

    //public PlayerStat
    #endregion


    protected override void Awake()
    {
        base.Awake();

        // animator이 있는 곳에 추가했다(1인칭, 3인칭 각각)
        animator = GetComponent<Animator>();
        _rb = GetComponentInParent<Rigidbody>();
        _statHandler = GetComponentInParent<PlayerStatHandler>();

        _jumpHeight = _statHandler.JumpPower;
        _rb.interpolation = RigidbodyInterpolation.Interpolate;
        _rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;

        GetComponentInParent<PlayerStatHandler>().OnDeath += OnDeath;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    protected override void Update()
    {

    }

    public override void FixedUpdateNetwork()
    {
        Debug.Log("RemotePlayerController FixedUpdateNetwork 진입");
        if (GetInput(out NetworkInputData input))
        {
            // 테스트용으로 플레이어를 이동시킨다

            //// 입력을 현재 상태에 전달
            //stateMachine.currentState.HandleInput(input);

            //// 상태 업데이트
            //stateMachine.currentState.UpdateLogic();
        }
    }


    // 3인칭에만 붙는 변수들
    public override void ApplyNetworkState(PlayerStatData data)
    {

    }
    public override void PlayFireAnim() => animator?.SetTrigger("Fire");

    public override void StartJump()
    {
        Debug.Log("점프 시작");
    }

    public override void HandleFire(bool started)
    {
        Debug.Log("Fire 시작");
    }


    private void OnDeath()
    {
        //_statHandler.OnDeath(_networkData.networkId);
        Debug.Log($"RemotePlayerController OnDeath : {_networkData.networkId}");
    }
    private void HandleJump()
    {
        _jumpElapsed += Time.deltaTime;
        float t = Mathf.Clamp01(_jumpElapsed / _jumpDuration);
        float height = Mathf.SmoothStep(0f, _jumpHeight, t);
        Vector3 pos = _jumpStartPos;
        pos.y += height;
        transform.position = pos;

        if (t >= 1f)
            _isJumpingUp = false;
    }


    private void ApplyCorrection()
    {
        if (_isInAir || _isJumpingUp) return;

        float dist = Vector3.Distance(transform.position, _correctedPosition);

        if (dist > 0.01f)
        {
            float lerpFactor = Mathf.Clamp01(_correctionSpeed * Time.deltaTime);
            transform.position = Vector3.Lerp(transform.position, _correctedPosition, lerpFactor);
        }
    }

    public override bool IsGrounded()
    {
        Vector3 origin = bodyCollider.position + Vector3.up * 0.1f;
        return Physics.Raycast(origin, Vector3.down, 1.0f, groundLayer);
    }

}

