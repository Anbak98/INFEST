using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using UnityEngine.InputSystem;
using UnityEngine.Playables;
using UnityEngine.InputSystem.XR;
using UnityEngine.Windows;
using UnityEditor;
//using static UnityEditor.Experimental.GraphView.GraphView;


/// <summary>
/// 캐릭터 동작 처리를 한다
/// 
/// InputAction의 이벤트메서드를 연결한다
/// 플레이어의 FSM은 네트워크에서 동기화된 입력 데이터를 기반으로 상태 전환
/// 
/// 플레이어의 동작 및 상태 관리
/// FixedUpdateNetwork()에서 Fusion으로부터 받은 입력 데이터를 기반으로 시뮬레이션 수행.
/// </summary>
public class PlayerController : BaseController
{
    // 동적 연결되는 변수 숨기기
    public Player player;
    public WeaponSpawner weapons;

    public PlayerCameraHandler cameraHandler;

    protected float verticalVelocity;
    //protected float gravity = -9.81f; // player.networkCharacterController.gravity를 사용해야한다

    public string playerId;
    protected bool hitSuccess;
    protected string hitTatgetId;

    public override void Awake()
    {
        weapons = player.GetWeapons();
        stateMachine = new PlayerStateMachine(player, this);
    }

    // 플레이어가 땅 위에 있는지?
    public override bool IsGrounded() => player.networkCharacterController.Grounded;
    public override float GetVerticalVelocity() => verticalVelocity;

    // 상태가 바뀌면 NetworkCharacterController.Grounded의 시점을 강제로 맞춘다
    public override void SetGrounded(bool b)
    {
        player.networkCharacterController.Grounded = b;
    }

    // 플레이어의 이동(방향은 CameraHandler에서 설정) 처리. 그 방향이 transform.forward로 이미 설정되었다
    public override void HandleMovement(NetworkInputData data)
    {
        Vector3 input = data.direction;
        weapons.OnMoveAnimation(input);

        // 카메라 기준 방향 가져오기
        Vector3 camForward = player.cameraHandler.GetCameraForwardOnXZ();
        Vector3 camRight = player.cameraHandler.GetCameraRightOnXZ();
        Vector3 moveDir = (camRight * input.x + camForward * input.z).normalized;

        //Vector3 forward = transform.forward;
        //Vector3 right = transform.right;
        //Vector3 moveDir = (right * input.x + forward * input.z).normalized;
        moveDir.y = 0f; // 수직 방향 제거
        // 회전은 막고, 이동만 한다
        // xz평면상에서만 이동해야한다
        player.networkCharacterController.Move(
            moveDir * player.statHandler.MoveSpeedModifier
        );

        // 회전 강제 고정: 카메라가 지정한 forward로
        player.transform.forward = camForward;
        //player.transform.forward = forward;
    }

    public override void ApplyGravity()
    {
        if (IsGrounded())
        {
            verticalVelocity = 0f;
        }
        else
        {
            // 해당 gravity를 사용해서 점프력의 차이가 발생했던 것
            // NetworkCharacterController의 gravity를 사용해야 한다
            //player.networkCharacterController.gravity
            //verticalVelocity += gravity * Time.deltaTime;
            verticalVelocity += player.networkCharacterController.gravity * Time.deltaTime;
        }

        player.networkCharacterController.Jump(false, verticalVelocity);
    }
    /// <summary>
    /// 점프 시작 시 수직 속도 계산
    /// </summary>
    public override void StartJump()
    {
        //verticalVelocity = Mathf.Sqrt(player.statHandler.JumpPower * -2f * gravity);

        //verticalVelocity = Mathf.Sqrt(player.statHandler.JumpPower * -2f * player.networkCharacterController.gravity);
        verticalVelocity = Mathf.Sqrt(player.networkCharacterController.jumpImpulse * -1f * player.networkCharacterController.gravity);
        // 땅에서 떨어졌으므로 Grounded를 false로 강제변경
        //SetGrounded(false);

        player.networkCharacterController.Jump(false, verticalVelocity);
    }

    // 앉는다
    public override void StartSit()
    {
        // collider는 상태에서 변화시키므로 여기서는 transform만 아래로
        float playerYpos = player.transform.position.y;
        playerYpos /= 2;
        player.transform.position = new Vector3(player.transform.position.x, player.transform.position.y - playerYpos + 0.01f, player.transform.position.z);
    }
    // 일어난다
    public override void StartStand()
    {
        // collider는 상태에서 변화시키므로 여기서는 transform만 아래로
        float playerYpos = player.transform.position.y;
        playerYpos *= 2;
        player.transform.position = new Vector3(player.transform.position.x, player.transform.position.y + playerYpos + 0.01f, player.transform.position.z);
    }

    public override void StartFire(NetworkInputData data)
    {
        // 네트워크 객체는 StateAuthority(호스트)만 생성할 수 있기 때문에 StateAuthority에 대한 확인이 필요
        // 호스트에서만 실행되고 클라이언트에서는 예측되지 않는다0
        if (HasStateAuthority && delay.ExpiredOrNotRunning(Runner) && !player.isInteraction)
        {
            // 마우스 좌클릭(공격)
            if (data.buttons.IsSet(NetworkInputData.BUTTON_FIRE))
            {
                //data.isShotgunOnFiring = true;
                weapons.Fire(data.buttons.IsSet(NetworkInputData.BUTTON_FIREPRESSED));

                //delay = TickTimer.CreateFromSeconds(Runner, 0.5f);
            }
        }
    }

    public override void StartReload(NetworkInputData data)
    {
        // TODO
    }
}
