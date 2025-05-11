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

    public bool inStoreZoon = false;
    public bool isInteraction = false;
    public PlayerAnimationController animationController;
    [HideInInspector] public PlayerData data; 
    [HideInInspector] public PlayerStatHandler statHandler;
    public PlayerAttackedEffectController attackedEffectController;
    public PlayerCameraHandler cameraHandler;
    public Store store;
    public Inventory inventory;
    public CharacterInfoInstance characterInfoInstance;
    public GameObject FirstPersonCamera;    // CinemachineVirtualCamera�� ������ �ִ� ������Ʈ
    private Vector3 _forward = Vector3.forward;
    public WeaponSpawner Weapons;// SY
    public ConsumeSpawner Consumes;// SY
    public float MoveSpeed = 6f;
    public float JumpForce = 10f;
    public Transform CameraHandle;
    public GameObject FirstPersonRoot;
    public GameObject ThirdPersonRoot;
    private ChangeDetector _changeDetector;
    public Material _material;
    private TMP_Text _messages;

    [Networked] public bool spawnedProjectile { get; set; }

    public override void Spawned()
    {
        /// ������ ������
        //_cc = GetComponent<NetworkCharacterController>();
        _forward = transform.forward;
        /// Player�� ���� PlayerColor ��ũ��Ʈ�� MeshRenderer�� �����Ͽ� material�� �����´�
        _material = GetComponentInChildren<MeshRenderer>().material;
        inventory = GetComponent<Inventory>();

        // Ʃ�丮�� �ִ� �κ�, �����ϴϱ� ������ ���� ���� �ϴ� ���ܵξ���
        _changeDetector = GetChangeDetector(ChangeDetector.Source.SimulationState);

        name = $"{Object.InputAuthority} ({(HasInputAuthority ? "Input Authority" : (HasStateAuthority ? "State Authority" : "Proxy"))})";

        // Enable first person visual for local player, third person visual for proxies.
        SetFirstPersonVisuals(HasInputAuthority);

        // CinemachineVirtualCamera�� ���Ե� ���ӿ�����Ʈ�� ��Ȱ��ȭ�� ���·� �����ϹǷ� 
        if (HasInputAuthority == false)
        {
            // �ٸ� �÷��̾��� CinemachineVirtualCamera�� �켱���� �����
            FirstPersonCamera.GetComponent<CinemachineVirtualCamera>().Priority = 0;
        }

        if (Object.HasInputAuthority) // ���� �÷��̾� ������ ��
        {
            // FirstPersonCamera
            //FirstPersonCamera.SetActive(true);

            FirstPersonCamera.GetComponent<CinemachineVirtualCamera>().Priority = 100;  // �켱������ ���̸�

            local = this;
            Debug.Log("Local Player ���� �Ϸ�");
        }

        if (characterInfoInstance == null) // ĳ�����ν��Ͻ�, �κ��丮 ����
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

            for (int i = 0; i < Consumes.Consumes.Count; i++)
            {
                if (Consumes.Consumes[i].key == characterInfoInstance.data.StartConsumeItem1)
                {
                    int itemChk = characterInfoInstance.data.StartConsumeItem1 % 10000;
                    bool throwingWeapon = itemChk < 800 && itemChk > 700;
                    bool recoveryItem = itemChk < 900 && itemChk > 800;
                    bool shieldItme = itemChk < 1000 && itemChk > 900;

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
        FirstPersonRoot.SetActive(firstPerson);
        ThirdPersonRoot.SetActive(firstPerson == false);
    }

    // �ǰ�
    public void TakeDamage(int amount)
    {
        statHandler.TakeDamage(amount);
    }
}