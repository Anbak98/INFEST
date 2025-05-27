using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSitReloadState : PlayerSitState
{
    private bool prevIsReloading = false;

    public PlayerSitReloadState(PlayerController controller, PlayerStateMachine stateMachine) : base(controller, stateMachine)
    {
    }

    public override void Enter()
    {
        Debug.Log("SitReload상태 진입");
        base.Enter();

        // Sit && Reload
        //StartAnimation(stateMachine.Player.AnimationData.ReloadParameterHash);
    }
    public override void Exit()
    {
        base.Exit();
        //StopAnimation(stateMachine.Player.AnimationData.ReloadParameterHash);
    }

    public override void OnUpdate(NetworkInputData data)
    {
        // 이동하면서 재장전 가능하다
        PlayerWaddle(data);

        // false → true로 바뀌는 순간만 감지 (즉, 입력이 딱 들어온 그 순간)
        if (!prevIsReloading && data.isReloading)
        {
            SitReload(data);
        }

        // reloading 끝났으면 상태 나가기
        if (!data.isReloading)
        {
            stateMachine.ChangeState(stateMachine.SitIdleState);
        }

        prevIsReloading = data.isReloading; // 다음 frame을 위한 저장
    }
}
