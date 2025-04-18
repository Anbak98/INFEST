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
    public PlayerInputHandler InputHandler { get; }

    // ���µ��� �޾ư� �� �ִ� ������ ����
    public PlayerStatHandler StatHandler { get; set; }

    ///// <summary>
    ///// Animator�� ������ ����
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
    public PlayerIdleState IdleState { get; private set; }
    public PlayerMoveState MoveState { get; private set; }
    public PlayerRunState RunState { get; private set; }
    public PlayerAttackState AttackState { get; private set; }
    public PlayerReloadState ReloadState { get; private set; }
    public PlayerJumpState JumpState { get; private set; }
    public PlayerFallState FallState { get; private set; }
    public PlayerSitState SitState { get; private set; }


    public PlayerStateMachine(Player player, PlayerController controller)
    {
        this.Player = player;

        StatHandler = player.statHandler;
        InputHandler = player.Input;

        // ��������
        IdleState = new PlayerIdleState(controller, this);
        MoveState = new PlayerMoveState(controller, this);
        RunState = new PlayerRunState(controller, this);

        AttackState = new PlayerAttackState(controller, this);
        ReloadState = new PlayerReloadState(controller, this);

        JumpState = new PlayerJumpState(controller, this);
        FallState = new PlayerFallState(controller, this);

        SitState = new PlayerSitState(controller, this);

        // ó������ IdleState����
        currentState = IdleState; // �Ķ���Ͱ� ��� ��������� ����
    }
}

