using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttackState : PlayerGroundState
{
    public PlayerAttackState(PlayerController controller, PlayerStateMachine stateMachine, InputManager inputManager) : base(controller, stateMachine, inputManager)
    {
    }

    public override void Enter()
    {
        Debug.Log("Attack���� ����");
        base.Enter();

        StartAnimation(stateMachine.Player.AnimationData.AttackParameterHash);
    }
    public override void Exit()
    {
        base.Exit();
        StopAnimation(stateMachine.Player.AnimationData.AttackParameterHash);
    }



    public override void OnUpdate(NetworkInputData data)
    {
        // blend tree �ִϸ��̼ǿ����� �Է°��� ������Ʈ�ؼ� �ִϸ��̼��� �����ؾ��Ѵ�
        bool isFire = data.isFiring;

        // ���
        PlayerFire();
        controller.ApplyGravity();  // �߷�

        // �̵� �Է��� ������ Idle ���·�
        if (!isFire)
        {
            stateMachine.ChangeState(stateMachine.IdleState);
        }
    }
    // reload�� idle�� move����


}
