using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �ɾ� �ִ� ����
// SitIdle, Waddle, SitAttack, SitReload
public class PlayerSitState : PlayerBaseState
{
    
    public PlayerSitState(PlayerController controller, PlayerStateMachine stateMachine) : base(controller, stateMachine)
    {
    }

    public override void Enter()
    {
        // �ɾ� �ִ� ��� ���¿��� �޸��� �Ұ�
        controller.LockState = PlayerLockState.RunLock;

        base.Enter();

        // �ɴ´�
        controller.StartSit();
    }
    public override void Exit()
    {
        base.Exit();
        // �Ͼ��
        controller.StartStand();
    }

    public override void OnUpdate(NetworkInputData data)
    {
        base.OnUpdate(data);
        player.animationController.isSitting = data.isSitting;

    }
    public override void PhysicsUpdate(NetworkInputData data)
    {
    }
}
