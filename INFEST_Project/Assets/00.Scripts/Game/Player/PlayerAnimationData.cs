using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

// 컴포넌트가 아니므로 Serializable 선언이 되어 있어야 인스펙터 창에 표시가능
[Serializable]
public class PlayerAnimationData
{
    [SerializeField] private string _idleParameterName = "Idle";
    //[SerializeField] private string _moveParameterName = "Move";    // walk
    [SerializeField] private string _moveXParameterName = "MoveX";    // walk
    [SerializeField] private string _moveZParameterName = "MoveZ";    // walk


    [SerializeField] private string _runParameterName = "Run";

    [SerializeField] private string _jumpParameterName = "Jump";

    // 떨어지는 상태는 jump상태와 분명히 다르다
    // isGround가 false && isjumping이 false인 것으로 표현할 수 있어서 파라미터는 없어도 된다
    // 문제는 parameter가 없을때 idle과 구분해야한다는거
    //[SerializeField] private string _fallParameterName = "Fall";    

    [SerializeField] private string _sitParameterName = "Sit";

    // 앉아서 이동(105)하는건 새로운 애니메이션이 필요하지만 파라미터는 Sit && Move로 처리할 수 있다
    //[SerializeField] private string _waddleParameterName = "Waddle";

    [SerializeField] private string _attackParameterName = "Fire";
    [SerializeField] private string _reloadParameterName = "IsReloading";    // 장전하는 상태가 있어야 할 것 같기도 하다

    // 사격 중 이동(107)
    // Move && Attack일 때 애니메이션을 blend해야 할 것 같다

    [SerializeField] private string _aimParameterName = "Aim";

    // 조준 상태 이동(109)
    // Move && Aim

    // 조준 상태 사격(110)
    // Attack && Aim

    // 조준 상태 이동사격(111)
    // Walk&& Attack && Aim 

    [SerializeField] private string _dieParameterName = "Die";


    // string 비교는 연산이 많으므로 int로 바꾸어 비교한다
    public int IdleParameterHash { get; private set; }
    //public int MoveParameterHash { get; private set; }
    public int MoveXParameterHash { get; private set; }
    public int MoveZParameterHash { get; private set; }
    public int RunParameterHash { get; private set; }
    //
    public int JumpParameterHash { get; private set; }
    //
    public int SitParameterHash { get; private set; }
    //
    public int AttackParameterHash { get; private set; }
    public int ReloadParameterHash { get; private set; }

    public int AimParameterHash { get; private set; }

    public int DieParameterHash { get; private set; }


    public void Initialize()
    {
        IdleParameterHash = Animator.StringToHash(_idleParameterName);
        //MoveParameterHash = Animator.StringToHash(_moveParameterName);
        MoveXParameterHash = Animator.StringToHash(_moveXParameterName);
        MoveZParameterHash = Animator.StringToHash(_moveZParameterName);
        RunParameterHash = Animator.StringToHash(_runParameterName);
        //
        JumpParameterHash = Animator.StringToHash(_jumpParameterName);
        //
        SitParameterHash = Animator.StringToHash(_sitParameterName);
        //
        AttackParameterHash = Animator.StringToHash(_attackParameterName);
        ReloadParameterHash = Animator.StringToHash(_reloadParameterName);

        AimParameterHash = Animator.StringToHash(_aimParameterName);

        DieParameterHash = Animator.StringToHash(_dieParameterName);
    }
}

/*
// 상태(애니메이션)
100 Idle 기본 상태
101	Move	이동
102	Run	달리기
103	Jump	뛰기
104	Sit	앉기
105	Waddle	앉아서 이동
106	Attack	사격
107	AttackWalk	사격 중 이동
108	Aim	조준
109	AimWalk	조준 상태 이동
110	AimAttack	조준 상태 사격
111	AimAttackWalk	조중 상태 이동사격
112	Die	사망
*/