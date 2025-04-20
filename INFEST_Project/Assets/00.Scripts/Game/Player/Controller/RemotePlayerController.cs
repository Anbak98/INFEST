using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 3��Ī �����տ� �پ �ִϸ��̼� ����
/// 1��Ī�� ���� �����ϹǷ� 3��Ī �����հ� ���õ� ��� ���� ����(ȸ��, �ִϸ��̼�)
/// �÷��̾��� ȸ��
/// 
/// 3��Ī �������� ���� �� �� �����Ƿ� ��Ȱ��ȭ ����
/// ���游 �� �� �����ϱ� ��Ʈ��ũ�� ��������Ѵ�
/// 
/// </summary>
public class RemotePlayerController : PlayerController
{
    #region �÷��̾� ������ ����
    [Header("Components")]

    private PlayerStatData _networkData;   //�����κ��� ���� ���� ������
    //private DummyGunController _dummyGunController;
    private Vector3 _networkPosition;

    // �ʿ��Ѱ�?
    private Vector3 _correctedPosition;
    private float _correctionSpeed = 20f;

    private Quaternion _networkRotation;

    private PlayerStatHandler _statHandler; // �ٸ� �÷��̾��� ������ ��Ʈ��ũ�� �޴� ������ �����Ѵ�

    // �ʿ��Ѱ�?
    [SerializeField] private Transform weaponHolder; // �տ� ���̴� ����
    [SerializeField] private Transform weaponFix;   //���� ����

    [Header("�̸� �±�")]
    //[SerializeField] public PlayerNameTag nameTag;

    [SerializeField] private Transform model;   // 3��Ī ��
    //[SerializeField] private Transform bodyCollider;  // Player�� Capsule Collider
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
    #endregion


    public override void Awake()
    {
        base.Awake();

        // animator�� �ִ� ���� �߰��ߴ�(1��Ī, 3��Ī ����)


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

    // Start is called before the first frame update
    void Start()
    {
    }

    //public override void Update()
    //{
    //    base.Update();
    //    // RemotePlayerController���� ������ �Ʒ��� �߰�
    //}

    /// <summary>
    /// ������ �����͸� �̿��Ͽ� �ִϸ��̼�, ������ �ְ�޴� ���� ���� 
    /// ���¿� ���� �ִϸ��̼� ��ȭ�� ���ÿ��� ���
    /// Network���� �������� �����͸� �ŷ��ϹǷ�...
    /// Update�� FixedUpdate�� �ٲٸ� ���� ������?
    /// </summary>
    public override void FixedUpdateNetwork()
    {
        // ������ �����͸� �����ޱ⸸ �Ѵ�
        if (GetInput(out NetworkInputData input))
        {
            Debug.Log("RemotePlayerController FixedUpdateNetwork ����");
            base.Update();
            // RemotePlayerController���� ������ �Ʒ��� �߰�
        }

    }

    // Remote �÷��̾�� ���� �̵����� �ʴ´� (�����κ��� �޴� ������ �ŷ�)
    public override void HandleMovement()
    {
        // TODO
    }
    public override void ApplyGravity()
    {
        // TODO
    }

    public override void HandleFire()
    {
        // TODO
    }
    public override void StartJump()
    {
        // TODO
    }
    public override void StartSit()
    {
        // TODO
    }
    public override void StartStand()
    {
        // TODO
    }


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

