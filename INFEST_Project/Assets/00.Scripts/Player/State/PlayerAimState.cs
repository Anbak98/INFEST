using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ThirdPersonRoot는 Aim 애니메이션이 없다
// FirstPersonRoot가 가지고 있으며, Sit과 Stand의 구분이 없다
public class PlayerAimState : PlayerGroundState
{
    public PlayerAimState(PlayerController controller, PlayerStateMachine stateMachine) : base(controller, stateMachine)
    {
    }

    public override void Enter()
    {
        // 조준중에는 달리기, 점프 불가
        controller.LockState = PlayerLockState.RunLock | PlayerLockState.JumpLock;

        base.Enter();
    }
    public override void Exit()
    {
        base.Exit();
    }

    public override void OnUpdate(NetworkInputData data)
    {
        base.OnUpdate(data);

        //player.animationController.MoveDirection = data.direction;
        //PlayerMove(data);

        //player.animationController.isFiring = data.isFiring;
        //if (data.isFiring)
        //    PlayerFire(data);

        /*
        // Aim 상태에서는 Idle, AimWalk, AimAttack으로 전환 가능
        if (data.direction != Vector3.zero)
        {
            stateMachine.ChangeState(stateMachine.AimWalkState);
        }
        if (data.isFiring)
        {
            player.animationController.isFiring = data.isFiring;
            PlayerFire(data);

            stateMachine.ChangeState(stateMachine.AimAttackState);
        }
        if (!data.isZooming)
        {
            stateMachine.ChangeState(stateMachine.IdleState);
        }
        */
    }
}
