using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �ɾ� �ִ� ����
// SitIdle, Waddle, SitAttack, SitReload
public class PlayerDeadState : PlayerBaseState
{
    float respawnTime = 0f;  // 30��(��Ʈ��ũ �ð� ����ȭ �ʿ�)
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
        //Debug.Log(respawnTime);   // ��Ʈ��ũ ����ȭ�� �ʿ���
        if (respawnTime >= 30f)
        {
            // �ڽ��� spawn �Ǿ��� ��ġ��
            //player.transform.position = stageSpawner.playerSpawnPoint.transform.position;

            statHandler.SetHealth(100);
            player.animationController.Die = false;
            player.stateMachine.IsDead = false;
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
        // CinemachineVirtualCamera�� priority�� 0�ʱ�ȭ
        foreach (var virtualCamera in alivePlayerCameras)
        {
            virtualCamera.Priority = 0;
        }
        alivePlayerCameras.Clear();

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
