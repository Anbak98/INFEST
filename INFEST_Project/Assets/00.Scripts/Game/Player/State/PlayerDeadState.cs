using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 앉아 있는 상태
// SitIdle, Waddle, SitAttack, SitReload
public class PlayerDeadState : PlayerBaseState
{
    float respawnTime = 0f;  // 30초(네트워크 시간 동기화 필요)
    //public MVPStageSpawner stageSpawner;
    //List<Player> alivePlayerList = new List<Player>();

    List<CinemachineVirtualCamera> alivePlayerCameras = new List<CinemachineVirtualCamera>();
    public int currentPlayerIndex = 0;

    public PlayerDeadState(PlayerController controller, PlayerStateMachine stateMachine) : base(controller, stateMachine)
    {
        //stageSpawner = GameObject.FindFirstObjectByType<MVPStageSpawner>();
        //Debug.Log("stageSpawner: " + stageSpawner.name);
    }

    public override void Enter()
    {
        player.animationController.Die = true;
        player.stateMachine.IsDead = true;
        Debug.Log("????");

        player.FirstPersonRoot.SetActive(false);
    }

    public override void Exit()
    {
    }

    public override void OnUpdate(NetworkInputData data)
    {
        // Jump, Fall 상태에서 죽었다면 바닥에 닿을 때까지 떨어져야한다
        controller.ApplyGravity();


        // 키 입력을 통한 관전(다른 플레이어의 카메라로)
        if (data.isUsingHeal)  // E 입력
        {
            FindAlivePlayers(); // 생존자 갱신

            Debug.Log("다음 플레이어 카메라로 관전합니다 ");
            // 다음 인덱스의 카메라를 가져온다
            if (alivePlayerCameras.Count > 0)
            {
                currentPlayerIndex = (currentPlayerIndex + 1) % alivePlayerCameras.Count;
                SetSpectatorTarget(alivePlayerCameras[currentPlayerIndex]);
            }
        }
        if (data.isChangingCamera)  // Q 입력
        {
            FindAlivePlayers(); // 생존자 갱신

            Debug.Log("이전 플레이어 카메라로 관전합니다 ");
            // 이전 인덱스의 카메라를 가져온다
            if (alivePlayerCameras.Count > 0)
            {
                currentPlayerIndex = (alivePlayerCameras.Count + currentPlayerIndex - 1) % alivePlayerCameras.Count;
                SetSpectatorTarget(alivePlayerCameras[currentPlayerIndex]);
            }
        }


        // 30초 후 체력 100 채우고 Idle 상태로 전환
        //Debug.Log(respawnTime);   // 네트워크 동기화가 필요함
        if (respawnTime >= 30f)
        {
            // 자신이 spawn 되었던 위치로
            //player.transform.position = stageSpawner.playerSpawnPoint.transform.position;

            statHandler.SetHealth(100);
            player.animationController.Die = false;
            player.stateMachine.IsDead = false;
            player.FirstPersonRoot.SetActive(true); // FirstPersonRoot는 로컬처리

            // 관전모드 초기화
            ResetSpectatorTarget(alivePlayerCameras[currentPlayerIndex]);

            // 자신의 virtual camera의 priority를 다시 100으로
            respawnTime = 0f;
            stateMachine.ChangeState(stateMachine.IdleState);
        }
        else
        {
            respawnTime += Time.deltaTime;
        }
    }

    public override void PhysicsUpdate(NetworkInputData data)
    {
    }

    // 살아있는 다른 플레이어들의 카메라를 저장하는데, 실시간으로 해야 한다는 문제가 있다
    // 이미 관전하고 있는 상대가 갑자기 죽어버린다면?
    // 입력을 하지 않는 동안에는 놔두겠지만, 입력을 하면, 새로 검사해서 살아있는것으로 List를 갱신
    public void FindAlivePlayers()
    {
        // CinemachineVirtualCamera의 priority를 0초기화
        foreach (var virtualCamera in alivePlayerCameras)
        {
            virtualCamera.Priority = 0;
        }
        alivePlayerCameras.Clear();

        // 생존자 체크(살아있는 플레이어들만 alivePlayerCameras에 남겨야한다)
        // 생존자는 계속 변경되므로 매 프레임마다 체크한다
        //foreach (var kvp in stageSpawner.spawnedCharacters)
        //{
        //    var otherPlayerObj = kvp.Value;
        //    var otherPlayer = otherPlayerObj.GetComponent<Player>();
        //    var otherCamHandler = otherPlayerObj.GetComponentInChildren<PlayerCameraHandler>();
        //    if (otherPlayer.statHandler.CurrentHealth > 0)
        //    {
        //        var cam = otherCamHandler.virtualCamera;
        //        //if (cam == null)
        //        //{
        //        //    cam = otherPlayerObj.GetComponentInChildren<CinemachineVirtualCamera>(true); // inactive 포함
        //        //    otherCamHandler.virtualCamera = cam;    // null이니 추가해줘야한다
        //        //}
        //        alivePlayerCameras.Add(cam);
        //    }
        //}
    }

    // 다른 플레이어의 카메라 전환(관전모드)
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
}
