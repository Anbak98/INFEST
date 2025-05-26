using UnityEngine;
using Fusion;
using Cinemachine;
using System.Collections.Generic;
using INFEST.Game;
using Fusion.Addons.SimpleKCC;
using UnityEngine.Windows;
using Cinemachine.Utility;

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
    [SerializeField] private SimpleKCC _simpleKCC;
    public Transform CameraHandle;

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
    // 죽은 것만으로는 관전상태가 되지 않으므로 구분해준다
    public bool isSpectating = false;

    [SerializeField] private List<CinemachineVirtualCamera> alivePlayerCameras = new List<CinemachineVirtualCamera>();
    public int currentPlayerIndex = 0;  // 플레이어마다 다른 값을 가진다(확인했다)
    List<PlayerRef> playerRefs = new List<PlayerRef>();
    private int previousTime = -1;
    public CinemachineVirtualCamera curSpectatorCam;   // 현재 관전중인 대상
    private bool shouldChangeTarget = false;    // Q, E 입력여부(별도의 키 입력 없이 이번 프레임의 Render에서 호출)

    public void Init()
    {
        stateMachine = new PlayerStateMachine(player, this);

        player.statHandler.OnHealthChanged += (amount) => player.attackedEffectController.CalledWhenPlayerAttacked(amount);
        player.statHandler.OnDeath += OnDeath;
        player.statHandler.OnRespawn += OnRespawn;

        // 관전모드를 위해 임시
        alivePlayerCameras.Add(player.cameraHandler.spectatorCamera);
        // Set custom gravity.
        _simpleKCC.SetGravity(Physics.gravity.magnitude * -4.0f);
    }

    public override void FixedUpdateNetwork()
    {
        if (GetInput(out NetworkInputData data))
        {
            DEBUG_DATA = data;
            if (!player.statHandler.IsDead)   // 죽지 않았을때만 가능한 동작
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

                if (data.buttons.IsSet(NetworkInputData.BUTTON_MENU) && player.isInteraction && player.inStoreZoon)
                {
                    player.store.RPC_RequestStopInteraction(Object.InputAuthority);
                    player.isInteraction = !player.isInteraction;
                }

                if (data.buttons.IsSet(NetworkInputData.BUTTON_INTERACT) && player.inMysteryBoxZoon)
                {
                    if (!player.isInteraction)
                        player.mysteryBox.RPC_RequestInteraction(Object.InputAuthority);
                    else
                        player.mysteryBox.RPC_RequestStopInteraction(Object.InputAuthority);

                    player.isInteraction = !player.isInteraction;

                }

                if (data.buttons.IsSet(NetworkInputData.BUTTON_MENU) && player.isInteraction && player.inMysteryBoxZoon)
                {
                    player.mysteryBox.RPC_RequestStopInteraction(Object.InputAuthority);
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
        // 죽은 경우에 시점 전환(로컬 처리)
        if (player.statHandler.IsDead)
        {
            // 키 입력시 
            if (data.buttons.IsSet(NetworkInputData.BUTTON_CHANGECAMERA) ||
                data.buttons.IsSet(NetworkInputData.BUTTON_USEHEAL))
            {
                shouldChangeTarget = true;
                FindAlivePlayers(); // 생존자 목록 갱신
                int count = alivePlayerCameras.Count;
                if (count == 0) // 생존자 없음
                {
                    Debug.Log("생존자 없음");
                    return;
                }
                // 방향 계산: Q → -1, E → +1
                int direction = data.buttons.IsSet(NetworkInputData.BUTTON_USEHEAL) ? 1 : -1;

                if (curSpectatorCam == null)
                {
                    // 최초 관전 진입: 현재 타겟이 없으므로 바로 선택
                    currentPlayerIndex = (count + currentPlayerIndex + direction) % count;
                }
                else
                {
                    // spectatorCam이 이미 존재하는 경우: 중복 방지
                    int attempts = 0;
                    do
                    {
                        currentPlayerIndex = (count + currentPlayerIndex + direction) % count;
                        attempts++;
                    }
                    while (alivePlayerCameras[currentPlayerIndex] == curSpectatorCam && attempts < count);
                }

                player.cameraHandler.firstPersonCamera.GetComponent<CinemachineVirtualCamera>().Priority = 0;
                SetSpectatorTarget(alivePlayerCameras[currentPlayerIndex]);
                // 여길 통과했다면 spectatorCam는 null이 아니다

                Debug.Log($"{alivePlayerCameras[currentPlayerIndex]} 관전 중");
            }
        }
        // 타이머 만료되면 리스폰 처리
        if (respawnTimer.Expired(player.Runner))
        {
            respawnTimer = TickTimer.None; // 재호출 방지
            OnRespawn();
            stateMachine.ChangeState(stateMachine.IdleState);
        }
    }
    /// <summary>
    /// FixedUpdateNetwork 다음에 호출된다
    /// 각각의 플레이어 local에서 render
    /// </summary>
    public override void Render()
    {
        if (player.statHandler.IsDead)
        {
            if (!isSpectating)  // 죽었지만 아직 관전모드 아님
            {
                // 관전 타겟: 1인칭 ON, 3인칭 OFF
                // 나머지는 1인칭 off, 3인칭 on
                foreach (var playerRef in playerRefs)
                {
                    Player otherPlayer = NetworkGameManager.Instance.gamePlayers.GetPlayerObj(playerRef);
                    // 자신: 1인칭 ON, 3인칭 OFF
                    if (otherPlayer.HasInputAuthority)
                    {
                        ShowFirstPersonModel(otherPlayer.FirstPersonRoot);
                        HideThirdPersonModel(otherPlayer.ThirdPersonRoot);
                    }
                    // 다른 플레이어: 1인칭 OFF, 3인칭 ON
                    else
                    {
                        HideFirstPersonModel(otherPlayer.FirstPersonRoot);
                        ShowThirdPersonModel(otherPlayer.ThirdPersonRoot);
                    }
                }
            }
            else if (isSpectating) // 관전모드
            {
                if (curSpectatorCam == null) return;

                // 관전 타겟 플레이어 얻기
                Player targetPlayer = curSpectatorCam.GetComponentInParent<Player>();
                if (targetPlayer == null) return;

                // 관전 타겟: 1인칭 ON, 3인칭 OFF
                // 나머지는 1인칭 off, 3인칭 on
                foreach (var playerRef in playerRefs)
                {
                    Player otherPlayer = NetworkGameManager.Instance.gamePlayers.GetPlayerObj(playerRef);
                    // 관전 타겟: 1인칭 ON, 3인칭 OFF
                    if (otherPlayer == targetPlayer)
                    {
                        ShowFirstPersonModel(otherPlayer.FirstPersonRoot);
                        HideThirdPersonModel(otherPlayer.ThirdPersonRoot);
                    }
                    // 관전 타겟이 아닌 모든 플레이어(자신 포함): 1인칭 OFF, 3인칭 ON
                    else
                    {
                        HideFirstPersonModel(otherPlayer.FirstPersonRoot);
                        ShowThirdPersonModel(otherPlayer.ThirdPersonRoot);
                    }
                }
                // 타겟이 바뀌는 것은 별도의 입력 처리에서만 shouldChangeTarget을 갱신
                shouldChangeTarget = false;
            }
        }
        else // 안죽었다
        {
            // 안죽었을때는 관전모드 아니니까 따로 검사하지 않아도 된다
            // 입력 권한이 있는 자신의 렌더러 처리
            if (HasInputAuthority)
            {
                // 자신의 1인칭 렌더러 활성화, 3인칭 렌더러 비활성화
                ShowFirstPersonModel(player.FirstPersonRoot);
                HideThirdPersonModel(player.ThirdPersonRoot);
            }
            else // 입력 권한 없는 대상(다른 플레이어)
            {
                /// 다른 대상의 경우
                foreach (var playerRef in playerRefs)
                {
                    // NetworkObject 가져온 다음 Player 컴포넌트에 접근
                    Player otherPlayer = NetworkGameManager.Instance.gamePlayers.GetPlayerObj(playerRef);
                    if (otherPlayer == player) continue; // 본인 제외

                    // 남의 오브젝트: 1인칭 OFF, 3인칭 ON
                    HideFirstPersonModel(otherPlayer.FirstPersonRoot);
                    ShowThirdPersonModel(otherPlayer.ThirdPersonRoot);
                }

            }
        }
    }
    void HideFirstPersonModel(GameObject firstPersonRoot)
    {
        // 1인칭 렌더러 비활성화
        MeshRenderer[] meshRenderers = firstPersonRoot.GetComponentsInChildren<MeshRenderer>(true);
        foreach (var mr in meshRenderers)
            mr.enabled = false;

        SkinnedMeshRenderer[] skinnedRenderers = firstPersonRoot.GetComponentsInChildren<SkinnedMeshRenderer>(true);
        foreach (var smr in skinnedRenderers)
            smr.enabled = false;

    }
    void ShowFirstPersonModel(GameObject firstPersonRoot)
    {
        // 리스폰 시 다시 활성화 (OnRespawn() 대신 여기서 처리)
        MeshRenderer[] meshRenderers = firstPersonRoot.GetComponentsInChildren<MeshRenderer>(true);

        foreach (var mr in meshRenderers)
            mr.enabled = true;

        SkinnedMeshRenderer[] skinnedRenderers = firstPersonRoot.GetComponentsInChildren<SkinnedMeshRenderer>(true);
        foreach (var smr in skinnedRenderers)
            smr.enabled = true;
    }

    void HideThirdPersonModel(GameObject thirdPersonRoot)
    {
        foreach (var renderer in thirdPersonRoot.GetComponentsInChildren<Renderer>())
            renderer.enabled = false;
    }
    void ShowThirdPersonModel(GameObject thirdPersonRoot)
    {
        foreach (var renderer in thirdPersonRoot.GetComponentsInChildren<Renderer>())
            renderer.enabled = true;
    }


    #region 부활,관전모드
    private void OnDeath()
    {
        if (alivePlayerCameras.Count > 0)
            FindAlivePlayers(); // 리스트 갱신

        // 서버 권한 로직은 여전히 이 아래에서 실행됨
        respawnTimer = TickTimer.CreateFromSeconds(Runner, respawnTime);

        stateMachine.ChangeState(stateMachine.DeadState);
    }

    private void OnRespawn()
    {
        /// 생성된 위치로 들고온다
        int randomIndex = UnityEngine.Random.Range(0, NetworkGameManager.Instance.gamePlayers.PlayerSpawnPoints.Count);
        player.transform.position = NetworkGameManager.Instance.gamePlayers.PlayerSpawnPoints[randomIndex].transform.position;

        //player.statHandler.IsDead = false;

        player.statHandler.SetHealth(200);  // 여기에서 IsDead를 false로 만들어준다
        if (alivePlayerCameras.Count > 0)
            ResetSpectatorTarget();
    }

    public void FindAlivePlayers()
    {
        // 현재 접속중인 플레이어 정보들 저장
        playerRefs = NetworkGameManager.Instance.gamePlayers.GetPlayerRefs();

        // 이전 생존자 리스트 제거
        alivePlayerCameras.Clear();

        // playerRefs에 있는 플레이어들의 virtualCamera들의 위치를 저장
        foreach (var playerRef in playerRefs)
        {
            // NetworkObject 가져온 다음 Player 컴포넌트에 접근
            Player otherPlayer = NetworkGameManager.Instance.gamePlayers.GetPlayerObj(playerRef);
            if (otherPlayer == player) continue; // 본인 제외

            if (otherPlayer != null && otherPlayer.statHandler.CurHealth > 0)
            {
                PlayerCameraHandler otherCamHandler = otherPlayer.GetComponentInChildren<PlayerCameraHandler>();
                otherCamHandler.spectatorCamera.Priority = 0;
                alivePlayerCameras.Add(otherCamHandler.spectatorCamera);
            }
        }
    }
    public void SetSpectatorTarget(CinemachineVirtualCamera targetCam)
    {
        if (targetCam == null) return;
        if (curSpectatorCam != null)
            curSpectatorCam.Priority = 0;    // 지금 관전하고 있는 대상의 우선순위 낮춘다


        /// 타겟을 바꾸는 중에 타겟이 죽는 경우에도 타겟이 alivePlayerCameras에 있으면 이동해야한다
        curSpectatorCam = targetCam;
        curSpectatorCam.Priority = 100;    // 새로운 관전 대상의 우선순위 높인다
        isSpectating = true;
    }
    public void ResetSpectatorTarget()
    {
        // 현재 관전중인 대상 우선순위를 낮춘다
        if (curSpectatorCam != null)
            curSpectatorCam.Priority = 0;

        // 자신의 플레이어인 경우
        if (HasInputAuthority)
            player.cameraHandler.firstPersonCamera.GetComponent<CinemachineVirtualCamera>().Priority = 100;

        // 관전대상 없음
        curSpectatorCam = null;
        isSpectating = false;
    }
    #endregion

    #region 이동, 점프
    // 플레이어의 이동(방향은 CameraHandler에서 설정) 처리. 그 방향이 transform.forward로 이미 설정되었다
    private void HandleMovement(NetworkInputData input)
    {
        if (LockState == PlayerLockState.MoveLock)
        {
            _simpleKCC.Move(
                Vector3.zero
            );
        }
        else
        {
            //Vector3 input = data.direction;
            player.Weapons.OnMoveAnimation(input.direction);

            //// 카메라 기준 방향 가져오기
            Vector3 camForward = player.cameraHandler.GetCameraForwardOnXZ();
            Vector3 camRight = player.cameraHandler.GetCameraRightOnXZ();
            Vector3 moveDir = (camRight * input.direction.x + camForward * input.direction.z).normalized;

            moveDir.y = 0f; // 수직 방향 제거

            Vector3 moveVelocity = moveDir * 10f;
            Vector3 jumpImpulse = Vector3.zero;

            if (input.isJumping == true && _simpleKCC.IsGrounded == true)
            {
                // Set world space jump vector.
                jumpImpulse = Vector3.up * 10.0f;
            }

            _simpleKCC.Move(moveVelocity, jumpImpulse.magnitude);
            //networkCharacterController.Move(
            //    moveDir
            //);

            // Update camera pivot and transfer properties from camera handle to Main Camera.
            // LateUpdate() is called after all Render() calls - the character is already interpolated.
            //// 회전 강제 고정: 카메라가 지정한 forward로
            //player.transform.forward = camForward;
        }
    }

    public void ApplyGravity()
    {
        //if (IsGrounded())
        //{
        //    verticalVelocity = 0f;
        //}
        //else
        //{
        //    verticalVelocity += networkCharacterController.gravity * Time.deltaTime;
        //}

        //networkCharacterController.Jump(false, verticalVelocity);
    }
    /// <summary>
    /// 점프 시작 시 수직 속도 계산
    /// </summary>
    public void StartJump()
    {
        //verticalVelocity = Mathf.Sqrt(player.statHandler.JumpPower * -2f * gravity);

        //verticalVelocity = Mathf.Sqrt(player.statHandler.JumpPower * -2f * player.networkCharacterController.gravity);
        //verticalVelocity = Mathf.Sqrt(networkCharacterController.jumpImpulse * -1f * networkCharacterController.gravity);
        //// 땅에서 떨어졌으므로 Grounded를 false로 강제변경
        ////SetGrounded(false);

        //networkCharacterController.Jump(false, verticalVelocity);
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
    public bool IsGrounded() => _simpleKCC.IsGrounded;
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
