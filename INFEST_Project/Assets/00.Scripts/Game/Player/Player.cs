using Cinemachine;
using Fusion;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SocialPlatforms;


/// <summary>
/// 플레이어의 기본적인 사항들
/// </summary>
public class Player : NetworkBehaviour
{
    public static Player local { get; private set; }
    [Networked] private TickTimer delay { get; set; }

    [SerializeField] public PlayerAnimationController animationController;
    [field: SerializeField] public PlayerData data; // 플레이어의 데이터
                                                    // 초기화할때 json파일을 읽고 대입한다

    // playerController는 Input값 관리
    public PlayerController playerController;     // 1인칭: LocalPlayerController, 3인칭: RemotePlayerController
    public PlayerStatHandler statHandler;

    public CharacterController characterController; // collider, rigidbody 등 내장되어있다
    public NetworkCharacterController networkCharacterController;
    public PlayerAttackedEffectController attackedEffectController;

    public PlayerStateMachine stateMachine;
    public PlayerCameraHandler cameraHandler;

    public bool inStoreZoon = false;
    public bool isInteraction = false;
    public Store store;
    public Inventory inventory;
    public CharacterInfoInstance characterInfoInstance;

    // 관전모드
    public GameObject FirstPersonCamera;    // CinemachineVirtualCamera를 가지고 있는 오브젝트

    #region 기존의 데이터
    //private NetworkCharacterController _cc;
    private Vector3 _forward = Vector3.forward;
    public WeaponSpawner Weapons;// SY
    public ConsumeSpawner Consumes;// SY
    [Header("Setup")]
    public float MoveSpeed = 6f;
    public float JumpForce = 10f;
    //public AudioSource JumpSound;
    //public AudioClip[] JumpClips;
    public Transform CameraHandle;
    public GameObject FirstPersonRoot;
    public GameObject ThirdPersonRoot;
    //public NetworkObject SprayPrefab;


    // 네트워크 속성을 정의할 때 Fusion은 제공된 get 및 set 스텁을 사용자 지정 코드로 대체하여 네트워크 상태에 접근
    // 이는 애플리케이션이 이러한 방법을 사용하여 속성 값의 변화를 처리할 수 없으며
    // 별도의 setter 메소드를 만드는 것은 로컬에서만 작동한다는 것을 의미
    [Networked] public bool spawnedProjectile { get; set; }

    private ChangeDetector _changeDetector;

    public Material _material;


    private TMP_Text _messages;
    #endregion

    private void Awake()
    {
        /// 기존의 데이터
        //_cc = GetComponent<NetworkCharacterController>();
        _forward = transform.forward;
        /// Player에 붙은 PlayerColor 스크립트의 MeshRenderer에 접근하여 material을 가져온다
        _material = GetComponentInChildren<MeshRenderer>().material;
        inventory = GetComponent<Inventory>();
        statHandler.OnHealthChanged += attackedEffectController.CalledWhenPlayerAttacked;
        statHandler.OnDeath += OnDeath;
        statHandler.OnRespawn += OnRespawn;
    }

    private void OnDeath()
    {
        FirstPersonRoot.SetActive(false);
        ThirdPersonRoot.SetActive(true);
        stateMachine.ChangeState(stateMachine.DeadState);
    }
    private void OnRespawn()
    {
        FirstPersonRoot.SetActive(true);
        ThirdPersonRoot.SetActive(false);
        stateMachine.ChangeState(stateMachine.IdleState);
    }


    private void Start()
    {
        stateMachine = new PlayerStateMachine(this, playerController);
    }

    NetworkInputData DEBUG_DATA;
    public override void FixedUpdateNetwork()
    {
        if (GetInput(out NetworkInputData data))
        {
            DEBUG_DATA = data;
            playerController.stateMachine.OnUpdate(data);
            //cameraHandler.RoateCamera(data);

            if (data.buttons.IsSet(NetworkInputData.BUTTON_INTERACT) && inStoreZoon)
            {

                if (!isInteraction) store.RPC_RequestInteraction(this, Object.InputAuthority);

                else store.RPC_RequestStopInteraction(Object.InputAuthority);

                isInteraction = !isInteraction;

            }

            if (data.scrollValue.y != 0)
            {
                Debug.Log("스왑");
                Weapons.Swap(data.scrollValue.y);
            }

            if(data.buttons.IsSet(NetworkInputData.BUTTON_ZOOM))
            {
                Weapons.Aiming(true);
            }
            if (data.buttons.IsSet(NetworkInputData.BUTTON_ZOOMPRESSED))
            {
                Weapons.Aiming(false);

                }

                if(data.buttons.IsSet(NetworkInputData.BUTTON_USEGRENAD))
                {
                    Debug.Log("G키 호출됨");
                    Consumes.Throw();
                }

                if (data.buttons.IsSet(NetworkInputData.BUTTON_USEHEAL))
                {

                }

                if (data.buttons.IsSet(NetworkInputData.BUTTON_USESHIELD))
                {

                }
        }
    }
    

    private void OnGUI()
    {
        if (HasInputAuthority)
        {
            GUILayout.Label(playerController.stateMachine.currentState.ToString());
            GUILayout.Label(DEBUG_DATA.ToString());
            //
            GUILayout.Label("Player HP: " + statHandler.CurrentHealth.ToString());
            GUILayout.Label("PlayerController position: " + playerController.transform.position.ToString());
            GUILayout.Label("PlayerController rotation: " + playerController.transform.rotation.ToString());
            GUILayout.Label("CameraHandler position: " + cameraHandler.transform.position.ToString());
            GUILayout.Label("CameraHandler rotation: " + cameraHandler.transform.rotation.ToString());
            //
            GUILayout.Label("Grounded: " + networkCharacterController.Grounded.ToString());
            //GUILayout.Label("Equip: " + stateMachine.Player.GetWeapons()?.CurrentWeapon);
        }
    }

    public WeaponSpawner GetWeapons()
    {
        return Weapons;
    }

    public override void Spawned()
    {
        // 튜토리얼에 있던 부분, 삭제하니까 오류가 많이 나서 일단 남겨두었다
        _changeDetector = GetChangeDetector(ChangeDetector.Source.SimulationState);

        name = $"{Object.InputAuthority} ({(HasInputAuthority ? "Input Authority" : (HasStateAuthority ? "State Authority" : "Proxy"))})";

        // Enable first person visual for local player, third person visual for proxies.
        SetFirstPersonVisuals(HasInputAuthority);

        // CinemachineVirtualCamera가 포함된 게임오브젝트를 비활성화한 상태로 시작하므로 
        if (HasInputAuthority == false)
        {
            // 다른 플레이어의 CinemachineVirtualCamera는 우선순위 낮춘다
            FirstPersonCamera.GetComponent<CinemachineVirtualCamera>().Priority = 0; 

            //// Virtual cameras are enabled only for local player.
            //var virtualCameras = GetComponentsInChildren<CinemachineVirtualCamera>(true);
            //// 관전모드를 위해서 컴포넌트는 비활성화하면 안된다.게임오브젝트를 비활성화하는 방식으로 수정
            //for (int i = 0; i < virtualCameras.Length; i++)
            //{
            //    virtualCameras[i].enabled = false;
            //}
        }

        if (Object.HasInputAuthority) // 로컬 플레이어 본인일 때
        {
            // FirstPersonCamera
            //FirstPersonCamera.SetActive(true);

            FirstPersonCamera.GetComponent<CinemachineVirtualCamera>().Priority = 100;  // 우선순위를 높이면

            local = this;
            Debug.Log("Local Player 설정 완료");
        }

        if (characterInfoInstance == null) // 캐릭터인스턴스, 인벤토리 설정
        {
            characterInfoInstance = new(1);

            for (int i = 0; i < Weapons.Weapons.Count; i++)
            {
                if (Weapons.Weapons[i].key == characterInfoInstance.data.StartAuxiliaryWeapon)
                {
                    inventory.auxiliaryWeapon[0] = Weapons.Weapons[i];
                    inventory.auxiliaryWeapon[0].IsCollected = true;
                }

                if (Weapons.Weapons[i].key == characterInfoInstance.data.StartWeapon1)
                {
                    inventory.weapon[0] = Weapons.Weapons[i];
                    inventory.weapon[0].IsCollected = true;
                }

                if (inventory.auxiliaryWeapon[0] != null && inventory.weapon[0] != null)
                    break;
            }
            //inventory.auxiliaryWeapon[0] = Weapons.weapons[i] (characterInfoInstance.data.StartAuxiliaryWeapon);
            //inventory.weapon[0] =  new WeaponInstance(characterInfoInstance.data.StartWeapon1);
            //#region 체크용 bool 값
            //int itemChk = characterInfoInstance.data.StartConsumeItem1 % 10000;
            //bool throwingWeapon = itemChk < 800 && itemChk > 700;
            //bool recoveryItem = itemChk < 900 && itemChk > 800;
            //bool shieldItme = itemChk < 1000 && itemChk > 900;
            //#endregion


            for (int i = 0; i < Consumes.Consumes.Count; i++)
            {
                if(Consumes.Consumes[i].key == characterInfoInstance.data.StartConsumeItem1)
                {
                    #region 체크용 bool 값

                    int itemChk = characterInfoInstance.data.StartConsumeItem1 % 10000;
                    bool throwingWeapon = itemChk < 800 && itemChk > 700;
                    bool recoveryItem = itemChk < 900 && itemChk > 800;
                    bool shieldItme = itemChk < 1000 && itemChk > 900;
                    #endregion
                    if (throwingWeapon)
                        inventory.consume[0] = Consumes.Consumes[i];

                    if (recoveryItem)
                        inventory.consume[1] = Consumes.Consumes[i];

                    if (shieldItme)
                        inventory.consume[2] = Consumes.Consumes[i];
                    break;

                }

            }
            inventory.equippedWeapon = inventory.auxiliaryWeapon[0];
        }

        /// 디버그용
        /// 인스펙터 창에서 값 조절
        //statHandler.Init(200, 3, 2, 5, 8, 50, 60);
        Debug.LogFormat($"플레이어 hp = {statHandler.CurrentHealth}");
        Debug.LogFormat($"플레이어 speed = {statHandler.MoveSpeed}");
        Debug.LogFormat($"플레이어 speedModifier = {statHandler.MoveSpeedModifier}");
        Debug.LogFormat($"플레이어 rotationDamping = {statHandler.RotationDamping}");
        Debug.LogFormat($"플레이어 jumpPower = {statHandler.JumpPower}");
        Debug.LogFormat($"플레이어 attackPower = {statHandler.AttackPower}");
        Debug.LogFormat($"플레이어 depencePower = {statHandler.DefensePower}");
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
}