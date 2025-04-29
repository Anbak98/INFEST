using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAimState : PlayerGroundState
{
    public PlayerAimState(PlayerController controller, PlayerStateMachine stateMachine) : base(controller, stateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }
    public override void Exit()
    {
        base.Exit();
    }

    public override void OnUpdate(NetworkInputData data)
    {
        // Aim ���¿����� Idle, AimWalk, AimAttack���� ��ȯ ����
        if (data.direction != Vector3.zero)
        {
            stateMachine.ChangeState(stateMachine.AimWalkState);
        }
        if (data.isFiring)
        {
            stateMachine.ChangeState(stateMachine.AimAttackState);
        }
        if (!data.isZooming)
        {
            stateMachine.ChangeState(stateMachine.IdleState);
        }
    }
}
