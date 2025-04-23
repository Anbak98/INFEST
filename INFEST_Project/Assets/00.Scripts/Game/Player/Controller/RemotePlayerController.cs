using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 3��Ī �����տ� �پ �ִϸ��̼� ����
/// 1��Ī�� ���� �����ϹǷ� 3��Ī �����հ� ���õ� ��� ���� ����(�̵�, ȸ��, �ִϸ��̼�)
/// 
/// 3��Ī �������� ���� �� �� �����Ƿ� ��Ȱ��ȭ ����
/// ���游 �� �� �����ϱ� ��Ʈ��ũ�� ��������Ѵ�
/// </summary>
public class RemotePlayerController : PlayerController
{
    #region �÷��̾� ������ ����
    [Header("Components")]
    public Player player;

    // 3��Ī�� �÷��̾��� �ִϸ��̼Ǹ� ��ȯ�Ѵ�
    public Animator animator;
    // collider�� CharacterController�� ����Ǿ��ִ�
    //public CharacterController controller;    // �θ��� PlayerController�� ����Ǿ��ִ�

    private PlayerStatData _networkData;   //�����κ��� ���� ���� ������
    //private DummyGunController _dummyGunController;
    private Vector3 _networkPosition;

    // �ʿ��Ѱ�?
    private Vector3 _correctedPosition;
    private float _correctionSpeed = 20f;

    private Quaternion _networkRotation;

    private PlayerStatHandler _statHandler;

    // �ʿ��Ѱ�?
    [SerializeField] private Transform weaponHolder; // �տ� ���̴� ����
    [SerializeField] private Transform weaponFix;   //���� ����

    [Header("�̸� �±�")]
    //[SerializeField] public PlayerNameTag nameTag;

    [SerializeField] private Transform model;   // 3��Ī ��
    [SerializeField] private Transform bodyCollider;  // Rigidbody + CapsuleCollider
    [SerializeField] private LayerMask groundLayer;

    private GameObject equippedWeapon;
    #endregion

    #region 3��Ī �������� ���� ����
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
    private float _lerpDuration = 0.05f; // �� ���� ���� �ð� (ms �����ε� ���� ����)  
    private bool _isInterpolating = false;

    private Rigidbody _rb;  // Player�� �پ��ִ� rigidbody

    public PlayerStatData PlayerStateData => _networkData;
    //public DummyGunController DummyGunController => _dummyGunController;

    private Vector3 _velocity = Vector3.zero;

    //public PlayerStat
    #endregion


    public override void Awake()
    {
        base.Awake();

        // animator�� �ִ� ���� �߰��ߴ�(1��Ī, 3��Ī ����)
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

    /// <summary>
    /// ������ �����͸� �̿��Ͽ� �ִϸ��̼�, ������ �ְ�޴� ���� ���� 
    /// ���¿� ���� �ִϸ��̼� ��ȭ�� ���ÿ��� ���
    /// </summary>
    // Update is called once per frame
    public override void Update()
    {
        // �׽�Ʈ������ ���� �÷��̾� �̵���Ų��

        // ������ �Է��� ���� ���¿� ����
        //stateMachine.currentState.HandleInput(input);

        // ������ ���� ������Ʈ
        //stateMachine.currentState.UpdateLogic();
    }

    /// <summary>
    /// ������ �����͸� �ޱ⸸ �Ѵ�
    /// </summary>
    public override void FixedUpdateNetwork()
    {
        Debug.Log("RemotePlayerController FixedUpdateNetwork ����");
        // ������ �����͸� �����ޱ⸸ �Ѵ�
        if (GetInput(out NetworkInputData input))
        {

        }
    }


    public override void StartJump()
    {
        Debug.Log("���� ����");
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

