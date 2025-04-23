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
    public PlayerStatHandler StatHandler { get; set; }


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


    //public Transform MainCameraTransform { get; set; }  // Rotation�� �� ī�޶� ���� ȸ���ؾ��Ѵ�

    public PlayerStateMachine(Player player, PlayerController controller)
    {
        this.Player = player;
        this.controller = controller;

        StatHandler = player.statHandler;

        //MainCameraTransform = Camera.main.transform;



        IdleState = new PlayerIdleState(controller, this);
        MoveState = new PlayerMoveState(controller, this);
        RunState = new PlayerRunState(controller, this);
        AttackState = new PlayerAttackState(controller, this);
        ReloadState = new PlayerReloadState(controller, this);

        JumpState = new PlayerJumpState(controller, this);
        FallState = new PlayerFallState(controller, this);

        SitIdleState = new PlayerSitIdleState(controller, this);
        WaddleState = new PlayerWaddleState(controller, this);
        SitAttackState = new PlayerSitAttackState(controller, this);
        SitReloadState = new PlayerSitReloadState(controller, this);


        currentState = IdleState; // Idle ���¿��� ����
    }
}

