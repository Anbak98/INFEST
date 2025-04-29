using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// PlayerHandlerInput���� ���� ������ �������� ���¸� ������Ʈ(���� Ȥ�� ��ȭ)
/// </summary>
public class PlayerStateMachine : StateMachine
{
    public Player Player { get; }

    // player�� �Է������� �������⸸ �Ѵ�

    // ���µ��� �޾ư� �� �ִ� ������ ����
    public PlayerStatHandler StatHandler { get; set; }

    ///// <summary>
    ///// Animator�� ������ ����
    /// InputHandler�� �����ϱ� �Ʒ��� ������ ��� �ȴ�
    ///// </summary>
    //[field: Header("MoveData")]
    //public Vector3 position;        //�÷��̾� �� ��ġ

    //// look�� ���¿� ������ ���� �ʴ´�
    //public Vector3 lookInput;       //��� ���� �ִ��� ����     
    //public Vector3 moveInput;       //�̵� ���� ����

    //[field: Header("RotateData")]
    //public float rotationX;
    //public float rotationY;         //���콺 ȸ�� ��

    //[field: Header("JumpData")]
    //public bool isJumping;          //���� ������

    //[field: Header("FireData")]
    //public bool isFiring;           //���� ������
    //public bool hitSuccess;         //���ݿ� ���� �ߴ���
    ////public string hitTargetId;    //������ Ÿ���� ID�� ��������

    //[field: Header("SitData")]
    //public bool isSitting;       //�ɾ� �ִ���


    // ���߿� Dictionary�� ���¸� �����ϴ� ������� ������ ����
    // Ground
    public PlayerIdleState IdleState { get; private set; }
    public PlayerMoveState MoveState { get; private set; }
    public PlayerRunState RunState { get; private set; }

    public PlayerAttackState AttackState { get; private set; }
    public PlayerAttackWalkState AttackWalkState { get; private set; }

    public PlayerAimState AimState { get; private set; }
    public PlayerAimWalkState AimWalkState { get; private set; }
    public PlayerAimAttackState AimAttackState { get; private set; }
    public PlayerAimAttackWalkState AimAttackWalkState { get; private set; }

    // Air
    public PlayerJumpState JumpState { get; private set; }
    public PlayerFallState FallState { get; private set; }
    // Sit
    public PlayerSitIdleState SitIdleState { get; private set; }
    public PlayerSitAttackState SitAttackState { get; private set; }
    public PlayerWaddleState WaddleState { get; private set; }
    

    public PlayerStateMachine(Player player, PlayerController controller)
    {
        this.Player = player;
        this.controller = controller;

        StatHandler = player.statHandler;

        // ��������
        IdleState = new PlayerIdleState(controller, this);
        MoveState = new PlayerMoveState(controller, this);
        RunState = new PlayerRunState(controller, this);

        AttackState = new PlayerAttackState(controller, this);
        AttackWalkState = new PlayerAttackWalkState(controller, this);

        AimState = new PlayerAimState(controller, this);
        AimWalkState = new PlayerAimWalkState(controller, this);
        AimAttackState = new PlayerAimAttackState(controller, this);
        AimAttackWalkState = new PlayerAimAttackWalkState(controller, this);

        //ReloadState = new PlayerReloadState(controller, this);
        
        JumpState = new PlayerJumpState(controller, this);
        FallState = new PlayerFallState(controller, this);
        
        SitIdleState = new PlayerSitIdleState(controller, this);
        WaddleState = new PlayerWaddleState(controller, this);
        SitAttackState = new PlayerSitAttackState(controller, this);
        //SitReloadState = new PlayerSitReloadState(controller, this);


        // ó������ IdleState����
        currentState = IdleState;

        //currentState = FallState;         // ó���� ���ִ� ���¿��� ��������
    }
}

