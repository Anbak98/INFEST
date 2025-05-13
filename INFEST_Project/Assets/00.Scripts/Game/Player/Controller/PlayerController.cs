using UnityEngine;
using Fusion;

/// <summary>
/// 캐릭터 동작 처리를 한다
/// 
/// InputAction의 이벤트메서드를 연결한다
/// 플레이어의 FSM은 네트워크에서 동기화된 입력 데이터를 기반으로 상태 전환
/// 
/// 플레이어의 동작 및 상태 관리
/// FixedUpdateNetwork()에서 Fusion으로부터 받은 입력 데이터를 기반으로 시뮬레이션 수행.
/// </summary>
public enum PlayerLockState
{ 
    Free,
    MoveLock
}

public class PlayerController : NetworkBehaviour
{
    NetworkInputData DEBUG_DATA;

    public PlayerLockState LockState = PlayerLockState.Free;

    // 동적 연결되는 변수 숨기기
    public Player player;
    public PlayerStateMachine stateMachine;
    public PlayerCameraHandler cameraHandler;
    public NetworkCharacterController networkCharacterController;

    protected float verticalVelocity;
    //protected float gravity = -9.81f; // player.networkCharacterController.gravity를 사용해야한다

    public string playerId;
    protected bool hitSuccess;
    protected string hitTatgetId;
    [Networked] protected TickTimer delay { get; set; }

    public override void Spawned()
    {
        stateMachine = new PlayerStateMachine(player, this);

        player.statHandler.OnHealthChanged += (amount) => player.attackedEffectController.CalledWhenPlayerAttacked(amount);
        player.statHandler.OnDeath += OnDeath;
        player.statHandler.OnRespawn += OnRespawn;
    }

    public override void FixedUpdateNetwork()
    {
        if (GetInput(out NetworkInputData data))
        {
            DEBUG_DATA = data;
            HandleMovement(data);
            stateMachine.OnUpdate(data);

            if (data.buttons.IsSet(NetworkInputData.BUTTON_INTERACT) && player.inStoreZoon)
            {

                if (!player.isInteraction)
                    player.store.RPC_RequestInteraction(player, Object.InputAuthority);
                else 
                    player.store.RPC_RequestStopInteraction(Object.InputAuthority);

                player.isInteraction = !player.isInteraction;

            }

            if (data.scrollValue.y != 0)
            {
                player.Weapons.Swap(data.scrollValue.y);
            }

            if (data.buttons.IsSet(NetworkInputData.BUTTON_ZOOM))
            {
                player.Weapons.Aiming(true);
            }
            if (data.buttons.IsSet(NetworkInputData.BUTTON_ZOOMPRESSED))
            {
                player.Weapons.Aiming(false);
            }

            if (data.buttons.IsSet(NetworkInputData.BUTTON_USEGRENAD))
            {
                player.Weapons.ThrowGrenade();
                player.Consumes.Throw();
            }

            if (data.buttons.IsSet(NetworkInputData.BUTTON_USEHEAL))
            {
                player.Consumes.Heal();
            }

            if (data.buttons.IsSet(NetworkInputData.BUTTON_USESHIELD))
            {
                player.Consumes.Mounting();
            }
        }
    }

    // 플레이어의 이동(방향은 CameraHandler에서 설정) 처리. 그 방향이 transform.forward로 이미 설정되었다
    private void HandleMovement(NetworkInputData data)
    {
        if(LockState == PlayerLockState.MoveLock)
        {
            networkCharacterController.Move(
                Vector3.zero
            );
        }
        else
        {
            Vector3 input = data.direction;
            player.Weapons.OnMoveAnimation(input);

            // 카메라 기준 방향 가져오기
            Vector3 camForward = player.cameraHandler.GetCameraForwardOnXZ();
            Vector3 camRight = player.cameraHandler.GetCameraRightOnXZ();
            Vector3 moveDir = (camRight * input.x + camForward * input.z).normalized;

            moveDir.y = 0f; // 수직 방향 제거

            networkCharacterController.Move(
                moveDir
            );

            // 회전 강제 고정: 카메라가 지정한 forward로
            player.transform.forward = camForward;
        }
    }

    private void OnDeath()
    {
        player.FirstPersonRoot.SetActive(false);
        player.ThirdPersonRoot.SetActive(true);
        stateMachine.ChangeState(stateMachine.DeadState);
    }
    private void OnRespawn()
    {
        player.FirstPersonRoot.SetActive(true);
        player.ThirdPersonRoot.SetActive(false);
        stateMachine.ChangeState(stateMachine.IdleState);
    }

    public void ApplyGravity()
    {
        if (IsGrounded())
        {
            verticalVelocity = 0f;
        }
        else
        {
            verticalVelocity += networkCharacterController.gravity * Time.deltaTime;
        }

        networkCharacterController.Jump(false, verticalVelocity);
    }
    /// <summary>
    /// 점프 시작 시 수직 속도 계산
    /// </summary>
    public void StartJump()
    {
        //verticalVelocity = Mathf.Sqrt(player.statHandler.JumpPower * -2f * gravity);

        //verticalVelocity = Mathf.Sqrt(player.statHandler.JumpPower * -2f * player.networkCharacterController.gravity);
        verticalVelocity = Mathf.Sqrt(networkCharacterController.jumpImpulse * -1f * networkCharacterController.gravity);
        // 땅에서 떨어졌으므로 Grounded를 false로 강제변경
        //SetGrounded(false);

        networkCharacterController.Jump(false, verticalVelocity);
    }

    // 앉는다
    public void StartSit()
    {
        // collider는 상태에서 변화시키므로 여기서는 transform만 아래로
        float playerYpos = player.transform.position.y;
        playerYpos /= 2;
        player.transform.position = new Vector3(player.transform.position.x, player.transform.position.y - playerYpos + 0.01f, player.transform.position.z);
    }

    // 일어난다
    public void StartStand()
    {
        // collider는 상태에서 변화시키므로 여기서는 transform만 아래로
        float playerYpos = player.transform.position.y;
        playerYpos *= 2;
        player.transform.position = new Vector3(player.transform.position.x, player.transform.position.y + playerYpos + 0.01f, player.transform.position.z);
    }

    public void StartFire(NetworkInputData data)
    {
        // 네트워크 객체는 StateAuthority(호스트)만 생성할 수 있기 때문에 StateAuthority에 대한 확인이 필요
        // 호스트에서만 실행되고 클라이언트에서는 예측되지 않는다0
        if (HasStateAuthority && delay.ExpiredOrNotRunning(Runner) && !player.isInteraction)
        {
            // 마우스 좌클릭(공격)
            if (data.buttons.IsSet(NetworkInputData.BUTTON_FIRE))
            {
                //data.isShotgunOnFiring = true;
                player.Weapons.Fire(data.buttons.IsSet(NetworkInputData.BUTTON_FIREPRESSED));

                //delay = TickTimer.CreateFromSeconds(Runner, 0.5f);
            }
        }
    }

    public void StartReload(NetworkInputData data)
    {
        // TODO
    }

    // 플레이어가 땅 위에 있는지?
    public bool IsGrounded() => networkCharacterController.Grounded;
    public float GetVerticalVelocity() => verticalVelocity;
    private void OnGUI()
    {
        if (HasInputAuthority)
        {
            //GUILayout.Label(stateMachine.currentState.ToString());
            //GUILayout.Label(DEBUG_DATA.ToString());
            ////
            //GUILayout.Label("Player HP: " + player.statHandler.CurrentHealth.ToString());
            //GUILayout.Label("PlayerController position: " + transform.position.ToString());
            //GUILayout.Label("PlayerController rotation: " + transform.rotation.ToString());
            //GUILayout.Label("CameraHandler position: " + cameraHandler.transform.position.ToString());
            //GUILayout.Label("CameraHandler rotation: " + cameraHandler.transform.rotation.ToString());
            ////
            //GUILayout.Label("Grounded: " + networkCharacterController.Grounded.ToString());
            //GUILayout.Label("Equip: " + stateMachine.Player.GetWeapons()?.CurrentWeapon);
        }
    }
}
