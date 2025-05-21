using Cinemachine;
using Fusion;
using INFEST.Game;
using TMPro;
using UnityEngine;

/// <summary>
/// �÷��̾��� �⺻���� ���׵�
/// </summary>
public class Player : NetworkBehaviour
{
    public bool IsDead = false;
    public bool inStoreZoon = false;
    public bool isInteraction = false;
    public PlayerAnimationController animationController;
    public PlayerStatHandler statHandler;
    public PlayerController controller;
    public PlayerAttackedEffectController attackedEffectController;
    public PlayerCameraHandler cameraHandler;
    public Store store;
    public Inventory inventory;
    public GameObject FirstPersonCamera;    // CinemachineVirtualCamera�� ������ �ִ� ������Ʈ
    private Vector3 _forward = Vector3.forward;
    public WeaponSpawner Weapons;// SY
    public ConsumeSpawner Consumes;// SY
    public Transform CameraHandle;
    public GameObject FirstPersonRoot;
    public GameObject ThirdPersonRoot;
    public GameObject HandRoot;
    private ChangeDetector _changeDetector;
    public Material _material;
    private TMP_Text _messages;

    [Networked] public bool spawnedProjectile { get; set; }

    public TickTimer tickTimer; // �÷��̾ ����Ǵ� Timer�� �Ѱ����� ��� �ذ� ���������� �𸥴�

    public override void Spawned()
    {
        NetworkGameManager.Instance.gamePlayers.AddPlayerObj(Object.InputAuthority, Object.Id);

        //_cc = GetComponent<NetworkCharacterController>();
        _forward = transform.forward;

        /// Player�� ���� PlayerColor ��ũ��Ʈ�� MeshRenderer�� �����Ͽ� material�� �����´�
        _material = GetComponentInChildren<MeshRenderer>().material;
        inventory = GetComponent<Inventory>();

        // CinemachineVirtualCamera�� ���Ե� ���ӿ�����Ʈ�� ��Ȱ��ȭ�� ���·� �����ϹǷ� 
        if (HasInputAuthority == false)
        {
            // �ٸ� �÷��̾��� CinemachineVirtualCamera�� �켱���� �����
            FirstPersonCamera.GetComponent<CinemachineVirtualCamera>().Priority = 0;
        }

        /// ������ ������
        statHandler.Init();
        Weapons.Init();
        controller.Init();

        for (int i = 0; i < Consumes.Consumes.Count; i++)
        {
            if (Consumes.Consumes[i].key == statHandler.info.data.StartConsumeItem1)
            {
                int itemChk = statHandler.info.data.StartConsumeItem1 % 10000;
                bool throwingWeapon = itemChk < 800 && itemChk > 700;
                bool recoveryItem = itemChk < 900 && itemChk > 800;
                bool shieldItme = itemChk < 1000 && itemChk > 900;

                if (throwingWeapon)
                {
                    inventory.consume[0] = Consumes.Consumes[i];
                    inventory.AddConsumeItme(inventory.consume[0]);
                }

                if (recoveryItem)
                {
                    inventory.consume[1] = Consumes.Consumes[i];
                    inventory.AddConsumeItme(inventory.consume[1]);
                }

                if (shieldItme)
                {
                    inventory.consume[2] = Consumes.Consumes[i];
                    inventory.AddConsumeItme(inventory.consume[2]);

                }
                break;

            }
        }

        // Ʃ�丮�� �ִ� �κ�, �����ϴϱ� ������ ���� ���� �ϴ� ���ܵξ���
        _changeDetector = GetChangeDetector(ChangeDetector.Source.SimulationState);

        name = $"{Object.InputAuthority} ({(HasInputAuthority ? "Input Authority" : (HasStateAuthority ? "State Authority" : "Proxy"))})";

        // Enable first person visual for local player, third person visual for proxies.
        SetFirstPersonVisuals(HasInputAuthority);

        if (Object.HasInputAuthority) // ���� �÷��̾� ������ ��
        {
            // FirstPersonCamera
            //FirstPersonCamera.SetActive(true);

            FirstPersonCamera.GetComponent<CinemachineVirtualCamera>().Priority = 100;  // �켱������ ���̸�

            //inventory.consume[0] = Consumes.Consumes[0];
            //inventory.consume[1] = Consumes.Consumes[3];
            //inventory.AddConsumeItme(inventory.consume[0]);
            //inventory.AddConsumeItme(inventory.consume[1]);

            Global.Instance.UIManager.Show<UIStateView>();
            Global.Instance.UIManager.Show<UIBrightView>();
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

    private void SetFirstPersonVisuals(bool firstPerson)
    {
        HandRoot.SetActive(firstPerson);
        ThirdPersonRoot.SetActive(firstPerson == false);
    }
}