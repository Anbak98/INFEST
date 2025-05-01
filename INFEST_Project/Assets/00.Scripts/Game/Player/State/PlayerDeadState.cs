using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �ɾ� �ִ� ����
// SitIdle, Waddle, SitAttack, SitReload
public class PlayerDeadState : PlayerBaseState
{
    public PlayerDeadState(PlayerController controller, PlayerStateMachine stateMachine) : base(controller, stateMachine)
    {
    }

    public override void Enter()
    {
        player.animationController.Die = true;
        player.stateMachine.IsDead = true;
        Debug.Log("????");
    }

    public override void Exit()
    {
    }

    public override void OnUpdate(NetworkInputData data)
    {
        // Jump, Fall ���¿��� �׾��ٸ� �ٴڿ� ���� ������ ���������Ѵ�
        controller.ApplyGravity();  

        // Ű �Է��� ���� ����(�ٸ� �÷��̾��� ī�޶��)

    }

    public override void PhysicsUpdate(NetworkInputData data)
    {
    }
}
