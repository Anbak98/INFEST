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
    public PlayerInputHandler InputHandler { get; }

    // 상태들이 받아갈 수 있는 값들을 관리
    public PlayerStatHandler StatHandler { get; set; }

    ///// <summary>
    ///// Animator에 전달할 정보
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

        // StateMachine에서 BaseController타입을 사용하므로, 이를 전달받는 곳에서 사용하려면 형변환 필요
        // 제네릭으로 안전하게 캐스팅
        var playerController = GetController<PlayerController>();

        // 수정사항
        IdleState = new PlayerIdleState(playerController, this);
        MoveState = new PlayerMoveState(playerController, this);
        RunState = new PlayerRunState(playerController, this);

        AttackState = new PlayerAttackState(playerController, this);
        ReloadState = new PlayerReloadState(playerController, this);

        JumpState = new PlayerJumpState(playerController, this);
        FallState = new PlayerFallState(playerController, this);

        SitState = new PlayerSitState(playerController, this);

    }
    //// ChangeState, Update 등의 메서드는 부모인 StateMachine에 정의한 것을 사용한다
    //protected override void UpdateStateTransition()
    //{
    //    // 입력값에 따른 상태 전이


    //}
    //// 상태 전이 함수들

}

