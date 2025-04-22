using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSitAttackState : PlayerSitState
{
    public PlayerSitAttackState(PlayerController controller, PlayerStateMachine stateMachine) : base(controller, stateMachine)
    {
    }

    public override void Enter()
    {
        Debug.Log("SitAttack���� ����");
        base.Enter();

        // Sit && Attack
        StartAnimation(stateMachine.Player.AnimationData.AttackParameterHash);
    }
    public override void Exit()
    {
        base.Exit();    // ����� layer�� ������
        StopAnimation(stateMachine.Player.AnimationData.AttackParameterHash);
    }
    public override void OnUpdate(NetworkInputData data)
    {
        bool isFire = data.isFiring;

        // ���
        PlayerSitFire();
        controller.ApplyGravity();  // �߷�

        // �̵� �Է��� ������ Idle ���·�
        if (!isFire)
        {
            stateMachine.ChangeState(stateMachine.SitIdleState);
        }

    }
    public override void PhysicsUpdate()
    {
    }
}
