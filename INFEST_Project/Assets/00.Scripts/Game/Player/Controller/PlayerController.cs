using UnityEngine;
using Fusion;
using Cinemachine;
using System.Collections.Generic;
using INFEST.Game;

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

    // 리스폰
    [Networked] private TickTimer respawnTimer { get; set; }
    float respawnTime = 10f;

    // 관전모드
    List<CinemachineVirtualCamera> alivePlayerCameras = new List<CinemachineVirtualCamera>();
    public int currentPlayerIndex = 0;
    List<PlayerRef> playerRefs = new List<PlayerRef>();
    private int previousTime = -1;

    public override void Spawned()
    {
        stateMachine = new PlayerStateMachine(player, this);

        player.statHandler.OnHealthChanged += (amount) => player.attackedEffectController.CalledWhenPlayerAttacked(amount);
        player.statHandler.OnDeath += OnDeath;
        player.statHandler.OnRespawn += OnRespawn;

        networkCharacterController.maxSpeed = player.statHandler.CurSpeedMove;

        // 관전모드를 위해 임시
        alivePlayerCameras.Add(player.cameraHandler.virtualCamera);
    }

    public override void FixedUpdateNetwork()
    {
        if (GetInput(out NetworkInputData data))
        {
            DEBUG_DATA = data;
            if (!player.IsDead)   // 죽지 않았을때만 가능한 동작
            {
                HandleMovement(data);
                stateMachine.OnUpdate(data);

                if (data.buttons.IsSet(NetworkInputData.BUTTON_INTERACT) && player.inStoreZoon)
                {

                    if (!player.isInteraction)
                        player.store.RPC_RequestInteraction(Object.InputAuthority);
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
                    player.Weapons.OnThrowGrenade();
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
        // 죽은 경우에 시점 전환
        if (player.IsDead)
        {
            float remaining = respawnTimer.RemainingTime(Runner) ?? 0f;
            // 정수 단위로 바뀔 때만 로그 출력
            int currentTime = Mathf.FloorToInt(remaining);
            if (currentTime != previousTime)
                previousTime = currentTime;
            // 키 입력시 
            if (data.buttons.IsSet(NetworkInputData.BUTTON_CHANGECAMERA))   // Q
            {
                FindAlivePlayers(); // 생존자 갱신
                // 이전 인덱스의 카메라를 가져온다
                if (alivePlayerCameras.Count > 0)
                {
                    currentPlayerIndex = (alivePlayerCameras.Count + currentPlayerIndex - 1) % alivePlayerCameras.Count;
                    SetSpectatorTarget(alivePlayerCameras[currentPlayerIndex]);
                }

            }
            if (data.buttons.IsSet(NetworkInputData.BUTTON_USEHEAL))    // E
            {
                FindAlivePlayers(); // 생존자 갱신
                // 다음 인덱스의 카메라를 가져온다
                if (alivePlayerCameras.Count > 0)
                {
                    currentPlayerIndex = (currentPlayerIndex + 1) % alivePlayerCameras.Count;
                    SetSpectatorTarget(alivePlayerCameras[currentPlayerIndex]);
                }
            }
        }
        // 타이머 만료되면 리스폰 처리
        if (respawnTimer.Expired(player.Runner))
        {
            respawnTimer = TickTimer.None; // 재호출 방지
            OnRespawn();
        }
    }

    #region 부활,관전모드
    private void OnDeath()
    {
        if (alivePlayerCameras.Count > 0)
            FindAlivePlayers(); // 리스트 갱신

        respawnTimer = TickTimer.CreateFromSeconds(player.Runner, respawnTime);    // 5초 타이머 시작

        player.ThirdPersonRoot.SetActive(true);
        stateMachine.ChangeState(stateMachine.DeadState);

        // MeshRenderer 컴포넌트 비활성화
        MeshRenderer[] meshRenderers = player.FirstPersonRoot.GetComponentsInChildren<MeshRenderer>(true);
        foreach (var mr in meshRenderers)
        {
            mr.enabled = false;
        }
        // SkinnedMeshRenderer 컴포넌트 비활성화 (캐릭터 등 스킨드 메시 처리)
        SkinnedMeshRenderer[] skinnedRenderers = player.FirstPersonRoot.GetComponentsInChildren<SkinnedMeshRenderer>(true);
        foreach (var smr in skinnedRenderers)
        {
            smr.enabled = false;
        }

        Debug.Log($"Disabled {meshRenderers.Length} MeshRenderer(s) and {skinnedRenderers.Length} SkinnedMeshRenderer(s).");
    }
    private void OnRespawn()
    {
        Debug.Log("리스폰");
        int randomIndex = UnityEngine.Random.Range(0, NetworkGameManager.Instance.gamePlayers.PlayerSpawnPoints.Count);
        player.transform.position = NetworkGameManager.Instance.gamePlayers.PlayerSpawnPoints[randomIndex].transform.position;

        // MeshRenderer 컴포넌트 활성화
        MeshRenderer[] meshRenderers = player.FirstPersonRoot.GetComponentsInChildren<MeshRenderer>(false);
        foreach (var mr in meshRenderers)
        {
            mr.enabled = true;
        }
        // SkinnedMeshRenderer 컴포넌트 활성화 (캐릭터 등 스킨드 메시 처리)
        SkinnedMeshRenderer[] skinnedRenderers = player.FirstPersonRoot.GetComponentsInChildren<SkinnedMeshRenderer>(false);
        foreach (var smr in skinnedRenderers)
        {
            smr.enabled = true;
        }

        player.IsDead = false;

        player.FirstPersonRoot.SetActive(true);
        player.ThirdPersonRoot.SetActive(false);

        Debug.Log("1");

        player.statHandler.SetHealth(200);

        if (alivePlayerCameras.Count > 0)
            ResetSpectatorTarget(alivePlayerCameras[currentPlayerIndex]);

        stateMachine.ChangeState(stateMachine.IdleState);
    }

    public void FindAlivePlayers()
    {
        // 현재 접속중인 플레이어 정보들 저장
        playerRefs = NetworkGameManager.Instance.gamePlayers.GetPlayerRefs();

        // CinemachineVirtualCamera의 priority를 0초기화
        foreach (var virtualCamera in alivePlayerCameras)
        {
            virtualCamera.Priority = 0;
        }
        alivePlayerCameras.Clear();

        // playerRefs에 있는 플레이어들의 virtualCamera들 저장
        foreach (var playerRef in playerRefs)
        {
            // NetworkObject 가져오기

            // Player 컴포넌트 접근
            Player otherPlayer = NetworkGameManager.Instance.gamePlayers.GetPlayerObj(playerRef);
            if (otherPlayer != null && otherPlayer.statHandler.CurHealth > 0)
            {
                PlayerCameraHandler otherCamHandler = otherPlayer.GetComponentInChildren<PlayerCameraHandler>();
                alivePlayerCameras.Add(otherCamHandler.virtualCamera);
            }
        }
    }
    public void SetSpectatorTarget(CinemachineVirtualCamera targetCam)
    {
        if (targetCam == null) return;
        // 자신의 virtualcamera의 priority를 0으로 
        player.FirstPersonCamera.GetComponent<CinemachineVirtualCamera>().Priority = 0;

        // 타겟의 우선순위를 높이면 자동으로 Live
        targetCam.Priority = 100;

        // targetCam을 가진 오브젝트의 3인칭 프리팹 비활성화, 1인칭 프리팹 활성화
        var targetPlayer = targetCam.GetComponentInParent<Player>();
        if (targetPlayer != null)
        {
            targetPlayer.FirstPersonRoot.SetActive(true);
            targetPlayer.ThirdPersonRoot.SetActive(false);
        }
    }
    public void ResetSpectatorTarget(CinemachineVirtualCamera targetCam)
    {
        if (targetCam == null) return;
        // 우선순위를 낮추면 자동으로 Standby
        targetCam.Priority = 0;

        // 자신의 우선순위 100으로 
        player.FirstPersonCamera.GetComponent<CinemachineVirtualCamera>().Priority = 100;

        // targetCam을 가진 오브젝트의 3인칭 프리팹 활성화, 1인칭 프리팹 비활성화
        var targetPlayer = targetCam.GetComponentInParent<Player>();
        if (targetPlayer != null)
        {
            targetPlayer.FirstPersonRoot.SetActive(false);
            targetPlayer.ThirdPersonRoot.SetActive(true);
        }
    }
    #endregion



    #region 이동, 점프
    // 플레이어의 이동(방향은 CameraHandler에서 설정) 처리. 그 방향이 transform.forward로 이미 설정되었다
    private void HandleMovement(NetworkInputData data)
    {
        if (LockState == PlayerLockState.MoveLock)
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
        if (!player.isInteraction)
        {
            if (data.buttons.IsSet(NetworkInputData.BUTTON_RELOAD))
            {

                player.Weapons.Reload();

                //delay = TickTimer.CreateFromSeconds(Runner, 0.5f);
            }
        }
    }

    // 플레이어가 땅 위에 있는지?
    public bool IsGrounded() => networkCharacterController.Grounded;
    public float GetVerticalVelocity() => verticalVelocity;
    #endregion
    private void OnGUI()
    {
        if (HasInputAuthority)
        {
            //GUILayout.Label(stateMachine.currentState.ToString());
            //GUILayout.Label(DEBUG_DATA.ToString());
            ////
            //GUILayout.Label("Player HP: " + player.statHandler.CurHealth.ToString());
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
