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

        // StateMachine���� BaseControllerŸ���� ����ϹǷ�, �̸� ���޹޴� ������ ����Ϸ��� ����ȯ �ʿ�
        // ���׸����� �����ϰ� ĳ����
        var playerController = GetController<PlayerController>();

        // ��������
        IdleState = new PlayerIdleState(playerController, this);
        MoveState = new PlayerMoveState(playerController, this);
        RunState = new PlayerRunState(playerController, this);

        AttackState = new PlayerAttackState(playerController, this);
        ReloadState = new PlayerReloadState(playerController, this);

        JumpState = new PlayerJumpState(playerController, this);
        FallState = new PlayerFallState(playerController, this);

        SitState = new PlayerSitState(playerController, this);

    }
    //// ChangeState, Update ���� �޼���� �θ��� StateMachine�� ������ ���� ����Ѵ�
    //protected override void UpdateStateTransition()
    //{
    //    // �Է°��� ���� ���� ����


    //}
    //// ���� ���� �Լ���

}

