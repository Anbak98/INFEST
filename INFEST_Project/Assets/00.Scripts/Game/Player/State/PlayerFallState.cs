using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerFallState : PlayerAirState
{
    public PlayerFallState(PlayerController controller, PlayerStateMachine stateMachine) : base(controller, stateMachine)
    {
    }


    public override void Enter()
    {
        // ���鿡�� �������� && y���� �ӵ��� 0���� �۰ų� ������ Fall

        base.Enter();
    }
    public override void OnUpdate(NetworkInputData data)
    {
        base.OnUpdate(data);
        // �÷��̾� �̵�
        PlayerMove(data);
        controller.ApplyGravity();  // �߷�


        // ���� ���� ������(IsGrounded == true) �߷��� �޴´�
        if (controller.IsGrounded())
        {
            stateMachine.ChangeState(stateMachine.IdleState);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}
