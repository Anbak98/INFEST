using UnityEngine;
using Fusion;
using Cinemachine;
using System.Collections.Generic;
using INFEST.Game;
using Fusion.Addons.SimpleKCC;
using UnityEngine.Windows;
using Cinemachine.Utility;
using UnityEngine.UIElements;

/// <summary>
/// 캐릭터 동작 처리를 한다
/// 
/// InputAction의 이벤트메서드를 연결한다
/// 플레이어의 FSM은 네트워크에서 동기화된 입력 데이터를 기반으로 상태 전환
/// 
/// 플레이어의 동작 및 상태 관리
/// FixedUpdateNetwork()에서 Fusion으로부터 받은 입력 데이터를 기반으로 시뮬레이션 수행.
/// </summary>

[System.Flags]
public enum PlayerLockState
{
    Free = 0,   // idle
    MoveLock = 1 << 0,  // 0001
    RunLock = 1 << 1,   // 0010, 앉기, 앉아서 걷기, 조준 상태
    JumpLock = 1 << 2,   // 0100, 조준 상태
    SitLock = 1 << 3,   // 1000, 달리기 상태
    ZoomLock = 1 << 4,   // 10000, 달리기, 점프 상태
    FireLock = 1 << 5,    // 100000, 달리기
}

public class PlayerController : NetworkBehaviour
{
    NetworkInputData DEBUG_DATA;

    public PlayerLockState LockState = PlayerLockState.Free;

    public float curMoveSpeed; // 현재 이동 속도
    public float walkSpeed;   // 걷기 속도
    public float runSpeed;    // 달리기 속도
    public float waddleSpeed;  // 앉아서 걷기 속도
    public float acceleration = 10f;

    public float targetSpeed;
    public float prevSpeed;

    public WeaponSpawner weaponSpawner; // firstPersonAnimator를 가지고 있다 

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
    [Networked]
    private Vector3 _moveVelocity { get; set; }

    private float _standHeight;
    private float _sitHeight;

    public void Init()
    {
        stateMachine = new PlayerStateMachine(player, this);

        player.statHandler.OnHealthChanged += (amount) => player.attackedEffectController.CalledWhenPlayerAttacked(amount);
        player.statHandler.OnDeath += OnDeath;
        player.statHandler.OnRespawn += OnRespawn;

        // Set custom gravity.
        _simpleKCC.SetGravity(Physics.gravity.magnitude * -4.0f);
        targetSpeed = walkSpeed;
        _standHeight = cameraHandler.transform.position.y - player.transform.position.y;
        _sitHeight = _standHeight / 2; // 원하는 비율로 조정 가능
    }

    public override void FixedUpdateNetwork()
    {
        if (GetInput(out NetworkInputData data))
        {
            //Debug.Log(gameObject.name);   // 모든 플레이어에 PlayerController 스크립트가 붙어있어서 번갈아 호출되는것처럼 보이는 것
            DEBUG_DATA = data;
            if (!player.statHandler.IsDead)   // 죽지 않았을때만 가능한 동작
            {
                // 1. 상태 전환 먼저 수행
                if (stateMachine.TryGetNextState(data, out var nextState))
                {
                    stateMachine.ChangeState(nextState);
                }
                // 2. 현재 상태의 로직 수행
                stateMachine.OnUpdate(data);
                HandleMovement(data);
                StartFire(data);
                StartReload(data);

                if (stateMachine.currentState != stateMachine.RunState)
                {
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
                    if (data.buttons.IsSet(NetworkInputData.BUTTON_ZOOM) && (LockState & PlayerLockState.ZoomLock) == 0)
                    {
                        player.Weapons.Aiming(true);
                    }
                    if (data.buttons.IsSet(NetworkInputData.BUTTON_ZOOMPRESSED) && (LockState & PlayerLockState.ZoomLock) == 0)
                    {
                        player.Weapons.Aiming(false);
                    }
                    if (data.buttons.IsSet(NetworkInputData.BUTTON_USEGRENAD))
                    {
                        player.Weapons.RPC_OnThrowReady();
                    }
                    if (data.buttons.IsSet(NetworkInputData.BUTTON_GRENADPRESSED))
                    {
                        player.Weapons.RPC_OnThrowGrenade();
                    }
                    if (data.buttons.IsSet(NetworkInputData.BUTTON_USESHIELD))
                    {
                        player.Consumes.Mounting();
                    }
                }
                if (data.buttons.IsSet(NetworkInputData.BUTTON_USEHEAL) && player.statHandler.CurHealth != player.statHandler.info.data.Health)
                {
                    player.Consumes.Heal();
                }
            }
        }

        // 타이머 만료되면 리스폰 처리
        if (respawnTimer.Expired(player.Runner))
        {
            respawnTimer = TickTimer.None; // 재호출 방지

            //OnRespawn();
            player.statHandler.SetHealth(200);  // 여기에서 IsDead를 false로 만들어준다

            stateMachine.ChangeState(stateMachine.IdleState);
        }
        // 죽은 경우에 시점 전환(로컬 처리), 네트워크 공유하지 않는다 -> 따로 입력처리
        if (player.statHandler.IsDead)
            return;
    }
    /// <summary>
    /// FixedUpdateNetwork 다음에 호출된다
    /// 각각의 플레이어 local에서 render
    /// Render에서는 오브젝트 1개의 처리만 담당한다(자신이든, 타인이든)
    /// </summary>
    public override void Render()
    {
        // Q, E 입력은 로컬 처리
        LocalInputForChangingam();

        /// 1. isFocusing 중인 플레이어(자신 포함)
        if (cameraHandler.isFocusing)
        {
            ShowFirstPersonModel(player.FirstPersonRoot);
            HideThirdPersonModel(player.ThirdPersonRoot);
        }
        /// 2. 그 외 나머지 플레이어
        else if (!cameraHandler.isFocusing)
        {
            HideFirstPersonModel(player.FirstPersonRoot);
            ShowThirdPersonModel(player.ThirdPersonRoot);
        }
    }
    void HideFirstPersonModel(GameObject firstPersonRoot)
    {
        //firstPersonRoot.SetActive(false);

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
        //firstPersonRoot.SetActive(true);

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
        //thirdPersonRoot.SetActive(false);

        foreach (var renderer in thirdPersonRoot.GetComponentsInChildren<Renderer>())
            renderer.enabled = false;
    }
    void ShowThirdPersonModel(GameObject thirdPersonRoot)
    {
        //thirdPersonRoot.SetActive(true);

        foreach (var renderer in thirdPersonRoot.GetComponentsInChildren<Renderer>())
            renderer.enabled = true;
    }

    // PlayerAnimationController의 Animator를 바꿔야한다




    #region 부활,관전모드
    private void OnDeath()
    {
        if (player.cameraHandler.alivePlayerCameras.Count > 0)
            player.cameraHandler.FindAlivePlayers(); // 리스트 갱신

        weaponSpawner.SetWeaponAnimaDieParam(true);
        // 서버 권한 로직은 여전히 이 아래에서 실행됨
        respawnTimer = TickTimer.CreateFromSeconds(Runner, respawnTime);

        player.cameraHandler.SwitchToNextFocusingCam(1);
        stateMachine.ChangeState(stateMachine.DeadState);
    }

    private void OnRespawn()
    {
        if(NetworkGameManager.Instance.GameState != GameState.End)
        {
            /// 생성된 위치로 들고온다
            int randomIndex = UnityEngine.Random.Range(0, NetworkGameManager.Instance.gamePlayers.PlayerSpawnPoints.Count);
            player.transform.position = NetworkGameManager.Instance.gamePlayers.PlayerSpawnPoints[randomIndex].transform.position;

            player.statHandler.IsDead = false;
            //player.statHandler.SetHealth(200);  // 여기에서 IsDead를 false로 만들어준다
            //if (alivePlayerCameras.Count > 0)
            player.cameraHandler.ResetSpectatorTarget();
            player.targetableFromMonster.CurHealth = player.statHandler.CurHealth;
            weaponSpawner.SetWeaponAnimaDieParam(false);

            //isFocusing = false;
        }
    }

    /// 관전 모드에서 시점 전환 처리 (로컬 전용)
    /// Q/E 키로 생존 플레이어들 간 카메라 전환
    private void LocalInputForChangingam()
    {
        /// InputAuthority가 있는 플레이어가 죽었을 때
        if (!player.statHandler.IsDead || !HasInputAuthority)
            return;

        // 현재 프레임에서 Q 또는 E 키 입력 확인
        bool qPressed = UnityEngine.Input.GetKeyDown(KeyCode.Q);
        bool ePressed = UnityEngine.Input.GetKeyDown(KeyCode.E);

        if (!qPressed && !ePressed)
            return;

        int direction = ePressed ? 1 : -1;
        player.cameraHandler.SwitchToNextFocusingCam(direction);
        //isFocusing = true;
    }
    #endregion

    #region 이동, 점프, 앉기, 달리기, 사격
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


            bool CanRun = (LockState & PlayerLockState.RunLock) == 0;
            bool CanJump = (LockState & PlayerLockState.JumpLock) == 0;
            bool CanSit = (LockState & PlayerLockState.SitLock) == 0;
            bool CanZoom = (LockState & PlayerLockState.ZoomLock) == 0;

            // 상태에 따라 목표 속도 결정
            if (!CanZoom)
            {
                if (!CanSit) // 달리기
                {
                    targetSpeed = runSpeed;
                    prevSpeed = runSpeed;
                }
                else // 점프
                {
                    targetSpeed = prevSpeed;    /// 기존의 스피드로 움직인다
                }
            }
            else if (!CanRun)
            {
                if (!CanJump) // 조준
                {
                    targetSpeed = prevSpeed;  /// 기존의 스피드로 움직인다
                }
                else // 앉기, 앉아서 이동
                {
                    targetSpeed = waddleSpeed;
                    prevSpeed = waddleSpeed;
                }
            }
            else // idle
            {
                targetSpeed = walkSpeed;    // 기본 걷기
                prevSpeed = walkSpeed;
            }

            // 부드러운 속도 전환
            _moveVelocity = Vector3.Lerp(_moveVelocity, moveDir * targetSpeed, acceleration * Runner.DeltaTime);

            Vector3 jumpImpulse = Vector3.zero;

            if (CanJump && input.isJumping == true && _simpleKCC.IsGrounded == true)
            {
                // Set world space jump vector.
                jumpImpulse = Vector3.up * 10.0f;
            }

            _simpleKCC.Move(_moveVelocity, jumpImpulse.magnitude);
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
        cameraHandler.transform.position = new Vector3(cameraHandler.transform.position.x, _sitHeight, cameraHandler.transform.position.z);
    }

    // 일어난다
    public void StartStand()
    {
        // collider는 상태에서 변화시키므로 여기서는 transform만 아래로
        cameraHandler.transform.position = new Vector3(cameraHandler.transform.position.x, _standHeight, cameraHandler.transform.position.z);
    }

    public void StartFire(NetworkInputData data)
    {
        // 네트워크 객체는 StateAuthority(호스트)만 생성할 수 있기 때문에 StateAuthority에 대한 확인이 필요
        // 호스트에서만 실행되고 클라이언트에서는 예측되지 않는다0
        if (HasStateAuthority && delay.ExpiredOrNotRunning(Runner) && !player.isInteraction)
        {
            // 마우스 좌클릭(공격)
            if ((LockState & PlayerLockState.FireLock) == 0 && data.buttons.IsSet(NetworkInputData.BUTTON_FIRE))
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
    //private void OnGUI()
    //{
    //    if (HasInputAuthority)
    //    {
    //        GUILayout.Label(stateMachine.currentState.ToString());
    //        GUILayout.Label(DEBUG_DATA.ToString());
    //        //
    //        GUILayout.Label("Player HP: " + player.statHandler.CurHealth.ToString());
    //        GUILayout.Label("PlayerController position: " + transform.position.ToString());
    //        GUILayout.Label("PlayerController rotation: " + transform.rotation.ToString());
    //        GUILayout.Label("CameraHandler position: " + cameraHandler.transform.position.ToString());
    //        GUILayout.Label("CameraHandler rotation: " + cameraHandler.transform.rotation.ToString());
    //        GUILayout.Label("Current Speed: " + curMoveSpeed.ToString());
    //        GUILayout.Label("Velocity Speed: " + _simpleKCC.RealVelocity.magnitude);

    //        ////
    //        //GUILayout.Label("Grounded: " + networkCharacterController.Grounded.ToString());
    //        //GUILayout.Label("Equip: " + stateMachine.Player.GetWeapons()?.CurrentWeapon);

    //    }
    //}
}
