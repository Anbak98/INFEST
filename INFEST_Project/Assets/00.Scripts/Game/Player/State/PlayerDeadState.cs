using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �ɾ� �ִ� ����
// SitIdle, Waddle, SitAttack, SitReload
public class PlayerDeadState : PlayerBaseState
{
    float respawnTime;  // 30��
    // �÷��̾��� respawn�� ���� ... spawn point�� ����Ǿ��ִ� spawner�� �˾ƾ� �Ѵ�
    public MVPStageSpawner stageSpawner;    // ������ �� �����ؾ߰ڴ�
    // ���� ���: spawner > player > camera �� ������ �� �ִ�
     
    // ī�޶� ���°� ����ȭ�� �ʿ�� ����. PlayerRef���� Player
    List<Player> alivePlayerList = new List<Player>();

    public PlayerDeadState(PlayerController controller, PlayerStateMachine stateMachine) : base(controller, stateMachine)
    {
        stageSpawner = GameObject.FindFirstObjectByType<MVPStageSpawner>();
        Debug.Log("stageSpawner: "+ stageSpawner.name);
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
        if (data.isChangingCamera)
        {
            // �޼��� ȣ��
            FindAlivePlayers(); // �� �����Ӹ��� ������ ����

        }

        // 30�� �� ü�� 100 ä��� Idle ���·� ��ȯ
        if (respawnTime >= 30f)
        {
            // �ڽ��� spawn �Ǿ��� ��ġ��
            player.transform.position = stageSpawner.playerSpawnPoint.transform.position;


            statHandler.CurrentHealth = 100;
            player.FirstPersonRoot.SetActive(true);
            stateMachine.ChangeState(stateMachine.IdleState);
        }
        else
        {
            respawnTime += Time.deltaTime;
            // �ð� �Ҽ��� ���ϴ��� �߶� ����ϸ� ������
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
        // ����Ʈ���� �˻� ��, ����ִ°��� �����Ѵ�
        //if (stageSpawner.spawnedCharacters.TryGetValue())
    }

    // �ٸ� �÷��̾��� ī�޶� ��ȯ(�������)
    public void SpectorMode()
    {
        



    }




}
