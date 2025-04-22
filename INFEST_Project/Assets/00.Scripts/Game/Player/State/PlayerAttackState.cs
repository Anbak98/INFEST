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

        StartAnimation(stateMachine.Player.AnimationData.AttackParameterHash);
    }
    public override void Exit()
    {
        base.Exit();
        StopAnimation(stateMachine.Player.AnimationData.AttackParameterHash);
    }



    public override void OnUpdate()
    {
        // blend tree �ִϸ��̼ǿ����� �Է°��� ������Ʈ�ؼ� �ִϸ��̼��� �����ؾ��Ѵ�
        bool isFire = stateMachine.InputHandler.GetIsFiring();

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
