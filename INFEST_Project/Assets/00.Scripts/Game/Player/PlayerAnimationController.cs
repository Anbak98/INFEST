using Fusion;
using UnityEngine;

public class PlayerAnimationController : NetworkBehaviour
{
    public Animator playerAnimator;
    [Networked] public Vector3 MoveDirection { get; set; }

    private string _groundParameterName = "@Ground";    // 
    private string _idleParameterName = "Idle";
    //private string _moveParameterName = "Move";    // walk
    private string _moveXParameterName = "MoveX";    // walk
    private string _moveZParameterName = "MoveZ";    // walk
    private string _runParameterName = "Run";
    private string _attackParameterName = "Fire";
    private string _reloadParameterName = "IsReloading";    // 장전하는 상태가 있어야 할 것 같기도 하다
    private string _aimParameterName = "Aim";


    private string _airParameterName = "@Air";
    private string _jumpParameterName = "Jump";
    private string _fallParameterName = "Fall";
    // jump상태와 분명히 다르다
    // isGround가 false && isjumping이 false인 것으로 표현할 수 있어서 파라미터는 없어도 된다
    // 문제는 parameter가 없을때 idle과 구분해야한다는거

    private string _sitParameterName = "@Sit";

    // 앉아서 이동(105)하는건 새로운 애니메이션이 필요하지만 파라미터는 Sit && Move로 처리할 수 있다
    //[SerializeField] private string _waddleParameterName = "Waddle";


    // 사격 중 이동(107)
    // Move && Attack일 때 애니메이션을 blend해야 할 것 같다


    // 조준 상태 이동(109)
    // Move && Aim

    // 조준 상태 사격(110)
    // Attack && Aim

    // 조준 상태 이동사격(111)
    // Walk&& Attack && Aim 

    // 앉아서 공격 또한 
    // Sit && Attack 으로 처리 가능

    private string _dieParameterName = "Die";


    // string 비교는 연산이 많으므로 int로 바꾸어 비교한다
    public int GroundParameterHash { get; private set; }
    public int IdleParameterHash { get; private set; }
    //public int MoveParameterHash { get; private set; }
    public int MoveXParameterHash { get; private set; }
    public int MoveZParameterHash { get; private set; }
    public int RunParameterHash { get; private set; }
    public int AttackParameterHash { get; private set; }
    public int ReloadParameterHash { get; private set; }
    public int AimParameterHash { get; private set; }
    //
    public int JumpParameterHash { get; private set; }
    //
    public int SitParameterHash { get; private set; }
    //
    public int DieParameterHash { get; private set; }

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
    }
}