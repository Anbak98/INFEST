using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFallState : PlayerAirState
{
    public PlayerFallState(PlayerController controller, PlayerStateMachine stateMachine) : base(controller, stateMachine)
    {
    }


    public override void Enter()
    {
        // 지면에서 떨어졌다 && y방향 속도가 0보다 작거나 같으면 Fall

        base.Enter();
    }
    public override void OnUpdate(NetworkInputData data)
    {
        // false가 된 상태여야만 한다(점프, 낙하 중 점프키 입력X)
        player.animationController.isJumping = data.isJumping;

        base.OnUpdate(data);
    }

    public override void Exit()
    {
        base.Exit();
    }
}
