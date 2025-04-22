using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerReloadState : PlayerBaseState
{
    private bool prevIsReloading = false;

    public PlayerReloadState(PlayerController controller, PlayerStateMachine stateMachine) : base(controller, stateMachine)
    {

    }
    public override void Enter()
    {
        Debug.Log("Reload상태 진입");
        base.Enter();

        StartAnimation(stateMachine.Player.AnimationData.ReloadParameterHash);
    }
    public override void Exit()
    {
        base.Exit();
        StopAnimation(stateMachine.Player.AnimationData.ReloadParameterHash);
    }


    public override void Update()
    {
        // 장전 애니메이션 1회 실행 이 되고 바로 나가야하는데... 이를 어떻게 확인?
        bool currentIsReloading = stateMachine.InputHandler.GetIsReloading(); // 외부에서 bool 가져오기

        // 이동하면서 재장전 가능하다
        PlayerMove();

        // false → true로 바뀌는 순간만 감지 (즉, 입력이 딱 들어온 그 순간)
        if (!prevIsReloading && currentIsReloading)
        {
            Reload();
        }

        // reloading 끝났으면 상태 나가기
        if (!currentIsReloading)
        {
            stateMachine.ChangeState(stateMachine.IdleState);
        }

        prevIsReloading = currentIsReloading; // 다음 frame을 위한 저장
    }

    // 끝난 다음에는 다시 idle로 되돌아간다
}
