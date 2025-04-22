using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �ɾ� �ִ� ����
// SitIdle, Waddle, SitAttack, SitReload
public class PlayerSitState : PlayerGroundState
{
    public PlayerSitState(PlayerController controller, PlayerStateMachine stateMachine) : base(controller, stateMachine)
    {
    }

    public override void Enter()
    {
        // collider�� ũ�⸦ �������� ���δ�
        player.characterController.height /= 2;

        Debug.Log("Sit���� ����");
        base.Enter();
        
        StartAnimation(stateMachine.Player.AnimationData.SitParameterHash);
        // �ɴ´�
        controller.StartSit();
    }
    public override void Exit()
    {
        // collider�� ũ�⸦ 2��� �ø���

        base.Exit();
        StopAnimation(stateMachine.Player.AnimationData.SitParameterHash);
        // �Ͼ��
        controller.StartStand();

        player.characterController.height *= 2;
    }

    public override void OnUpdate(NetworkInputData data)
    {
        if (!data.isSitting)
        {
            stateMachine.ChangeState(stateMachine.IdleState);
        }
    }
    public override void PhysicsUpdate()
    {
    }
}
