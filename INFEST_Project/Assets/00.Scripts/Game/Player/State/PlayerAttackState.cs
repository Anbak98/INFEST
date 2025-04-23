using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttackState : PlayerGroundState
{
    public PlayerAttackState(PlayerController controller, PlayerStateMachine stateMachine) : base(controller, stateMachine)
    {
    }

    public override void Enter()
    {
        Debug.Log("Attack���� ����");
        base.Enter();

        //StartAnimation(stateMachine.Player.AnimationData.AttackParameterHash);
    }
    public override void Exit()
    {
        base.Exit();
        //StopAnimation(stateMachine.Player.AnimationData.AttackParameterHash);
    }

    public override void OnUpdate(NetworkInputData data)
    {
        // blend tree �ִϸ��̼ǿ����� �Է°��� ������Ʈ�ؼ� �ִϸ��̼��� �����ؾ��Ѵ�

        // ���
        PlayerFire(data);
        controller.ApplyGravity();  // �߷�

        // �̵� �Է��� ������ Idle ���·�
        if (!data.isFiring)
        {
            stateMachine.ChangeState(stateMachine.IdleState);
        }
    }
}
