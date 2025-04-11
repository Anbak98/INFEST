using Cinemachine;
using Fusion;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// 플레이어의 기본적인 사항들
/// </summary>
public class Player : NetworkBehaviour
{
    //[SerializeField] private Ball _prefabBall;
    // 버튼 누름이 감지될 때만 타이머를 재설정
    [Networked] private TickTimer delay { get; set; }

    // 
    private NetworkCharacterController _cc;
    private Vector3 _forward = Vector3.forward;

    [Header("Components")]
    //public SimpleKCC KCC;
    //public Weapons Weapons;
    //public Health Health;
    public Animator Animator;
    public HitboxRoot HitboxRoot;



    [Header("Setup")]
    public float MoveSpeed = 6f;
    public float JumpForce = 10f;
    //public AudioSource JumpSound;
    //public AudioClip[] JumpClips;
    public Transform CameraHandle;
    public GameObject FirstPersonRoot;
    public GameObject ThirdPersonRoot;
    //public NetworkObject SprayPrefab;


    //[SerializeField] private PhysxBall _prefabPhysxBall;

    // 네트워크 속성을 정의할 때 Fusion은 제공된 get 및 set 스텁을 사용자 지정 코드로 대체하여 네트워크 상태에 접근
    // 이는 애플리케이션이 이러한 방법을 사용하여 속성 값의 변화를 처리할 수 없으며
    // 별도의 setter 메소드를 만드는 것은 로컬에서만 작동한다는 것을 의미
    [Networked] public bool spawnedProjectile { get; set; }

    private ChangeDetector _changeDetector;

    public Material _material;

    // RPC(원격 프로시저 호출)는 특정 이벤트를 네트워크 클라이언트 간에 공유하기에 이상적입니다.
    // 반면에, [Networked] 속성은 지속적으로 변하는 상태를 공유하는 데 적합한 기본 솔루션입니다.
    //[Networked] public string playerName { get; set; }
    //[Networked] public Color playerColor { get; set; }

    private TMP_Text _messages;


    private void Awake()
    {
        _cc = GetComponent<NetworkCharacterController>();
        _forward = transform.forward;

        /// Player에 붙은 PlayerColor 스크립트의 MeshRenderer에 접근하여 material을 가져온다
        _material = GetComponentInChildren<MeshRenderer>().material;
    }

    private void Start()
    {
    }

    /// <summary>
    /// FixedUpdateNetwork를 PlayerController로 옮기는 건?
    /// </summary>
    public override void FixedUpdateNetwork()
    {
        //Debug.Log("FixedUpdateNetwork 진입");
        /// GetInput: OnInput에 있는 데이터 가져온다
        if (GetInput(out NetworkInputData data))
        {
            data.direction.Normalize();
            _cc.Move(5 * data.direction * Runner.DeltaTime);

            if (data.direction.sqrMagnitude > 0)
                _forward = data.direction;  //  마지막 이동 방향을 저장하고 이를 공의 앞 방향으로 사용

            // 네트워크 객체는 StateAuthority(호스트)만 생성할 수 있기 때문에 StateAuthority에 대한 확인이 필요
            // 호스트에서만 실행되고 클라이언트에서는 예측되지 않는다
            if (HasStateAuthority && delay.ExpiredOrNotRunning(Runner))
            {
                // 마우스 좌클릭(공격)
                if (data.buttons.IsSet(NetworkInputData.BUTTON_FIRE))
                {
                    Debug.Log("공격");

                    //delay = TickTimer.CreateFromSeconds(Runner, 0.5f);
                    //Runner.Spawn(_prefabBall,
                    //transform.position + _forward,
                    //Quaternion.LookRotation(_forward),
                    //  Object.InputAuthority, (runner, o) =>
                    //  {
                    //      // Initialize the Ball before synchronizing it
                    //      o.GetComponent<Ball>().Init();
                    //  });
                    //spawnedProjectile = !spawnedProjectile;
                }
                // PhyscBall.Init() 메소드를 사용하여 스폰을 호출하고 속도(마지막 순방향에 곱한 상수)를 설정
                
                // 마우스 우클릭(ZOOM)
                else if (data.buttons.IsSet(NetworkInputData.BUTTON_ZOOM))
                {
                    Debug.Log("조준");

                    //delay = TickTimer.CreateFromSeconds(Runner, 0.5f);
                    //Runner.Spawn(_prefabPhysxBall,
                    //  transform.position + _forward,
                    //  Quaternion.LookRotation(_forward),
                    //  Object.InputAuthority,
                    //  (runner, o) =>
                    //  {
                    //      o.GetComponent<PhysxBall>().Init(10 * _forward);
                    //  });
                    //spawnedProjectile = !spawnedProjectile;
                }
                // Runner.Spawn()을 호출한 후 spawnedProjectile 속성을 토글 하여 콜백을 트리거
                //spawnedProjectile = !spawnedProjectile;
            }
        }

        // 1인칭, 3인칭에 따라 다르게 보여야한다


    }

    private void Update()
    {
        // Object.HasInputAuthority를 확인한다: 모든 클라이언트에서 실행되지만 이 플레이어를 제어하는 클라이언트만 RPC를 호출해야 하기 때문
        if (Object.HasInputAuthority && Input.GetKeyDown(KeyCode.R))
        {
            RPC_SendMessage("Hey Mate!");
        }
    }
    public override void Spawned()
    {
        // 튜토리얼에 있던 부분, 삭제하니까 오류가 많이 나서 일단 남겨두었다
        _changeDetector = GetChangeDetector(ChangeDetector.Source.SimulationState);

        name = $"{Object.InputAuthority} ({(HasInputAuthority ? "Input Authority" : (HasStateAuthority ? "State Authority" : "Proxy"))})";

        // Enable first person visual for local player, third person visual for proxies.
        SetFirstPersonVisuals(HasInputAuthority);

        if (HasInputAuthority == false)
        {
            // Virtual cameras are enabled only for local player.
            var virtualCameras = GetComponentsInChildren<CinemachineVirtualCamera>(true);
            for (int i = 0; i < virtualCameras.Length; i++)
            {
                virtualCameras[i].enabled = false;
            }
        }
    }
    private void SetFirstPersonVisuals(bool firstPerson)
    {
        FirstPersonRoot.SetActive(firstPerson);
        ThirdPersonRoot.SetActive(firstPerson == false);
    }



    /// <summary>
    /// 마지막으로 ChangeDetector를 호출한 이후 네트워크화된 속성에 발생한 모든 변경 사항을 반복
    /// Render()는 Unity의 Update()처럼 Fusion이 매 프레임마다 렌더링 루프에서 자동으로 호출하는 함수다
    /// </summary>
    public override void Render()
    {
        foreach (var change in _changeDetector.DetectChanges(this))
        {
            switch (change)
            {
                case nameof(spawnedProjectile):
                    /// PlayerColor 스크립트의 MeshRenderer에 접근하여 material의 색깔 변경
                    _material.color = Color.white;
                    break;
            }
        }
        /// PlayerColor 스크립트의 MeshRenderer에 접근하여 material의 색깔 변경
        _material.color = Color.Lerp(_material.color, Color.blue, Time.deltaTime);
    }

    /// <summary>
    /// RPC 관련 메서드들
    /// </summary>
    /// <param name="name"></param>
    /// <param name="color"></param>
    [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority, HostMode = RpcHostMode.SourceIsHostPlayer)]
    public void RPC_SendMessage(string message, RpcInfo info = default)
    {
        RPC_RelayMessage(message, info.Source);
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All, HostMode = RpcHostMode.SourceIsServer)]
    public void RPC_RelayMessage(string message, PlayerRef messageSource)
    {
        if (_messages == null)
            _messages = FindObjectOfType<TMP_Text>();

        if (messageSource == Runner.LocalPlayer)
        {
            message = $"You said: {message}\n";
        }
        else
        {
            message = $"Some other player said: {message}\n";
        }

        _messages.text += message;
    }
}