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

    // 상태들이 받아갈 수 있는 값들을 관리
    public PlayerStatHandler StatHandler { get; set; }

    ///// <summary>
    ///// Animator에 전달할 정보
    /// InputHandler에 있으니까 아래의 내용은 없어도 된다
    ///// </summary>
    //[field: Header("MoveData")]
    //public Vector3 position;        //플레이어 현 위치

    //// look은 상태에 영향을 주지 않는다
    //public Vector3 lookInput;       //어디를 보고 있는지 벡터     
    //public Vector3 moveInput;       //이동 중인 벡터

    //[field: Header("RotateData")]
    //public float rotationX;
    //public float rotationY;         //마우스 회전 값

    //[field: Header("JumpData")]
    //public bool isJumping;          //점프 중인지

    //[field: Header("FireData")]
    //public bool isFiring;           //공격 중인지
    //public bool hitSuccess;         //공격에 성공 했는지
    ////public string hitTargetId;    //공격한 타겟의 ID는 무엇인지

    //[field: Header("SitData")]
    //public bool isSitting;       //앉아 있는지


    // 나중에 Dictionary에 상태를 관리하는 방식으로 수정할 예정
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

        // 수정사항
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


        // 처음에는 IdleState시작
        currentState = IdleState;

        //currentState = FallState;         // 처음에 떠있는 상태에서 떨어진다
    }
}

