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
    public PlayerReloadState ReloadState { get; private set; }
    // Air
    public PlayerJumpState JumpState { get; private set; }
    public PlayerFallState FallState { get; private set; }
    // Sit
    public PlayerSitIdleState SitIdleState { get; private set; }
    public PlayerSitAttackState SitAttackState { get; private set; }
    public PlayerSitReloadState SitReloadState { get; private set; }
    public PlayerWaddleState WaddleState { get; private set; }
    

    public PlayerStateMachine(Player player, PlayerController controller, InputManager inputManager)
    {
        this.Player = player;
        this.controller = controller;

        StatHandler = player.statHandler;

        // ��������
        IdleState = new PlayerIdleState(controller, this, inputManager);
        MoveState = new PlayerMoveState(controller, this, inputManager);
        RunState = new PlayerRunState(controller, this, inputManager);
        AttackState = new PlayerAttackState(controller, this, inputManager);
        ReloadState = new PlayerReloadState(controller, this, inputManager);
        
        JumpState = new PlayerJumpState(controller, this, inputManager);
        FallState = new PlayerFallState(controller, this, inputManager);
        
        SitIdleState = new PlayerSitIdleState(controller, this, inputManager);
        WaddleState = new PlayerWaddleState(controller, this, inputManager);
        SitAttackState = new PlayerSitAttackState(controller, this, inputManager);
        SitReloadState = new PlayerSitReloadState(controller, this, inputManager);


        // ó������ IdleState����
        currentState = IdleState; // �Ķ���Ͱ� ��� ��������� ����
        // Player���� �������ִϱ� ���¸ӽſ� �������� �ڲ� currentState�� null
    }
}

