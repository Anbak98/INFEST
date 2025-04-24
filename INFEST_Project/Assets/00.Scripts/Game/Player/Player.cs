using Cinemachine;
using Fusion;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SocialPlatforms;


/// <summary>
/// �÷��̾��� �⺻���� ���׵�
/// </summary>
public class Player : NetworkBehaviour
{
    public static Player local { get; private set; }
    [Networked] private TickTimer delay { get; set; }

    [SerializeField] public PlayerAnimationController animationController;
    [field: SerializeField] public PlayerData data; // �÷��̾��� ������
                                                    // �ʱ�ȭ�Ҷ� json������ �а� �����Ѵ�
                                                    // 

    // ���� �Ǿ��� ����Ǵ� animator�� 1���̹Ƿ� ���߿� 1���� ���δ�
    //public Animator firstPersonAnimator;   // Spawn�Ҷ� �����Ѵ�
    //public Animator thirdPersonAnimator;   // �ٸ� �÷��̾��� animator�� �̰��� ����Ǿ�� �Ѵ�

    // playerController�� Input�� ����
    public PlayerController playerController;     // 1��Ī: LocalPlayerController, 3��Ī: RemotePlayerController
    public PlayerStatHandler statHandler;


    public CharacterController characterController; // collider, rigidbody �� ����Ǿ��ִ�
    public NetworkCharacterController networkCharacterController;


    public PlayerStateMachine stateMachine;
    public PlayerCameraHandler cameraHandler;

    public bool inStoreZoon = false;
    public bool isInteraction = false;
    public Store store;
    public Inventory inventory = new();
    public int gold = 5000;
    #region ������ ������
    //private NetworkCharacterController _cc;
    private Vector3 _forward = Vector3.forward;
    public Weapons Weapons;// SY

    [Header("Components")]
    //public SimpleKCC KCC;
    //public Weapons Weapons;
    //public Health Health;
    //public HitboxRoot HitboxRoot;

    // ��Ʈ��ũ�� transform position ����(�����δ� �� �ʿ� ����. ����Ƽ���� �����ϴ� transform.position ���Ŵϱ�)
    //[Networked] public Vector3 playerTransform { get; set; }

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

    // ��Ʈ��ũ �Ӽ��� ������ �� Fusion�� ������ get �� set ������ ����� ���� �ڵ�� ��ü�Ͽ� ��Ʈ��ũ ���¿� ����
    // �̴� ���ø����̼��� �̷��� ����� ����Ͽ� �Ӽ� ���� ��ȭ�� ó���� �� ������
    // ������ setter �޼ҵ带 ����� ���� ���ÿ����� �۵��Ѵٴ� ���� �ǹ�
    [Networked] public bool spawnedProjectile { get; set; }

    private ChangeDetector _changeDetector;

    public Material _material;

    // RPC(���� ���ν��� ȣ��)�� Ư�� �̺�Ʈ�� ��Ʈ��ũ Ŭ���̾�Ʈ ���� �����ϱ⿡ �̻����Դϴ�.
    // �ݸ鿡, [Networked] �Ӽ��� ���������� ���ϴ� ���¸� �����ϴ� �� ������ �⺻ �ַ���Դϴ�.
    //[Networked] public string playerName { get; set; }
    //[Networked] public Color playerColor { get; set; }

    private TMP_Text _messages;
    #endregion

    private void Awake()
    {
        /// ������ ������
        //_cc = GetComponent<NetworkCharacterController>();
        _forward = transform.forward;
        /// Player�� ���� PlayerColor ��ũ��Ʈ�� MeshRenderer�� �����Ͽ� material�� �����´�
        _material = GetComponentInChildren<MeshRenderer>().material;
    }
    private void Start()
    {
        stateMachine = new PlayerStateMachine(this, playerController);
        //stateMachine.ChangeState(stateMachine.IdleState);
        //Cursor.lockState = CursorLockMode.Locked;
    }
    //public void Update()
    //{
    //    playerController.Update();
    //}
    NetworkInputData DEBUG_DATA;
    public override void FixedUpdateNetwork()
    {
        if (GetInput(out NetworkInputData data))
        {
            DEBUG_DATA = data;
            playerController.stateMachine.OnUpdate(data);

            if (data.buttons.IsSet(NetworkInputData.BUTTON_INTERACT) && inStoreZoon)
            {
                if (!isInteraction) store.RPC_RequestInteraction(this, Object.InputAuthority);

                else store.RPC_RequestStopInteraction(Object.InputAuthority);

                isInteraction = !isInteraction;
            }
        }
    }

    private void OnGUI()
    {
        GUILayout.Label(playerController.stateMachine.currentState.ToString());
        GUILayout.Label(DEBUG_DATA.ToString());
        GUILayout.Label("Grounded: " + networkCharacterController.Grounded.ToString());
        GUILayout.Label("Equip: " + stateMachine.Player.GetWeapons()?.CurrentWeapon);
    }


    /// <summary>
    /// FixedUpdateNetwork�� PlayerController�� �ű�� ��?
    /// </summary>
    //public override void FixedUpdateNetwork()
    //{
    //    /// ������ ������
    //    //Debug.Log("FixedUpdateNetwork ����");
    //    /// GetInput: OnInput�� �ִ� ������ �����´�
    //    if (GetInput(out NetworkInputData data))
    //    {
    //        data.direction.Normalize();
    //        //_cc.Move(5 * data.direction * Runner.DeltaTime);

    //        if (data.direction.sqrMagnitude > 0)
    //            _forward = data.direction;  //  ������ �̵� ������ �����ϰ� �̸� ���� �� �������� ���

    //        // ��Ʈ��ũ ��ü�� StateAuthority(ȣ��Ʈ)�� ������ �� �ֱ� ������ StateAuthority�� ���� Ȯ���� �ʿ�
    //        // ȣ��Ʈ������ ����ǰ� Ŭ���̾�Ʈ������ �������� �ʴ´�
    //        if (HasStateAuthority && delay.ExpiredOrNotRunning(Runner))
    //        {
    //            // ���콺 ��Ŭ��(����)
    //            if (data.buttons.IsSet(NetworkInputData.BUTTON_FIRE))
    //            {
    //                //Debug.Log("����");
    //                _weapons.Fire(data.buttons.IsSet(NetworkInputData.BUTTON_FIREPRESSED));

    //                delay = TickTimer.CreateFromSeconds(Runner, 0.5f);
    //                Runner.Spawn(_prefabBall,
    //                transform.position + _forward,
    //                Quaternion.LookRotation(_forward),
    //                  Object.InputAuthority, (runner, o) =>
    //                  {
    //                      // Initialize the Ball before synchronizing it
    //                      o.GetComponent<Ball>().Init();
    //                  });
    //                spawnedProjectile = !spawnedProjectile;
    //            }
    // PhyscBall.Init() �޼ҵ带 ����Ͽ� ������ ȣ���ϰ� �ӵ�(������ �����⿡ ���� ���)�� ����

    //            // ���콺 ��Ŭ��(ZOOM)
    //            if (data.buttons.IsSet(NetworkInputData.BUTTON_ZOOM))
    //            {
    //                //Debug.Log("����");

    //                _weapons.Aiming();
    //                //delay = TickTimer.CreateFromSeconds(Runner, 0.5f);
    //                //Runner.Spawn(_prefabPhysxBall,
    //                //  transform.position + _forward,
    //                //  Quaternion.LookRotation(_forward),
    //                //  Object.InputAuthority,
    //                //  (runner, o) =>
    //                //  {
    //                //      o.GetComponent<PhysxBall>().Init(10 * _forward);
    //                //  });
    //                //spawnedProjectile = !spawnedProjectile;
    //            }
    //            // Runner.Spawn()�� ȣ���� �� spawnedProjectile �Ӽ��� ��� �Ͽ� �ݹ��� Ʈ����
    //            //spawnedProjectile = !spawnedProjectile;

    //            if (data.buttons.IsSet(NetworkInputData.BUTTON_ZOOMPRESSED))
    //            {
    //                Debug.Log("���� ����");

    //                _weapons.StopAiming();
    //            }

    //            if (data.buttons.IsSet(NetworkInputData.BUTTON_RELOAD))
    //            {
    //                //Debug.Log("����");

    //                _weapons.Reload();
    //            }
    //            if (data.scrollValue.y != 0)
    //            {
    //                Debug.Log("����");
    //                _weapons.Swap(data.scrollValue.y);
    //            }
    //        }
    //    }
    //}

    //private void Update()
    //{
    //    // Object.HasInputAuthority�� Ȯ���Ѵ�: ��� Ŭ���̾�Ʈ���� ��������� �� �÷��̾ �����ϴ� Ŭ���̾�Ʈ�� RPC�� ȣ���ؾ� �ϱ� ����
    //    if (Object.HasInputAuthority && Input.GetKeyDown(KeyCode.R))
    //    {
    //        RPC_SendMessage("Hey Mate!");
    //    }
    //}


    //private void Update()
    //{
    //    // Object.HasInputAuthority�� Ȯ���Ѵ�: ��� Ŭ���̾�Ʈ���� ��������� �� �÷��̾ �����ϴ� Ŭ���̾�Ʈ�� RPC�� ȣ���ؾ� �ϱ� ����
    //    if (Object.HasInputAuthority && Input.GetKeyDown(KeyCode.R))
    //    {
    //        RPC_SendMessage("Hey Mate!");
    //    }
    //}

    public Weapons GetWeapons()
    {
        return Weapons;
    }



    public override void Spawned()
    {
        // Ʃ�丮�� �ִ� �κ�, �����ϴϱ� ������ ���� ���� �ϴ� ���ܵξ���
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

        if (Object.HasInputAuthority) // ���� �÷��̾� ������ ��
        {
            local = this;
            Debug.Log("Local Player ���� �Ϸ�");
        }
        /// ����׿�
        statHandler.Init(200, 3, 2, 5, 8, 50, 60);
        Debug.LogFormat($"�÷��̾� hp = {statHandler.CurrentHealth}");
        Debug.LogFormat($"�÷��̾� speed = {statHandler.MoveSpeed}");
        Debug.LogFormat($"�÷��̾� speedModifier = {statHandler.MoveSpeedModifier}");
        Debug.LogFormat($"�÷��̾� rotationDamping = {statHandler.RotationDamping}");
        Debug.LogFormat($"�÷��̾� jumpPower = {statHandler.JumpPower}");
        Debug.LogFormat($"�÷��̾� attackPower = {statHandler.AttackPower}");
        Debug.LogFormat($"�÷��̾� depencePower = {statHandler.DefensePower}");
    }
    private void SetFirstPersonVisuals(bool firstPerson)
    {
        FirstPersonRoot.SetActive(firstPerson);
        ThirdPersonRoot.SetActive(firstPerson == false);

        if (firstPerson)
        {
            // 1��Ī�� ���: LocalPlayerController �ڽĿ��� Animator ��������
            if (playerController != null)
            {
                // 1��Ī�� ��� Hands_Rifle�� Ȱ��ȭ �� ���·� �����Ͽ� Rifle�� Animator�� ����
                //firstPersonAnimator = localController.GetComponentInChildren<Animator>();
                //playerAnimator = playerController.GetComponentInChildren<Animator>();

                // ���� ��ü�Ҷ����� animator�� �˻��� �� ������ �����ϰ� �ҷ����°� ���� ����
                // 3�� �� Ȱ��ȭ �ϰ�, rifle�� ������ 2���� �ϴ� ��Ȱ��ȭ(����)
                // �켱 Weapons�� ���δ�
            }
        }
        else
        {
            if (playerController != null)
            {
                // Weapons�� �ٿ��� �Ѵ�

                //playerAnimator = playerController.GetComponent<Animator>();
                
            }
        }
    }



    /// <summary>
    /// ���������� ChangeDetector�� ȣ���� ���� ��Ʈ��ũȭ�� �Ӽ��� �߻��� ��� ���� ������ �ݺ�
    /// Render()�� Unity�� Update()ó�� Fusion�� �� �����Ӹ��� ������ �������� �ڵ����� ȣ���ϴ� �Լ���
    /// </summary>
    public override void Render()
    {
        foreach (var change in _changeDetector.DetectChanges(this))
        {
            switch (change)
            {
                case nameof(spawnedProjectile):
                    /// PlayerColor ��ũ��Ʈ�� MeshRenderer�� �����Ͽ� material�� ���� ����
                    _material.color = Color.white;
                    break;
            }
        }
        /// PlayerColor ��ũ��Ʈ�� MeshRenderer�� �����Ͽ� material�� ���� ����
        _material.color = Color.Lerp(_material.color, Color.blue, Time.deltaTime);
    }

    /// <summary>
    /// RPC ���� �޼����
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

        //_messages.text += message;
        _messages.text = message;
    }
}