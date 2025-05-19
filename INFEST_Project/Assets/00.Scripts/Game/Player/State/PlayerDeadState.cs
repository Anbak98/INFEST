using Cinemachine;
using Fusion;
using INFEST.Game;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeadState : PlayerBaseState
{
    float respawnTime = 30f;  // 30��(��Ʈ��ũ �ð� ����ȭ �ʿ�)

    //public MVPStageSpawner stageSpawner;
    //List<Player> alivePlayerList = new List<Player>();


    List<CinemachineVirtualCamera> alivePlayerCameras = new List<CinemachineVirtualCamera>();
    public int currentPlayerIndex = 0;

    List<PlayerRef> playerRefs = new List<PlayerRef>();

    public PlayerDeadState(PlayerController controller, PlayerStateMachine stateMachine) : base(controller, stateMachine)
    {
    }

    public override void Enter()
    {
        player.animationController.Die = true;
        stateMachine.IsDead = true;

        player.FirstPersonRoot.SetActive(false);

        //// ���� �������� �÷��̾� ������ ����
        //playerRefs = NetworkGameManager.Instance.gamePlayers.GetPlayerRefs();

        // �÷��̾ ������ timer�� ���� Ÿ�Ӹ� ���ư����Ѵ�(�÷��̾ ����Ǵ� �ð� �߿��� ���� �� �ð��� ������ �ð��̹Ƿ�, ������ �ð��� ����� �ų� ��������
        player.tickTimer = TickTimer.CreateFromSeconds(player.Runner, respawnTime); // �ڽ��� runner�� player.Runner
    }

    public override void Exit()
    {
        player.FirstPersonRoot.SetActive(true);
    }

    public override void OnUpdate(NetworkInputData data)
    {
        // Jump, Fall ���¿��� �׾��ٸ� �ٴڿ� ���� ������ ���������Ѵ�
        controller.ApplyGravity();


        // Ű �Է��� ���� ����(�ٸ� �÷��̾��� ī�޶��)
        if (data.isUsingHeal)  // E �Է�
        {
            FindAlivePlayers(); // ������ ����

            Debug.Log("���� �÷��̾� ī�޶�� �����մϴ� ");
            // ���� �ε����� ī�޶� �����´�
            if (alivePlayerCameras.Count > 0)
            {
                currentPlayerIndex = (currentPlayerIndex + 1) % alivePlayerCameras.Count;
                SetSpectatorTarget(alivePlayerCameras[currentPlayerIndex]);
            }
        }
        if (data.isChangingCamera)  // Q �Է�
        {
            FindAlivePlayers(); // ������ ����

            Debug.Log("���� �÷��̾� ī�޶�� �����մϴ� ");
            // ���� �ε����� ī�޶� �����´�
            if (alivePlayerCameras.Count > 0)
            {
                currentPlayerIndex = (alivePlayerCameras.Count + currentPlayerIndex - 1) % alivePlayerCameras.Count;
                SetSpectatorTarget(alivePlayerCameras[currentPlayerIndex]);
            }
        }

        // 30�� �� ü�� 100 ä��� Idle ���·� ��ȯ
        Debug.Log(player.tickTimer);   // �׽�Ʈ
        if (player.tickTimer.Expired(player.Runner))
        {
            // �÷��̾� ������
            int randomIndex = UnityEngine.Random.Range(0, NetworkGameManager.Instance.gamePlayers.PlayerSpawnPoints.Count);
            player.transform.position = NetworkGameManager.Instance.gamePlayers.PlayerSpawnPoints[randomIndex].transform.position;


            statHandler.SetHealth(100);
            player.animationController.Die = false;
            stateMachine.IsDead = false;
            player.FirstPersonRoot.SetActive(true); // FirstPersonRoot�� ����ó��

            // ������� �ʱ�ȭ
            ResetSpectatorTarget(alivePlayerCameras[currentPlayerIndex]);

            // �ڽ��� virtual camera�� priority�� �ٽ� 100����
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

    // ����ִ� �ٸ� �÷��̾���� ī�޶� �����ϴµ�, �ǽð����� �ؾ� �Ѵٴ� ������ �ִ�
    // �̹� �����ϰ� �ִ� ��밡 ���ڱ� �׾�����ٸ�?
    // �Է��� ���� �ʴ� ���ȿ��� ���ΰ�����, �Է��� �ϸ�, ���� �˻��ؼ� ����ִ°����� List�� ����
    public void FindAlivePlayers()
    {
        // ���� �������� �÷��̾� ������ ����
        playerRefs = NetworkGameManager.Instance.gamePlayers.GetPlayerRefs();

        // CinemachineVirtualCamera�� priority�� 0�ʱ�ȭ
        foreach (var virtualCamera in alivePlayerCameras)
        {
            virtualCamera.Priority = 0;
        }
        alivePlayerCameras.Clear();

        // playerRefs�� �ִ� �÷��̾���� virtualCamera�� ����
        foreach (var playerRef in playerRefs)
        {
            // NetworkObject ��������
            if (player.Runner.TryGetPlayerObject(playerRef, out NetworkObject netObj))
            {
                // Player ������Ʈ ����
                Player otherPlayer = netObj.GetComponent<Player>();
                if (otherPlayer != null && otherPlayer.statHandler.CurHealth > 0)
                {
                    PlayerCameraHandler otherCamHandler = otherPlayer.GetComponentInChildren<PlayerCameraHandler>();
                    alivePlayerCameras.Add(otherCamHandler.virtualCamera);
                }
            }
        }

        // ������ üũ(����ִ� �÷��̾�鸸 alivePlayerCameras�� ���ܾ��Ѵ�)
        // �����ڴ� ��� ����ǹǷ� �� �����Ӹ��� üũ�Ѵ�
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
        //        //    cam = otherPlayerObj.GetComponentInChildren<CinemachineVirtualCamera>(true); // inactive ����
        //        //    otherCamHandler.virtualCamera = cam;    // null�̴� �߰�������Ѵ�
        //        //}
        //        alivePlayerCameras.Add(cam);
        //    }
        //}

    }

    // �ٸ� �÷��̾��� ī�޶� ��ȯ(�������)
    public void SetSpectatorTarget(CinemachineVirtualCamera targetCam)
    {
        if (targetCam == null) return;
        // �ڽ��� virtualcamera�� priority�� 0���� 
        player.FirstPersonCamera.GetComponent<CinemachineVirtualCamera>().Priority = 0;

        // Ÿ���� �켱������ ���̸� �ڵ����� Live
        targetCam.Priority = 100;

        // targetCam�� ���� ������Ʈ�� 3��Ī ������ ��Ȱ��ȭ, 1��Ī ������ Ȱ��ȭ
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
        // �켱������ ���߸� �ڵ����� Standby
        targetCam.Priority = 0;

        // �ڽ��� �켱���� 100���� 
        player.FirstPersonCamera.GetComponent<CinemachineVirtualCamera>().Priority = 100;

        // targetCam�� ���� ������Ʈ�� 3��Ī ������ Ȱ��ȭ, 1��Ī ������ ��Ȱ��ȭ
        var targetPlayer = targetCam.GetComponentInParent<Player>();
        if (targetPlayer != null)
        {
            targetPlayer.FirstPersonRoot.SetActive(false);
            targetPlayer.ThirdPersonRoot.SetActive(true);
        }
    }
}
