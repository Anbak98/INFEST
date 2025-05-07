using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 앉아 있는 상태
// SitIdle, Waddle, SitAttack, SitReload
public class PlayerDeadState : PlayerBaseState
{
    float respawnTime = 0f;  // 30초
    // 플레이어의 respawn을 위해 ... spawn point가 저장되어있는 spawner를 알아야 한다
    public MVPStageSpawner stageSpawner;    // 생성될 때 연결해야겠다
    // 관전 모드: spawner > player > camera 로 접근할 수 있다
     
    // 카메라를 보는건 동기화할 필요는 없다. PlayerRef말고 Player
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
        // Jump, Fall 상태에서 죽었다면 바닥에 닿을 때까지 떨어져야한다
        controller.ApplyGravity();

        // 키 입력을 통한 관전(다른 플레이어의 카메라로)
        if (data.isChangingCamera)
        {
            // 메서드 호출
            FindAlivePlayers(); // 매 프레임마다 생존자 갱신
        }
        Debug.Log(respawnTime);
        // 30초 후 체력 100 채우고 Idle 상태로 전환
        if (respawnTime >= 30f)
        {
            // 자신이 spawn 되었던 위치로
            player.transform.position = stageSpawner.playerSpawnPoint.transform.position;

            statHandler.SetHealth(100);
            player.animationController.Die = false;
            player.stateMachine.IsDead = false;
            player.FirstPersonRoot.SetActive(true);
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
        // 리스트에서 검색 후, 살아있는것을 저장한다
        //if (stageSpawner.spawnedCharacters.TryGetValue())
        //{

        //}

    }

    // 다른 플레이어의 카메라 전환(관전모드)
    public void SpectorMode()
    {
        



    }




}
