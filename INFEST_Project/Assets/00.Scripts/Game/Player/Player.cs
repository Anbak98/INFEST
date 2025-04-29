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

    // playerController�� Input�� ����
    public PlayerController playerController;     // 1��Ī: LocalPlayerController, 3��Ī: RemotePlayerController
    public PlayerStatHandler statHandler;

    public CharacterController characterController; // collider, rigidbody �� ����Ǿ��ִ�
    public NetworkCharacterController networkCharacterController;
    public PlayerAttackedEffectController attackedEffectController;

    public PlayerStateMachine stateMachine;
    public PlayerCameraHandler cameraHandler;

    public bool inStoreZoon = false;
    public bool isInteraction = false;
    public Store store;
    public Inventory inventory;
    public CharacterInfoInstance characterInfoInstance;
    #region ������ ������
    //private NetworkCharacterController _cc;
    private Vector3 _forward = Vector3.forward;
    public WeaponSpawner Weapons;// SY

    [Header("Setup")]
    public float MoveSpeed = 6f;
    public float JumpForce = 10f;
    //public AudioSource JumpSound;
    //public AudioClip[] JumpClips;
    public Transform CameraHandle;
    public GameObject FirstPersonRoot;
    public GameObject ThirdPersonRoot;
    //public NetworkObject SprayPrefab;


    // ��Ʈ��ũ �Ӽ��� ������ �� Fusion�� ������ get �� set ������ ����� ���� �ڵ�� ��ü�Ͽ� ��Ʈ��ũ ���¿� ����
    // �̴� ���ø����̼��� �̷��� ����� ����Ͽ� �Ӽ� ���� ��ȭ�� ó���� �� ������
    // ������ setter �޼ҵ带 ����� ���� ���ÿ����� �۵��Ѵٴ� ���� �ǹ�
    [Networked] public bool spawnedProjectile { get; set; }

    private ChangeDetector _changeDetector;

    public Material _material;


    private TMP_Text _messages;
    #endregion

    private void Awake()
    {
        /// ������ ������
        //_cc = GetComponent<NetworkCharacterController>();
        _forward = transform.forward;
        /// Player�� ���� PlayerColor ��ũ��Ʈ�� MeshRenderer�� �����Ͽ� material�� �����´�
        _material = GetComponentInChildren<MeshRenderer>().material;
        inventory = GetComponent<Inventory>();
        statHandler.OnHealthChanged += attackedEffectController.CalledWhenPlayerAttacked;
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
            if(HasStateAuthority)
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

        if(characterInfoInstance == null) // ĳ�����ν��Ͻ�, �κ��丮 ����
        {
            characterInfoInstance = new(1);

            for(int i=0; i< Weapons.Weapons.Count; i++)
            {
                if (Weapons.Weapons[i].key == characterInfoInstance.data.StartAuxiliaryWeapon)
                    inventory.auxiliaryWeapon[0] = Weapons.Weapons[i];
                if (Weapons.Weapons[i].key == characterInfoInstance.data.StartWeapon1)
                    inventory.weapon[0] = Weapons.Weapons[i];

                if (inventory.auxiliaryWeapon[0] != null && inventory.weapon[0] != null)
                    break;
            }
            //inventory.auxiliaryWeapon[0] = Weapons.weapons[i] (characterInfoInstance.data.StartAuxiliaryWeapon);
            //inventory.weapon[0] =  new WeaponInstance(characterInfoInstance.data.StartWeapon1);
            #region üũ�� bool ��
            int itemChk = characterInfoInstance.data.StartConsumeItem1 % 10000;
            bool throwingWeapon = itemChk < 800 && itemChk > 700;
            bool recoveryItem = itemChk < 900 && itemChk > 800;
            bool shieldItme = itemChk < 1000 && itemChk > 900;
            #endregion
            if (throwingWeapon)
                inventory.consume[0] = new ConsumeInstance(characterInfoInstance.data.StartConsumeItem1);
            if (recoveryItem)
                inventory.consume[1] = new ConsumeInstance(characterInfoInstance.data.StartConsumeItem1);
            if (shieldItme)
                inventory.consume[2] = new ConsumeInstance(characterInfoInstance.data.StartConsumeItem1);

            inventory.equippedWeapon = inventory.auxiliaryWeapon[0];
        }

        /// ����׿�
        /// �ν����� â���� �� ����
        //statHandler.Init(200, 3, 2, 5, 8, 50, 60);
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
}