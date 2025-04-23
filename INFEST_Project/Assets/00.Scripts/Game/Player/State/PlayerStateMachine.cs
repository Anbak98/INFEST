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

    // 나중에 Dictionary에 상태를 관리하는 방식으로 수정할 예정
    public PlayerIdleState IdleState { get; private set; }
    public PlayerWalkState WalkState { get; private set; }
    public PlayerRunState RunState { get; private set; }
    public PlayerFireState FireState { get; private set; }
    public PlayerReloadState ReloadState { get; private set; }
    public PlayerJumpState JumpState { get; private set; }
    public PlayerFallState FallState { get; private set; }

    // 카메라의 회전상태 또한 여기에서...? 조금 더 고민해본다 
    public Transform MainCameraTransform { get; set; }  // Rotation할 때 카메라도 같이 회전해야한다

    public PlayerStateMachine(Player player, PlayerController controller)
    {
        this.Player = player;

        MainCameraTransform = Camera.main.transform;

        // StateMachine에서 BaseController타입을 사용하므로, 이를 전달받는 곳에서 사용하려면 형변환 필요
        // 제네릭으로 안전하게 캐스팅
        var playerController = GetController<PlayerController>();

        // 수정사항
        IdleState = new PlayerIdleState(playerController, this);
        WalkState = new PlayerWalkState(playerController, this);
        RunState = new PlayerRunState(playerController, this);
        JumpState = new PlayerJumpState(playerController, this);
        FallState = new PlayerFallState(playerController, this);

        // 여기에서는 변수만 지정하는데, PlayerInputHandler가 플레이어의 입력값을 저장하고 있으므로 아래의 내용은 없어야한다
        //MovementSpeed = player.Data.GroundData.BaseSpeed;
        //RotationDamping = player.Data.GroundData.BaseRotationDamping;
    }
    // ChangeState, Update 등의 메서드는 부모인 StateMachine에 정의한 것을 사용한다
    protected override void UpdateStateTransition()
    {
        // 입력값에 따른 상태 전이


    }
    // 상태 전이 함수들

}

