using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// PlayerHandlerInput에서 받은 정보를 바탕으로 상태를 업데이트(유지 혹은 변화)
/// </summary>
public class PlayerStateMachine : StateMachine
{
    public Player Player { get; }
    // player의 입력정보를 가져오기만 한다
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


    //public Transform MainCameraTransform { get; set; }  // Rotation할 때 카메라도 같이 회전해야한다

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


        currentState = IdleState; // Idle 상태에서 시작
    }
}

