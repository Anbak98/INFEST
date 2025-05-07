using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerIdleState : PlayerGroundState
{
    public PlayerIdleState(PlayerController controller, PlayerStateMachine stateMachine) : base(controller, stateMachine)
    {
    }

    // 가장 먼저 시작
    public override void Enter()
    {
        // 일단 숫자대입. 나중에 PlayStatData.WalkSpeedModifier 변수 추가해서 그,값으로 바꾼다
        stateMachine.StatHandler.MoveSpeedModifier = 0;

        base.Enter();   // Ground 진입
        //Debug.Log("Idle상태 진입");

        // Ground && Idle
        //StartAnimation(stateMachine.Player.AnimationData.IdleParameterHash);
    }

    public override void Exit()
    {
        base.Exit();
        //StopAnimation(stateMachine.Player.AnimationData.IdleParameterHash);
    }

    public override void PhysicsUpdate(NetworkInputData data)
    {
        base.PhysicsUpdate(data);
    }

    public override void OnUpdate(NetworkInputData data)
    {
        base.OnUpdate(data);


        if ((controller.IsGrounded()) && data.isJumping)
        {
            player.animationController.isJumping = data.isJumping;
            stateMachine.ChangeState(stateMachine.JumpState);
        }
        // 정지한 상태에서 좌우회전
        if (data.direction != Vector3.zero)
        {
            stateMachine.ChangeState(stateMachine.MoveState);
        }
        // 일단 샷건(isShotgunOnFiring)은 미작성
        if (stateMachine.Player.GetWeapons() != null && data.isFiring)
        {
            // 이전 frame의 정보와 state가 바뀐 다음 frame의 정보가 다르기 때문에
            // 이전 frame에서 _isShotgunOnFiring true라도 다음 frame에서 false가 되기 때문이다
            // 따라서 이번 프레임에서 1번 쏘고 넘어가야한다
            // rifle인 경우에 다음프레임에서도 계속 발사하며
            // pistol인 경우에는 다음 프레임에 발사하지 않는다
            player.animationController.isFiring = data.isFiring;
            PlayerFire(data);

            stateMachine.ChangeState(stateMachine.AttackState);
        }
        if (controller.IsGrounded() && data.isZooming)
        {
            stateMachine.ChangeState(stateMachine.AimState);
        }


        // 앉는 상태는 잠금
        //if (controller.IsGrounded() && data.isSitting)
        //{
        //    stateMachine.ChangeState(stateMachine.SitIdleState);
        //}
    }
}
