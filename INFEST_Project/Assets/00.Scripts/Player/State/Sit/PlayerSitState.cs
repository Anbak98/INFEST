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

        // collider�� ũ�⸦ �������� ���δ�
        //player.characterController.height /= 2;        

        //Debug.Log("Sit���� ����");
        base.Enter();

        // �ɴ´�
        controller.StartSit();
    }
    public override void Exit()
    {
        // collider�� ũ�⸦ 2��� �ø���

        base.Exit();
        // �Ͼ��
        controller.StartStand();

        //player.characterController.height *= 2;
    }

    public override void OnUpdate(NetworkInputData data)
    {
        base.OnUpdate(data);

        // �ɾ� �ִ� ��쿡 Layer�� ����


        //if (!data.isSitting)
        //{
        //    // �ٽ� Layer�� Base�� �����


        //    stateMachine.ChangeState(stateMachine.IdleState);
        //    return;
        //}
    }
    public override void PhysicsUpdate(NetworkInputData data)
    {
    }
}
