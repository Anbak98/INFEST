using Fusion;
using UnityEngine;

public class PlayerAnimationController : NetworkBehaviour
{
    public Animator playerAnimator;

    // �Է��� ���� ��Ʈ��ũ�� ���޹޴´�
    [Networked] public Vector3 MoveDirection { get; set; }
    [Networked] public Vector2 lookDelta { get; set; }
    [Networked] public bool isJumping { get; set; }
    [Networked] public bool isReloading { get; set; }
    [Networked] public bool isFiring { get; set; }
    [Networked] public bool isRunning { get; set; }
    [Networked]  public bool isSitting { get; set; }


    private string _groundParameterName = "@Ground";    // 
    private string _idleParameterName = "Idle";
    //private string _moveParameterName = "Move";    // walk
    private string _moveXParameterName = "MoveX";    // walk
    private string _moveZParameterName = "MoveZ";    // walk
    private string _runParameterName = "Run";
    private string _attackParameterName = "Fire";
    private string _reloadParameterName = "IsReloading";    // �����ϴ� ���°� �־�� �� �� ���⵵ �ϴ�
    private string _aimParameterName = "Aim";


    private string _airParameterName = "@Air";
    private string _jumpParameterName = "Jump";
    private string _fallParameterName = "Fall";
    // jump���¿� �и��� �ٸ���
    // isGround�� false && isjumping�� false�� ������ ǥ���� �� �־ �Ķ���ʹ� ��� �ȴ�
    // ������ parameter�� ������ idle�� �����ؾ��Ѵٴ°�

    private string _sitParameterName = "@Sit";

    // �ɾƼ� �̵�(105)�ϴ°� ���ο� �ִϸ��̼��� �ʿ������� �Ķ���ʹ� Sit && Move�� ó���� �� �ִ�
    //[SerializeField] private string _waddleParameterName = "Waddle";

    // ��� �� �̵�(107)
    // Move && Attack�� �� �ִϸ��̼��� blend�ؾ� �� �� ����

    // ���� ���� �̵�(109)
    // Move && Aim

    // ���� ���� ���(110)
    // Attack && Aim

    // ���� ���� �̵����(111)
    // Walk&& Attack && Aim 

    // �ɾƼ� ���� ���� 
    // Sit && Attack ���� ó�� ����

    // �Է��� �޴� ���� �ƴ�����, ������ bool���� ���ϴ� ������ �����
    private string _dieParameterName = "Die";


    // string �񱳴� ������ �����Ƿ� int�� �ٲپ� ���Ѵ�
    public int GroundParameterHash { get; set; }
    public int IdleParameterHash { get; set; }
    //public int MoveParameterHash { get; private set; }
    public int MoveXParameterHash { get; set; }
    public int MoveZParameterHash { get; set; }
    public int RunParameterHash { get; set; }
    public int AttackParameterHash { get; set; }
    public int ReloadParameterHash { get; set; }
    public int AimParameterHash { get; set; }
    //
    public int JumpParameterHash { get; set; }
    //
    public int SitParameterHash { get; set; }
    //
    public int DieParameterHash { get; set; }




    private void Start()
    {
        GroundParameterHash = Animator.StringToHash(_groundParameterName);

        //IdleParameterHash = Animator.StringToHash(_idleParameterName);
        //MoveParameterHash = Animator.StringToHash(_moveParameterName);
        MoveXParameterHash = Animator.StringToHash(_moveXParameterName);
        MoveZParameterHash = Animator.StringToHash(_moveZParameterName);
        RunParameterHash = Animator.StringToHash(_runParameterName);
        JumpParameterHash = Animator.StringToHash(_jumpParameterName);
        SitParameterHash = Animator.StringToHash(_sitParameterName);
        AttackParameterHash = Animator.StringToHash(_attackParameterName);
        ReloadParameterHash = Animator.StringToHash(_reloadParameterName);
        AimParameterHash = Animator.StringToHash(_aimParameterName);
        DieParameterHash = Animator.StringToHash(_dieParameterName);
    }

    public override void Spawned()
    {
        base.Spawned();
    }

    public override void Render()
    {
        base.Render();
        playerAnimator.SetFloat(MoveXParameterHash, MoveDirection.x);
        playerAnimator.SetFloat(MoveZParameterHash, MoveDirection.z);

        playerAnimator.SetBool(JumpParameterHash, isJumping);
        playerAnimator.SetBool(ReloadParameterHash, isReloading);
        playerAnimator.SetBool(AttackParameterHash, isFiring);
        playerAnimator.SetBool(RunParameterHash, isRunning);
        playerAnimator.SetBool(SitParameterHash, isSitting);
    }
}