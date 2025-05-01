using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class PlayerMoveState : PlayerGroundState
{
    public PlayerMoveState(PlayerController controller, PlayerStateMachine stateMachine) : base(controller, stateMachine)
    {
    }

    public override void Enter()
    {
        // 일단 숫자대입. 나중에 PlayStatData.WalkSpeedModifier 변수 추가해서 그,값으로 바꾼다
        stateMachine.StatHandler.MoveSpeedModifier = 4;
        Debug.Log("Move상태 진입");
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void OnUpdate(NetworkInputData data)
    {
        base.OnUpdate(data);

        // blend tree 애니메이션에서는 입력값을 업데이트해서 애니메이션을 변경해야한다        
        player.animationController.MoveDirection = data.direction;
        PlayerMove(data);

        if (data.direction == Vector3.zero)
        {
            stateMachine.ChangeState(stateMachine.IdleState);
        }
        if (data.isRunning)
        {
            player.animationController.isRunning = data.isRunning;
            stateMachine.ChangeState(stateMachine.RunState);
        }
        if (data.isJumping)
        {
            player.animationController.isJumping = data.isJumping;
            stateMachine.ChangeState(stateMachine.JumpState);
        }


        if ((stateMachine.Player.GetWeapons() != null) && data.isFiring)
        {
            player.animationController.isFiring = data.isFiring;
            PlayerFire(data);

            stateMachine.ChangeState(stateMachine.AttackWalkState);
            //player.animationController.isFiring = data.isFiring;
            //PlayerFire(data);
        }
        if (data.isZooming)
        {
            stateMachine.ChangeState(stateMachine.AimWalkState);
        }

        //if (data.isSitting)
        //{
        //    stateMachine.ChangeState(stateMachine.SitIdleState);
        //}
    }
}
