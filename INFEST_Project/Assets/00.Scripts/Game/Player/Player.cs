using Cinemachine;
using Fusion;
using INFEST.Game;
using TMPro;
using UnityEngine;

/// <summary>
/// 플레이어의 기본적인 사항들
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
    public GameObject FirstPersonCamera;    // CinemachineVirtualCamera를 가지고 있는 오브젝트
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

    public TickTimer tickTimer; // 플레이어에 적용되는 Timer는 한가지로 모두 해결 가능할지도 모른다

    public override void Spawned()
    {
        NetworkGameManager.Instance.gamePlayers.AddPlayerObj(Object.InputAuthority, Object.Id);

        //_cc = GetComponent<NetworkCharacterController>();
        _forward = transform.forward;

        /// Player에 붙은 PlayerColor 스크립트의 MeshRenderer에 접근하여 material을 가져온다
        _material = GetComponentInChildren<MeshRenderer>().material;
        inventory = GetComponent<Inventory>();

        // CinemachineVirtualCamera가 포함된 게임오브젝트를 비활성화한 상태로 시작하므로 
        if (HasInputAuthority == false)
        {
            // 다른 플레이어의 CinemachineVirtualCamera는 우선순위 낮춘다
            FirstPersonCamera.GetComponent<CinemachineVirtualCamera>().Priority = 0;
        }

        /// 기존의 데이터
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

        // 튜토리얼에 있던 부분, 삭제하니까 오류가 많이 나서 일단 남겨두었다
        _changeDetector = GetChangeDetector(ChangeDetector.Source.SimulationState);

        name = $"{Object.InputAuthority} ({(HasInputAuthority ? "Input Authority" : (HasStateAuthority ? "State Authority" : "Proxy"))})";

        // Enable first person visual for local player, third person visual for proxies.
        SetFirstPersonVisuals(HasInputAuthority);

        if (Object.HasInputAuthority) // 로컬 플레이어 본인일 때
        {
            // FirstPersonCamera
            //FirstPersonCamera.SetActive(true);

            FirstPersonCamera.GetComponent<CinemachineVirtualCamera>().Priority = 100;  // 우선순위를 높이면

            //inventory.consume[0] = Consumes.Consumes[0];
            //inventory.consume[1] = Consumes.Consumes[3];
            //inventory.AddConsumeItme(inventory.consume[0]);
            //inventory.AddConsumeItme(inventory.consume[1]);

            Global.Instance.UIManager.Show<UIStateView>();
            Global.Instance.UIManager.Show<UIBrightView>();
        }
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

    private void SetFirstPersonVisuals(bool firstPerson)
    {
        HandRoot.SetActive(firstPerson);
        ThirdPersonRoot.SetActive(firstPerson == false);
    }
}