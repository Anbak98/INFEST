using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

// ������Ʈ�� �ƴϹǷ� Serializable ������ �Ǿ� �־�� �ν����� â�� ǥ�ð���
[Serializable]
public class PlayerAnimationData
{
    [SerializeField] private string _idleParameterName = "Idle";
    //[SerializeField] private string _moveParameterName = "Move";    // walk
    [SerializeField] private string _moveXParameterName = "MoveX";    // walk
    [SerializeField] private string _moveZParameterName = "MoveZ";    // walk


    [SerializeField] private string _runParameterName = "Run";

    [SerializeField] private string _jumpParameterName = "Jump";

    // �������� ���´� jump���¿� �и��� �ٸ���
    // isGround�� false && isjumping�� false�� ������ ǥ���� �� �־ �Ķ���ʹ� ��� �ȴ�
    // ������ parameter�� ������ idle�� �����ؾ��Ѵٴ°�
    //[SerializeField] private string _fallParameterName = "Fall";    

    [SerializeField] private string _sitParameterName = "Sit";

    // �ɾƼ� �̵�(105)�ϴ°� ���ο� �ִϸ��̼��� �ʿ������� �Ķ���ʹ� Sit && Move�� ó���� �� �ִ�
    //[SerializeField] private string _waddleParameterName = "Waddle";

    [SerializeField] private string _attackParameterName = "Fire";
    [SerializeField] private string _reloadParameterName = "IsReloading";    // �����ϴ� ���°� �־�� �� �� ���⵵ �ϴ�

    // ��� �� �̵�(107)
    // Move && Attack�� �� �ִϸ��̼��� blend�ؾ� �� �� ����

    [SerializeField] private string _aimParameterName = "Aim";

    // ���� ���� �̵�(109)
    // Move && Aim

    // ���� ���� ���(110)
    // Attack && Aim

    // ���� ���� �̵����(111)
    // Walk&& Attack && Aim 

    [SerializeField] private string _dieParameterName = "Die";


    // string �񱳴� ������ �����Ƿ� int�� �ٲپ� ���Ѵ�
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
// ����(�ִϸ��̼�)
100 Idle �⺻ ����
101	Move	�̵�
102	Run	�޸���
103	Jump	�ٱ�
104	Sit	�ɱ�
105	Waddle	�ɾƼ� �̵�
106	Attack	���
107	AttackWalk	��� �� �̵�
108	Aim	����
109	AimWalk	���� ���� �̵�
110	AimAttack	���� ���� ���
111	AimAttackWalk	���� ���� �̵����
112	Die	���
*/